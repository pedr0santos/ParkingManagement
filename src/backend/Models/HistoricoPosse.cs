namespace Parking.Api.Models
{
    public class HistoricoPosse
    {
        public Guid Id { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; } // null = ainda é o dono atual

        public Veiculo Veiculo { get; set; } = null!;
        public Cliente Cliente { get; set; } = null!;
    }
}
