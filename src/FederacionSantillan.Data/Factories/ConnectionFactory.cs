using System.Text.Json;
using MySqlConnector;

namespace FederacionSantillan.Data.Factories;

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
        catch { }
        return DefaultConnectionString;
    }

    public static MySqlConnection CreateConnection() => new(GetConnectionString());

    public static bool TestConnection(out string errorMessage)
    {
        try
        {
            using var conn = CreateConnection();
            conn.Open();
            errorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }
}
