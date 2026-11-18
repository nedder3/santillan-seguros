using System.Globalization;
using System.Text;
using FederacionSantillan.Data.Factories;
using FederacionSantillan.Data.Translators;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class DatabaseBackupDao
{
    private static readonly string[] OrderedTables =
    {
        "users", "branches", "insureds", "policies", "installments", "notifications"
    };

    public string GenerateSqlDump() =>
        DbExceptionTranslator.Execute(() =>
        {
            var sb = new StringBuilder(16384);
            sb.AppendLine("-- ============================================================================");
            sb.AppendLine($"-- SQL DUMP - Federacion Santillan Seguros ({DateTime.Now:yyyy-MM-dd HH:mm:ss})");
            sb.AppendLine("-- ============================================================================");
            sb.AppendLine("CREATE DATABASE IF NOT EXISTS `santillan_seguros` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;");
            sb.AppendLine("USE `santillan_seguros`;");
            sb.AppendLine("SET FOREIGN_KEY_CHECKS = 0;");
            sb.AppendLine("SET SQL_MODE = 'NO_AUTO_VALUE_ON_ZERO';");
            sb.AppendLine("START TRANSACTION;");
            sb.AppendLine();

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            foreach (string table in OrderedTables)
            {
                AppendTableDdl(conn, sb, table);
                AppendTableInserts(conn, sb, table);
            }

            sb.AppendLine("SET FOREIGN_KEY_CHECKS = 1;");
            sb.AppendLine("COMMIT;");
            return sb.ToString();
        });

    private static void AppendTableDdl(MySqlConnection conn, StringBuilder sb, string table)
    {
        using var cmd = new MySqlCommand($"SHOW CREATE TABLE `{table}`;", conn);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return;

        sb.AppendLine($"DROP TABLE IF EXISTS `{table}`;");
        sb.AppendLine($"{reader.GetString(1)};");
        sb.AppendLine();
    }

    private static void AppendTableInserts(MySqlConnection conn, StringBuilder sb, string table)
    {
        using var cmd = new MySqlCommand($"SELECT * FROM `{table}`;", conn);
        using var reader = cmd.ExecuteReader();
        if (!reader.HasRows) return;

        int fieldCount = reader.FieldCount;
        var colNames = new string[fieldCount];
        for (int i = 0; i < fieldCount; i++)
            colNames[i] = $"`{reader.GetName(i)}`";

        string columnsHeader = string.Join(", ", colNames);

        while (reader.Read())
        {
            var values = new string[fieldCount];
            for (int i = 0; i < fieldCount; i++)
                values[i] = FormatSqlValue(reader.GetValue(i));

            sb.Append("INSERT INTO `").Append(table).Append("` (").Append(columnsHeader)
              .Append(") VALUES (").Append(string.Join(", ", values)).AppendLine(");");
        }
        sb.AppendLine();
    }

    private static string FormatSqlValue(object value)
    {
        if (value == null || value == DBNull.Value) return "NULL";
        return value switch
        {
            bool b => b ? "1" : "0",
            int or long or short or byte => Convert.ToString(value, CultureInfo.InvariantCulture),
            decimal or double or float => Convert.ToString(value, CultureInfo.InvariantCulture),
            DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
            _ => $"'{value.ToString().Replace("\\", "\\\\").Replace("'", "\\'")}'"
        };
    }
}
