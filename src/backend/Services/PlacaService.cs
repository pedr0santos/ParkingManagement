
using System.Text.RegularExpressions;

namespace Parking.Api.Services
{
    public class PlacaService
    {
        public string Sanitizar(string? placa)
        {
            if (string.IsNullOrWhiteSpace(placa)) return string.Empty;
            return Regex.Replace(placa.Trim(), "[^A-Za-z0-9]", "").ToUpperInvariant();
        }

        public bool EhValida(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa)) return false;

            var regexAntigo = @"^[A-Z]{3}[0-9]{4}$";
            var regexMercosul = @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$";

            return Regex.IsMatch(placa, regexAntigo) || Regex.IsMatch(placa, regexMercosul);
        }
    }
}
