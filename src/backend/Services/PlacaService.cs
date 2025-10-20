
using System.Text.RegularExpressions;

namespace Parking.Api.Services
{
    public class PlacaService
    {
        // Intencionalmente simples (candidato deve robustecer)
        public string Sanitizar(string? placa)
        {
            var p = Regex.Replace(placa ?? "", "[^A-Za-z0-9]", "").ToUpperInvariant();
            return p;
        }

        // TODO: melhorar regras para Mercosul - aceitar AAA1A23 e similares
        public bool EhValida(string placa)
        {
            return Regex.IsMatch(placa, "^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");
        }
    }
}
