
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking.Api.Models
{
    public class Veiculo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(8)] public string Placa { get; set; } = string.Empty;
        [MaxLength(120)] public string? Modelo { get; set; }
        public int? Ano { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.UtcNow;

        [Required] public Guid ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
