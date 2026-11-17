namespace FederacionSantillan.Model.Exceptions;

public static class SystemMessages
{
    public const string CredentialsRequired = "Debe ingresar usuario y contraseña.";
    public const string InvalidCredentials = "Usuario o contraseña incorrectos, o cuenta inactiva.";
    public const string DuplicateUsername = "El usuario '{0}' ya existe en el sistema.";
    public const string DuplicateTaxId = "Ya existe un asegurado registrado con el DNI/CUIT {0}.";
    public const string DuplicatePolicyNumber = "La póliza #{0} ya se encuentra registrada.";
    public const string InvalidPolicyDates = "La fecha de fin de vigencia debe ser posterior a la fecha de inicio.";
    public const string SelectionRequired = "Debe seleccionar un registro válido antes de continuar.";
    public const string InstallmentAlreadyPaid = "No se pudo registrar el pago (verifique si la cuota ya estaba abonada).";
    public const string DatabaseConnectionError = "No se pudo conectar con MariaDB. Verifique que el servicio esté activo.";
    public const string GenericPersistenceError = "Ocurrió un error al acceder a la base de datos: {0}";

    public static string Format(string template, params object[] args) =>
        string.Format(template, args);
}
