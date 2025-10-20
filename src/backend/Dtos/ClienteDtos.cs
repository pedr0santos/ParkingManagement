
namespace Parking.Api.Dtos
{
    public record ClienteCreateDto(string Nome, string? Telefone, string? Endereco, bool Mensalista, decimal? ValorMensalidade);
    public record ClienteUpdateDto(string Nome, string? Telefone, string? Endereco, bool Mensalista, decimal? ValorMensalidade);
}
