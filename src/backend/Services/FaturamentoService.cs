
using Microsoft.EntityFrameworkCore;
using Parking.Api.Data;
using Parking.Api.Models;

namespace Parking.Api.Services
{
    public class FaturamentoService
    {
        private readonly AppDbContext _db;
        public FaturamentoService(AppDbContext db) => _db = db;

        public async Task<List<Fatura>> GerarAsync(string competencia, CancellationToken ct = default)
        {
            // competencia formato yyyy-MM
            var part = competencia.Split('-');
            var ano = int.Parse(part[0]);
            var mes = int.Parse(part[1]);

            var primeiroDiaMes = new DateTime(ano, mes, 1);
            var ultimoDiaMes = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes), 23, 59, 59);
            var diasNoMes = (ultimoDiaMes - primeiroDiaMes).Days + 1;

            var mensalistas = await _db.Clientes
                .Where(c => c.Mensalista)
                .AsNoTracking()
                .ToListAsync(ct);

            var criadas = new List<Fatura>();

            foreach (var cli in mensalistas)
            {
                var existente = await _db.Faturas
                    .FirstOrDefaultAsync(f => f.ClienteId == cli.Id && f.Competencia == competencia, ct);
                if (existente != null) continue; // idempotência simples

                var historicos = await _db.HistoricoPosse
                    .Where(h => h.ClienteId == cli.Id &&
                                h.DataInicio <= ultimoDiaMes &&
                                (h.DataFim == null || h.DataFim >= primeiroDiaMes))
                    .ToListAsync(ct);

                if (!historicos.Any())
                    continue;

                decimal valorTotalCliente = 0m;
                var veiculosFaturados = new List<Guid>();

                foreach (var hist in historicos)
                {
                    var inicioEfetivo = hist.DataInicio < primeiroDiaMes ? primeiroDiaMes : hist.DataInicio;
                    var fimEfetivo = (hist.DataFim == null || hist.DataFim > ultimoDiaMes) ? ultimoDiaMes : hist.DataFim.Value;

                    var diasDePosse = (fimEfetivo - inicioEfetivo).Days + 1;
                    if (diasDePosse <= 0) continue;

                    var valorDiario = (cli.ValorMensalidade ?? 0m) / diasNoMes;
                    var valorProporcional = valorDiario * diasDePosse;

                    valorTotalCliente += valorProporcional;
                    veiculosFaturados.Add(hist.VeiculoId);
                }

                var fat = new Fatura
                {
                    Competencia = competencia,
                    ClienteId = cli.Id,
                    Valor = Math.Round(valorTotalCliente, 2),
                    Observacao = $"Fatura proporcional ({competencia}): {valorTotalCliente:C} - base {diasNoMes} dias"
                };

                foreach (var id in veiculosFaturados.Distinct())
                    fat.Veiculos.Add(new FaturaVeiculo { FaturaId = fat.Id, VeiculoId = id });

                _db.Faturas.Add(fat);
                criadas.Add(fat);
            }

            await _db.SaveChangesAsync(ct);
            return criadas;
        }

    }

}
