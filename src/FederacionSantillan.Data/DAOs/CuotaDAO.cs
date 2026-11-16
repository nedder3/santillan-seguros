using FederacionSantillan.Core;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

/// <summary>
/// DAO de Cuotas y Alertas: Consultas SQL de cuotas vencidas, por vencer y registro de pagos.
/// </summary>
public class CuotaDAO
{
    /// <summary>
    /// Sincroniza el estado a VENCIDA para cuotas pendientes cuya fecha ya pasó.
    /// </summary>
    public void ActualizarCuotasVencidas()
    {
        const string sql = @"
            UPDATE cuotas
            SET estado = 'VENCIDA'
            WHERE estado = 'PENDIENTE' AND fecha_vencimiento < CURDATE();";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public List<Cuota> ObtenerPorPoliza(int idPoliza)
    {
        const string sql = @"
            SELECT c.id_cuota, c.id_poliza, c.numero_cuota, c.monto, c.fecha_vencimiento,
                   c.estado, c.fecha_pago, COALESCE(c.metodo_pago, '') AS metodo_pago,
                   COALESCE(c.observaciones, '') AS observaciones,
                   p.numero_poliza, a.apellido_nombre, a.telefono, r.nombre AS ramo_nombre
            FROM cuotas c
            INNER JOIN polizas p ON c.id_poliza = p.id_poliza
            INNER JOIN asegurados a ON p.id_asegurado = a.id_asegurado
            INNER JOIN ramos r ON p.id_ramo = r.id_ramo
            WHERE c.id_poliza = @idPoliza
            ORDER BY c.numero_cuota;";

        return EjecutarConsultaCuotas(sql, cmd => cmd.Parameters.AddWithValue("@idPoliza", idPoliza));
    }

    public List<Cuota> ObtenerVencidas()
    {
        ActualizarCuotasVencidas();
        const string sql = @"
            SELECT c.id_cuota, c.id_poliza, c.numero_cuota, c.monto, c.fecha_vencimiento,
                   c.estado, c.fecha_pago, COALESCE(c.metodo_pago, '') AS metodo_pago,
                   COALESCE(c.observaciones, '') AS observaciones,
                   p.numero_poliza, a.apellido_nombre, a.telefono, r.nombre AS ramo_nombre
            FROM cuotas c
            INNER JOIN polizas p ON c.id_poliza = p.id_poliza
            INNER JOIN asegurados a ON p.id_asegurado = a.id_asegurado
            INNER JOIN ramos r ON p.id_ramo = r.id_ramo
            WHERE c.estado = 'VENCIDA'
              AND p.estado = 'ACTIVA'
              AND a.activo = TRUE
            ORDER BY c.fecha_vencimiento ASC;";

        return EjecutarConsultaCuotas(sql);
    }

    public List<Cuota> ObtenerPorVencer(int diasAnticipacion = 15)
    {
        const string sql = @"
            SELECT c.id_cuota, c.id_poliza, c.numero_cuota, c.monto, c.fecha_vencimiento,
                   c.estado, c.fecha_pago, COALESCE(c.metodo_pago, '') AS metodo_pago,
                   COALESCE(c.observaciones, '') AS observaciones,
                   p.numero_poliza, a.apellido_nombre, a.telefono, r.nombre AS ramo_nombre
            FROM cuotas c
            INNER JOIN polizas p ON c.id_poliza = p.id_poliza
            INNER JOIN asegurados a ON p.id_asegurado = a.id_asegurado
            INNER JOIN ramos r ON p.id_ramo = r.id_ramo
            WHERE c.estado = 'PENDIENTE'
              AND c.fecha_vencimiento BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @dias DAY)
              AND p.estado = 'ACTIVA'
              AND a.activo = TRUE
            ORDER BY c.fecha_vencimiento ASC;";

        return EjecutarConsultaCuotas(sql, cmd => cmd.Parameters.AddWithValue("@dias", diasAnticipacion));
    }

    public bool RegistrarPago(int idCuota, string metodoPago, string observaciones = "")
    {
        const string sql = @"
            UPDATE cuotas
            SET estado = 'PAGADA',
                fecha_pago = NOW(),
                metodo_pago = @metodo,
                observaciones = @obs
            WHERE id_cuota = @idCuota AND estado <> 'PAGADA';";

        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@idCuota", idCuota);
        cmd.Parameters.AddWithValue("@metodo", metodoPago);
        cmd.Parameters.AddWithValue("@obs", observaciones ?? string.Empty);
        return cmd.ExecuteNonQuery() > 0;
    }

    private List<Cuota> EjecutarConsultaCuotas(string sql, Action<MySqlCommand> configurarParametros = null)
    {
        var lista = new List<Cuota>();
        using var conn = ConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        configurarParametros?.Invoke(cmd);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var c = new Cuota(
                reader.GetInt32("id_cuota"),
                reader.GetInt32("id_poliza"),
                reader.GetInt32("numero_cuota"),
                reader.GetDecimal("monto"),
                reader.GetDateTime("fecha_vencimiento"),
                reader.GetString("estado")
            )
            {
                MetodoPago = reader.GetString("metodo_pago"),
                Observaciones = reader.GetString("observaciones"),
                NumeroPolizaRef = reader.GetInt32("numero_poliza"),
                AseguradoNombreRef = reader.GetString("apellido_nombre"),
                AseguradoTelefonoRef = reader.GetString("telefono"),
                RamoNombreRef = reader.GetString("ramo_nombre")
            };

            if (!reader.IsDBNull(reader.GetOrdinal("fecha_pago")))
                c.FechaPago = reader.GetDateTime("fecha_pago");

            lista.Add(c);
        }
        return lista;
    }
}
