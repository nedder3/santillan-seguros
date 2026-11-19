namespace FederacionSantillan.Model.Validators;

public static class ArgentineDocumentValidator
{
    private static readonly int[] Multipliers = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    public static bool IsValidDni(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni)) return false;
        string clean = new string(dni.Where(char.IsDigit).ToArray());
        return clean.Length is >= 7 and <= 8 && long.TryParse(clean, out long num) && num >= 1_000_000;
    }

    public static bool IsValidCuit(string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit)) return false;
        string clean = new string(cuit.Where(char.IsDigit).ToArray());
        if (clean.Length != 11) return false;

        string prefix = clean[..2];
        string[] validPrefixes = { "20", "23", "24", "27", "30", "33", "34" };
        if (!validPrefixes.Contains(prefix)) return false;

        int sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (clean[i] - '0') * Multipliers[i];

        int mod = 11 - (sum % 11);
        int expectedCheck = mod switch
        {
            11 => 0,
            10 => 9,
            _ => mod
        };

        return (clean[10] - '0') == expectedCheck;
    }

    public static string FormatCuit(string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit)) return string.Empty;
        string clean = new string(cuit.Where(char.IsDigit).ToArray());
        if (clean.Length == 11)
            return $"{clean[..2]}-{clean.Substring(2, 8)}-{clean[10]}";
        return clean;
    }

    public static bool ValidateDocument(string docType, string documentNumber, out string error)
    {
        error = string.Empty;
        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            error = "El número de documento es obligatorio.";
            return false;
        }

        string clean = new string(documentNumber.Where(char.IsDigit).ToArray());

        if (string.Equals(docType, "DNI", StringComparison.OrdinalIgnoreCase))
        {
            if (clean.Length is < 7 or > 8)
            {
                error = "El DNI debe tener 7 u 8 dígitos numéricos (sin puntos ni letras).";
                return false;
            }
            if (!IsValidDni(clean))
            {
                error = "El número de DNI ingresado no es válido.";
                return false;
            }
            return true;
        }

        if (string.Equals(docType, "CUIT", StringComparison.OrdinalIgnoreCase))
        {
            if (clean.Length != 11)
            {
                error = "El CUIT debe contener exactamente 11 dígitos.";
                return false;
            }
            if (!IsValidCuit(clean))
            {
                error = "El CUIT ingresado no es válido (falla el dígito verificador oficial de Argentina).";
                return false;
            }
            return true;
        }

        // Si no se especifica tipo pero tiene 7-8 dígitos es DNI, o si tiene 11 es CUIT
        if (clean.Length is 7 or 8)
            return IsValidDni(clean);

        if (clean.Length == 11)
        {
            if (!IsValidCuit(clean))
            {
                error = "El CUIT ingresado no es válido (dígito verificador incorrecto).";
                return false;
            }
            return true;
        }

        error = "Debe ingresar un DNI (7-8 dígitos) o CUIT (11 dígitos con dígito verificador válido).";
        return false;
    }
}
