using FederacionSantillan.Core;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

/// <summary>
/// DTO de lectura para grilla de Pólizas con datos joineados de Asegurado y Ramo.
/// </summary>
public class PolizaResumenDTO
{
    public int IdPoliza { get; set; }
    public int NumeroPoliza { get; set; }
    public int IdAsegurado { get; set; }
    public string AseguradoNombre { get; set; }
    public string RamoNombre { get; set; }
    public string Compania { get; set; }
    public string BienAsegurado { get; set; }
    public decimal PrimaTotal { get; set; }
    public int CantidadCuotas { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; }
}

/// <summary>
/// DAO de Pólizas: Encapsula consultas SQL y emisión transaccional con generación de cuotas.
/// </summary>
public class PolizaDAO
{
    public List<PolizaResumenDTO> ObtenerTodas(string filtro = "")
    {
        var lista = new List<PolizaResumenDTO>();
        const string sql = @"
            SELECT p.id_poliza, p.numero_poliza, p.id_asegurado,
                   a.apellido_nombre AS asegurado_nombre,
                   r.nombre AS ramo_nombre,
                   p.compania, p.bien_asegurado, p.prima_total,
                   p.cantidad_cuotas, p.fecha_inicio, p.fecha_fin, p.estado
            FROM polizas p
            INNER JOIN asegurados a ON p.id_asegurado = a.id_asegurado
            INNER JOIN ramos r ON p.id_ramo = r.id_ramo
            WHERE (@filtro = '' OR CAST(p.numero_poliza AS CHAR) LIKE @likeFiltro
                   OR a.apellido_nombre LIKE @likeFiltro OR p.bien_asegurado LIKE @likeFiltro)
            ORDER BY p.numero_poliza DESC;";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        string f = filtro?.Trim() ?? string.Empty;
        cmd.Parameters.AddWithValue("@filtro", f);
        cmd.Parameters.AddWithValue("@likeFiltro", $"%{f}%");

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new PolizaResumenDTO
            {
                IdPoliza = reader.GetInt32("id_poliza"),
                NumeroPoliza = reader.GetInt32("numero_poliza"),
                IdAsegurado = reader.GetInt32("id_asegurado"),
                AseguradoNombre = reader.GetString("asegurado_nombre"),
                RamoNombre = reader.GetString("ramo_nombre"),
                Compania = reader.GetString("compania"),
                BienAsegurado = reader.GetString("bien_asegurado"),
                PrimaTotal = reader.GetDecimal("prima_total"),
                CantidadCuotas = reader.GetInt32("cantidad_cuotas"),
                FechaInicio = reader.GetDateTime("fecha_inicio"),
                FechaFin = reader.GetDateTime("fecha_fin"),
                Estado = reader.GetString("estado")
            });
        }
        return lista;
    }

    public bool ExisteNumeroPoliza(int numeroPoliza)
    {
        const string sql = "SELECT COUNT(1) FROM polizas WHERE numero_poliza = @num;";
        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@num", numeroPoliza);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    /// <summary>
    /// Emite una póliza y genera automáticamente su plan de cuotas mensuales en una transacción SQL atómica.
    /// </summary>
    public int EmitirPolizaConCuotas(
        int numeroPoliza, int idAsegurado, int idRamo, string compania,
        string bienAsegurado, decimal primaTotal, int cantidadCuotas,
        DateTime fechaInicio, DateTime fechaFin, DateTime primerVencimiento)
    {
        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            const string sqlPoliza = @"
                INSERT INTO polizas (numero_poliza, id_asegurado, id_ramo, compania, bien_asegurado,
                                     prima_total, cantidad_cuotas, fecha_inicio, fecha_fin, estado)
                VALUES (@num, @idAseg, @idRamo, @comp, @bien, @prima, @cant, @ini, @fin, 'ACTIVA');
                SELECT LAST_INSERT_ID();";

            int idPoliza;
            using (var cmd = new MySqlCommand(sqlPoliza, conn, tx))
            {
                cmd.Parameters.AddWithValue("@num", numeroPoliza);
                cmd.Parameters.AddWithValue("@idAseg", idAsegurado);
                cmd.Parameters.AddWithValue("@idRamo", idRamo);
                cmd.Parameters.AddWithValue("@comp", compania);
                cmd.Parameters.AddWithValue("@bien", bienAsegurado);
                cmd.Parameters.AddWithValue("@prima", primaTotal);
                cmd.Parameters.AddWithValue("@cant", cantidadCuotas);
                cmd.Parameters.AddWithValue("@ini", fechaInicio.Date);
                cmd.Parameters.AddWithValue("@fin", fechaFin.Date);
                idPoliza = Convert.ToInt32(cmd.ExecuteScalar());
            }

            decimal montoCuota = Math.Round(primaTotal / cantidadCuotas, 2);
            const string sqlCuota = @"
                INSERT INTO cuotas (id_poliza, numero_cuota, monto, fecha_vencimiento, estado)
                VALUES (@idPoliza, @nro, @monto, @vto, @estado);";

            for (int i = 1; i <= cantidadCuotas; i++)
            {
                DateTime vto = primerVencimiento.AddMonths(i - 1).Date;
                string estadoCuota = vto < DateTime.Today ? "VENCIDA" : "PENDIENTE";

                using var cmdCuota = new MySqlCommand(sqlCuota, conn, tx);
                cmdCuota.Parameters.AddWithValue("@idPoliza", idPoliza);
                cmdCuota.Parameters.AddWithValue("@nro", i);
                cmdCuota.Parameters.AddWithValue("@monto", montoCuota);
                cmdCuota.Parameters.AddWithValue("@vto", vto);
                cmdCuota.Parameters.AddWithValue("@estado", estadoCuota);
                cmdCuota.ExecuteNonQuery();
            }

            tx.Commit();
            return idPoliza;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}
