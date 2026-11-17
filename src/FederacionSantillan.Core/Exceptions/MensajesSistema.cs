namespace FederacionSantillan.Core.Exceptions;

public static class MensajesSistema
{
    public const string CredencialesRequeridas = "Debe ingresar usuario y contraseña.";
    public const string CredencialesInvalidas = "Usuario o contraseña incorrectos, o cuenta inactiva.";
    public const string UsuarioDuplicado = "El usuario '{0}' ya existe en el sistema.";
    public const string DniDuplicado = "Ya existe un asegurado registrado con el DNI/CUIT {0}.";
    public const string PolizaDuplicada = "La póliza #{0} ya se encuentra registrada.";
    public const string VigenciaInvalida = "La fecha de fin de vigencia debe ser posterior a la fecha de inicio.";
    public const string SeleccionRequerida = "Debe seleccionar un registro válido antes de continuar.";
    public const string CuotaYaPagada = "No se pudo registrar el pago (verifique si la cuota ya estaba abonada).";
    public const string ErrorConexionDb = "No se pudo conectar con MariaDB. Verifique que el servicio esté activo.";
    public const string ErrorPersistenciaGenerico = "Ocurrió un error al acceder a la base de datos: {0}";

    public static string Formatear(string plantilla, params object[] args) =>
        string.Format(plantilla, args);
}
