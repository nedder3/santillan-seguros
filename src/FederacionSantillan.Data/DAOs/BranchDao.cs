using FederacionSantillan.Model;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class BranchDao
{
    public List<Branch> GetAll() =>
        DbExceptionTranslator.Execute(() =>
        {
            var list = new List<Branch>();
            const string sql = "SELECT branch_id, code, name, COALESCE(description, '') AS description FROM branches ORDER BY name;";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Branch(
                    reader.GetInt32("branch_id"),
                    reader.GetString("code"),
                    reader.GetString("name"),
                    reader.GetString("description")
                ));
            }
            return list;
        });
}
