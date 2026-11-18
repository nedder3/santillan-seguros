using System.Security.Cryptography;
using System.Text;
using FederacionSantillan.Data.Factories;
using FederacionSantillan.Data.Translators;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.Entities;
using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class UserDao
{
    public User Authenticate(SignInRequestDto dto) =>
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
            cmd.Parameters.AddWithValue("@username", dto.Username.Trim());
            cmd.Parameters.AddWithValue("@passwordHash", ComputeSha256(dto.Password));

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                throw new ValidationException(SystemMessages.InvalidCredentials);

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

    public void Register(RegisterUserRequestDto dto) =>
        DbExceptionTranslator.Execute(() =>
        {
            string cleanUser = dto.Username.Trim();
            if (ExistsUsername(cleanUser))
                throw new DuplicateResourceException(SystemMessages.Format(SystemMessages.DuplicateUsername, cleanUser));

            const string sql = @"
                INSERT INTO users (username, password_hash, full_name, role, active)
                VALUES (@username, @passwordHash, @fullName, @role, TRUE);";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", cleanUser);
            cmd.Parameters.AddWithValue("@passwordHash", ComputeSha256(dto.Password));
            cmd.Parameters.AddWithValue("@fullName", dto.FullName.Trim());
            cmd.Parameters.AddWithValue("@role", dto.Role);
            cmd.ExecuteNonQuery();
        }, SystemMessages.Format(SystemMessages.DuplicateUsername, dto.Username?.Trim()));

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

    public static string ComputeSha256(string text) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text ?? string.Empty))).ToLowerInvariant();
}
