
namespace Parking.Api.Models
{
    public class Fatura
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Competencia { get; set; } = ""; // yyyy-MM
        public Guid ClienteId { get; set; }
        public decimal Valor { get; set; }
        public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
        public string? Observacao { get; set; }

        public List<FaturaVeiculo> Veiculos { get; set; } = new();
    }

    public class FaturaVeiculo
    {
        public Guid FaturaId { get; set; }
        public Guid VeiculoId { get; set; }
    }
}
