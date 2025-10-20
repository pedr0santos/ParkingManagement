
namespace Parking.Api.Dtos
{
    public record VeiculoCreateDto(string Placa, string? Modelo, int? Ano, Guid ClienteId);
    public record VeiculoUpdateDto(string Placa, string? Modelo, int? Ano, Guid ClienteId);
}
