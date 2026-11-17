using FederacionSantillan.Model;
using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class InsuredDao
{
    public List<Insured> GetAll(string filter = "") =>
        DbExceptionTranslator.Execute(() =>
        {
            var list = new List<Insured>();
            const string sql = @"
                SELECT insured_id, tax_id, full_name, phone,
                       COALESCE(email, '') AS email, COALESCE(address, '') AS address, active
                FROM insureds
                WHERE active = TRUE
                  AND (@filter = '' OR full_name LIKE @likeFilter OR tax_id LIKE @likeFilter)
                ORDER BY full_name;";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            string f = filter?.Trim() ?? string.Empty;
            cmd.Parameters.AddWithValue("@filter", f);
            cmd.Parameters.AddWithValue("@likeFilter", $"%{f}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Insured(
                    reader.GetInt32("insured_id"),
                    reader.GetString("tax_id"),
                    reader.GetString("full_name"),
                    reader.GetString("phone"),
                    reader.GetString("email"),
                    reader.GetString("address")
                )
                {
                    Active = reader.GetBoolean("active")
                });
            }
            return list;
        });

    public bool ExistsTaxId(string taxId, int excludeId = 0) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = "SELECT COUNT(1) FROM insureds WHERE tax_id = @taxId AND insured_id <> @excludeId;";
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@taxId", taxId);
            cmd.Parameters.AddWithValue("@excludeId", excludeId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        });

    public int Insert(Insured insured) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                INSERT INTO insureds (tax_id, full_name, phone, email, address, active)
                VALUES (@taxId, @fullName, @phone, @email, @address, TRUE);
                SELECT LAST_INSERT_ID();";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@taxId", insured.getTaxId());
            cmd.Parameters.AddWithValue("@fullName", insured.getFullName());
            cmd.Parameters.AddWithValue("@phone", insured.getPhone());
            cmd.Parameters.AddWithValue("@email", insured.getEmail());
            cmd.Parameters.AddWithValue("@address", insured.getAddress());
            return Convert.ToInt32(cmd.ExecuteScalar());
        }, SystemMessages.Format(SystemMessages.DuplicateTaxId, insured.getTaxId()));

    public bool Update(Insured insured) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                UPDATE insureds
                SET tax_id = @taxId, full_name = @fullName, phone = @phone,
                    email = @email, address = @address
                WHERE insured_id = @id;";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", insured.getInsuredId());
            cmd.Parameters.AddWithValue("@taxId", insured.getTaxId());
            cmd.Parameters.AddWithValue("@fullName", insured.getFullName());
            cmd.Parameters.AddWithValue("@phone", insured.getPhone());
            cmd.Parameters.AddWithValue("@email", insured.getEmail());
            cmd.Parameters.AddWithValue("@address", insured.getAddress());
            return cmd.ExecuteNonQuery() > 0;
        }, SystemMessages.Format(SystemMessages.DuplicateTaxId, insured.getTaxId()));

    public bool SoftDelete(int insuredId) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = "UPDATE insureds SET active = FALSE WHERE insured_id = @id;";
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", insuredId);
            return cmd.ExecuteNonQuery() > 0;
        });
}
