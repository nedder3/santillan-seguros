using FederacionSantillan.Core;
using FederacionSantillan.Core.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class UsuarioDAO
{
    public Usuario Autenticar(string username, string passwordHash) =>
        DbExceptionTranslator.Ejecutar(() =>
        {
            const string sql = @"
                SELECT id_usuario, username, password_hash, nombre_completo, rol, activo
                FROM usuarios
                WHERE username = @username AND password_hash = @passwordHash AND activo = TRUE
                LIMIT 1;";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new Usuario(
                reader.GetInt32("id_usuario"),
                reader.GetString("username"),
                reader.GetString("password_hash"),
                reader.GetString("nombre_completo"),
                reader.GetString("rol")
            )
            {
                Activo = reader.GetBoolean("activo")
            };
        });

    public bool ExisteUsername(string username) =>
        DbExceptionTranslator.Ejecutar(() =>
        {
            const string sql = "SELECT COUNT(1) FROM usuarios WHERE username = @username;";
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        });

    public bool Registrar(Usuario usuario) =>
        DbExceptionTranslator.Ejecutar(() =>
        {
            const string sql = @"
                INSERT INTO usuarios (username, password_hash, nombre_completo, rol, activo)
                VALUES (@username, @passwordHash, @nombreCompleto, @rol, TRUE);";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", usuario.obtenerUsername());
            cmd.Parameters.AddWithValue("@passwordHash", usuario.obtenerPasswordHash());
            cmd.Parameters.AddWithValue("@nombreCompleto", usuario.obtenerNombreCompleto());
            cmd.Parameters.AddWithValue("@rol", usuario.obtenerRol());
            return cmd.ExecuteNonQuery() > 0;
        }, MensajesSistema.Formatear(MensajesSistema.UsuarioDuplicado, usuario.obtenerUsername()));
}
