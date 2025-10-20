
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Api.Data;
using Parking.Api.Dtos;
using Parking.Api.Models;
using Parking.Api.Services;

namespace Parking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaturasController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly FaturamentoService _fat;
        public FaturasController(AppDbContext db, FaturamentoService fat) { _db = db; _fat = fat; }

        [HttpPost("gerar")]
        public async Task<IActionResult> Gerar([FromBody] GerarFaturaRequest req, CancellationToken ct)
        {
            var criadas = await _fat.GerarAsync(req.Competencia, ct);
            return Ok(new { criadas = criadas.Count });
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? competencia = null)
        {
            var q = _db.Faturas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(competencia)) q = q.Where(f => f.Competencia == competencia);
            var list = await q
                .OrderByDescending(f => f.CriadaEm)
                .Select(f => new {
                    f.Id, f.Competencia, f.ClienteId, f.Valor, f.CriadaEm,
                    qtdVeiculos = _db.FaturasVeiculos.Count(x => x.FaturaId == f.Id)
                })
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:guid}/placas")]
        public async Task<IActionResult> Placas(Guid id)
        {
            var placas = await _db.FaturasVeiculos
                .Where(x => x.FaturaId == id)
                .Join(_db.Veiculos, fv => fv.VeiculoId, v => v.Id, (fv, v) => v.Placa)
                .ToListAsync();
            return Ok(placas);
        }
    }
}
