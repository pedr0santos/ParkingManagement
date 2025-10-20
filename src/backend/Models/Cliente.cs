
using System.ComponentModel.DataAnnotations;

namespace Parking.Api.Models
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(200)] public string Nome { get; set; } = string.Empty;
        [MaxLength(20)] public string? Telefone { get; set; }
        [MaxLength(400)] public string? Endereco { get; set; }
        public bool Mensalista { get; set; }
        public decimal? ValorMensalidade { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.UtcNow;

        public List<Veiculo> Veiculos { get; set; } = new();
    }
}
