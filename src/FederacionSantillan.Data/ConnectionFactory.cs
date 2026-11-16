using System.Text.Json;
using MySqlConnector;

namespace FederacionSantillan.Data;

/// <summary>
/// Fábrica de conexiones a MariaDB/MySQL (Capa de Acceso a Datos - 3 Capas).
/// Lee la cadena de conexión desde appsettings.json o usa el contenedor Docker local por defecto.
/// </summary>
public static class ConnectionFactory
{
    private const string DefaultConnectionString =
        "Server=localhost;Port=3306;Database=santillan_seguros;User ID=santillan;Password=santillan123;Connection Timeout=5;";

    public static string GetConnectionString()
    {
        try
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cs) &&
                    cs.TryGetProperty("MariaDb", out var connStr))
                {
                    string val = connStr.GetString();
                    if (!string.IsNullOrWhiteSpace(val)) return val;
                }
            }
        }
        catch
        {
            // Fallback seguro a la configuración por defecto si el JSON está ausente o malformado
        }
        return DefaultConnectionString;
    }

    public static MySqlConnection CreateConnection() => new MySqlConnection(GetConnectionString());

    public static bool TestConnection(out string mensajeError)
    {
        try
        {
            using var conn = CreateConnection();
            conn.Open();
            mensajeError = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            mensajeError = ex.Message;
            return false;
        }
    }
}
