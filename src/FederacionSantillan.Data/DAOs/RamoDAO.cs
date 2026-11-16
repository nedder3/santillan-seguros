using FederacionSantillan.Core;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

/// <summary>
/// DAO de Ramos de Seguros: Lectura de catálogo paramétrico desde MariaDB.
/// </summary>
public class RamoDAO
{
    public List<Ramo> ObtenerTodos()
    {
        var lista = new List<Ramo>();
        const string sql = "SELECT id_ramo, codigo, nombre, COALESCE(descripcion, '') AS descripcion FROM ramos ORDER BY nombre;";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Ramo(
                reader.GetInt32("id_ramo"),
                reader.GetString("codigo"),
                reader.GetString("nombre"),
                reader.GetString("descripcion")
            ));
        }
        return lista;
    }
}
