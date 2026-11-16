using FederacionSantillan.Core;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

/// <summary>
/// DAO de Asegurados: Operaciones CRUD completas sobre la tabla 'asegurados' en MariaDB.
/// </summary>
public class AseguradoDAO
{
    public List<Asegurado> ObtenerTodos(string filtro = "")
    {
        var lista = new List<Asegurado>();
        const string sql = @"
            SELECT id_asegurado, dni_cuit, apellido_nombre, telefono,
                   COALESCE(email, '') AS email, COALESCE(domicilio, '') AS domicilio, activo
            FROM asegurados
            WHERE activo = TRUE
              AND (@filtro = '' OR apellido_nombre LIKE @likeFiltro OR dni_cuit LIKE @likeFiltro)
            ORDER BY apellido_nombre;";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        string f = filtro?.Trim() ?? string.Empty;
        cmd.Parameters.AddWithValue("@filtro", f);
        cmd.Parameters.AddWithValue("@likeFiltro", $"%{f}%");

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Asegurado(
                reader.GetInt32("id_asegurado"),
                reader.GetString("dni_cuit"),
                reader.GetString("apellido_nombre"),
                reader.GetString("telefono"),
                reader.GetString("email"),
                reader.GetString("domicilio")
            )
            {
                Activo = reader.GetBoolean("activo")
            });
        }
        return lista;
    }

    public bool ExisteDni(string dniCuit, int excluirId = 0)
    {
        const string sql = "SELECT COUNT(1) FROM asegurados WHERE dni_cuit = @dni AND id_asegurado <> @excluirId;";
        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@dni", dniCuit);
        cmd.Parameters.AddWithValue("@excluirId", excluirId);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public int Insertar(Asegurado a)
    {
        const string sql = @"
            INSERT INTO asegurados (dni_cuit, apellido_nombre, telefono, email, domicilio, activo)
            VALUES (@dni, @nombre, @tel, @email, @domicilio, TRUE);
            SELECT LAST_INSERT_ID();";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@dni", a.getDni());
        cmd.Parameters.AddWithValue("@nombre", a.getNombre());
        cmd.Parameters.AddWithValue("@tel", a.getTelefono());
        cmd.Parameters.AddWithValue("@email", a.getEmail());
        cmd.Parameters.AddWithValue("@domicilio", a.getDomicilio());
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public bool Actualizar(Asegurado a)
    {
        const string sql = @"
            UPDATE asegurados
            SET dni_cuit = @dni, apellido_nombre = @nombre, telefono = @tel,
                email = @email, domicilio = @domicilio
            WHERE id_asegurado = @id;";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", a.getCodigo());
        cmd.Parameters.AddWithValue("@dni", a.getDni());
        cmd.Parameters.AddWithValue("@nombre", a.getNombre());
        cmd.Parameters.AddWithValue("@tel", a.getTelefono());
        cmd.Parameters.AddWithValue("@email", a.getEmail());
        cmd.Parameters.AddWithValue("@domicilio", a.getDomicilio());
        return cmd.ExecuteNonQuery() > 0;
    }

    public bool DarDeBaja(int idAsegurado)
    {
        const string sql = "UPDATE asegurados SET activo = FALSE WHERE id_asegurado = @id;";
        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", idAsegurado);
        return cmd.ExecuteNonQuery() > 0;
    }
}
