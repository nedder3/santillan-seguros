using FederacionSantillan.Model;
using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class UserDao
{
    public User Authenticate(string username, string passwordHash) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                SELECT user_id, username, password_hash, full_name, role, active
                FROM users
                WHERE username = @username AND password_hash = @passwordHash AND active = TRUE
                LIMIT 1;";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User(
                reader.GetInt32("user_id"),
                reader.GetString("username"),
                reader.GetString("password_hash"),
                reader.GetString("full_name"),
                reader.GetString("role")
            )
            {
                Active = reader.GetBoolean("active")
            };
        });

    public bool ExistsUsername(string username) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = "SELECT COUNT(1) FROM users WHERE username = @username;";
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        });

    public bool Register(User user) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                INSERT INTO users (username, password_hash, full_name, role, active)
                VALUES (@username, @passwordHash, @fullName, @role, TRUE);";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", user.getUsername());
            cmd.Parameters.AddWithValue("@passwordHash", user.getPasswordHash());
            cmd.Parameters.AddWithValue("@fullName", user.getFullName());
            cmd.Parameters.AddWithValue("@role", user.getRole());
            return cmd.ExecuteNonQuery() > 0;
        }, SystemMessages.Format(SystemMessages.DuplicateUsername, user.getUsername()));
}
