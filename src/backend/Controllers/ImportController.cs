
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Api.Data;
using Parking.Api.Models;
using Parking.Api.Services;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

namespace Parking.Api.Controllers
{
    [ApiController]
    [Route("api/import")]
    public class ImportController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PlacaService _placa;
        public ImportController(AppDbContext db, PlacaService placa) { _db = db; _placa = placa; }

        public class ImportResult
        {
            public int Processados { get; set; }
            public int Inseridos { get; set; }
            public List<object> Erros { get; set; } = new();
        }

        [HttpPost("csv")]
        public async Task<IActionResult> ImportCsv()
        {
            if (!Request.HasFormContentType || Request.Form.Files.Count == 0)
                return BadRequest("Envie um arquivo CSV no campo 'file'.");

            var file = Request.Form.Files[0];

            using var s = file.OpenReadStream();
            using var reader = new StreamReader(s, Encoding.UTF8);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null // evita exception por bad data, tratamos manualmente
            };

            int linha = 0;
            int processados = 0;
            int inseridos = 0;
            var erros = new List<object>();

            using var csv = new CsvReader(reader, config);

            try
            {
                await csv.ReadAsync();
                csv.ReadHeader();

                while (await csv.ReadAsync())
                {
                    linha = csv.Parser.Row; // número da linha atual no CSV (considera header)
                    var rawRecord = string.Join(",", csv.Parser.Record ?? Array.Empty<string>());

                    // Count processed only for non-empty rows
                    bool allEmpty = (csv.Parser.Record == null) || csv.Parser.Record.All(string.IsNullOrWhiteSpace);
                    if (allEmpty) continue;
                    processados++;

                    // Ler campos com segurança (verificar se existe)
                    string GetFieldSafe(int idx) => (idx < csv.Parser.Record.Length) ? (csv.GetField(idx) ?? string.Empty) : string.Empty;

                    try
                    {
                        var placaRaw = GetFieldSafe(0);
                        var modelo = GetFieldSafe(1);
                        var anoRaw = GetFieldSafe(2);
                        var cliId = GetFieldSafe(3);
                        var cliNome = GetFieldSafe(4);
                        var cliTelRaw = GetFieldSafe(5);
                        var cliEnd = GetFieldSafe(6);
                        var mensalistaRaw = GetFieldSafe(7);
                        var valorMensRaw = GetFieldSafe(8);

                        var placa = _placa.Sanitizar(placaRaw);
                        if (string.IsNullOrWhiteSpace(placa))
                            throw new InvalidOperationException("Placa vazia após sanitização");

                        if (!_placa.EhValida(placa))
                            throw new InvalidOperationException("Placa inválida");

                        int? ano = int.TryParse(anoRaw, out var _ano) ? _ano : null;
                        var cliTel = new string((cliTelRaw ?? "").Where(char.IsDigit).ToArray());
                        bool mensalista = bool.TryParse(mensalistaRaw, out var m) && m;
                        decimal? valorMens = decimal.TryParse(valorMensRaw, out var vm) ? vm : null;

                        // Verifica duplicado
                        if (await _db.Veiculos.AnyAsync(v => v.Placa == placa))
                            throw new InvalidOperationException("Placa duplicada");

                        // Validar cliente — preferir buscar por identificador se fornecido
                        Cliente cliente = null!;
                        if (!string.IsNullOrWhiteSpace(cliId))
                        {
                            // tentar parse Id numérico (se aplicável ao seu modelo)
                            if (int.TryParse(cliId, out var parsedId))
                            {
                                cliente = await _db.Clientes.FindAsync(parsedId);
                            }
                        }

                        if (cliente == null)
                        {
                            // fallback por nome + telefone
                            cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Nome == cliNome && c.Telefone == cliTel);
                        }

                        if (cliente == null)
                        {
                            cliente = new Cliente
                            {
                                Nome = cliNome,
                                Telefone = cliTel,
                                Endereco = cliEnd,
                                Mensalista = mensalista,
                                ValorMensalidade = valorMens
                            };
                            _db.Clientes.Add(cliente);
                            await _db.SaveChangesAsync();
                        }

                        var v = new Veiculo { Placa = placa, Modelo = modelo, Ano = ano, ClienteId = cliente.Id };
                        _db.Veiculos.Add(v);
                        await _db.SaveChangesAsync();
                        inseridos++;
                    }
                    catch (Exception exRow)
                    {
                        // registrar erro estruturado: linha, campo (se detectável), motivo, raw
                        erros.Add(new
                        {
                            linha,
                            motivo = exRow.Message,
                            raw = rawRecord
                        });
                    }
                }
            }
            catch (HeaderValidationException hv)
            {
                return BadRequest(new { error = "Cabeçalho CSV inválido", details = hv.Message });
            }
            catch (Exception ex)
            {
                // erro global ao ler/parsing do CSV
                return StatusCode(500, new { error = "Erro ao processar CSV", details = ex.Message });
            }

            var result = new ImportResult
            {
                Processados = processados,
                Inseridos = inseridos,
                Erros = erros
            };

            return Ok(result);
        }
    }
}
