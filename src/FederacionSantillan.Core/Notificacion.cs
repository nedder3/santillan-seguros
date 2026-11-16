using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

/// <summary>
/// Entidad de Dominio: Notificación de Vencimiento o Mora (Relación de Asociación con Cuota).
/// </summary>
public class Notificacion : ICopiable<Notificacion>, IClonable<Notificacion>, IIdentificable<int>
{
    private int idNotificacion;
    private Cuota cuota;
    private string tipoAlerta, mensaje;
    private DateTime fechaGeneracion;
    private bool leida;

    public int Id => idNotificacion;

    public Notificacion(int id, Cuota c, string tipo, string msg)
    {
        idNotificacion = id;
        cuota = c;
        tipoAlerta = tipo;
        mensaje = msg;
        fechaGeneracion = DateTime.Now;
        leida = false;
    }

    public int IdNotificacion { get => idNotificacion; set => idNotificacion = value; }
    public Cuota Cuota { get => cuota; set => cuota = value; }
    public string TipoAlerta { get => tipoAlerta; set => tipoAlerta = value; }
    public string Mensaje { get => mensaje; set => mensaje = value; }
    public DateTime FechaGeneracion { get => fechaGeneracion; set => fechaGeneracion = value; }
    public bool Leida { get => leida; set => leida = value; }

    public void marcarComoLeida() => leida = true;

    // Tríada Sagrada EN PROFUNDIDAD
    public void copy(Notificacion n)
    {
        if (n == null) return;
        idNotificacion = n.idNotificacion;
        tipoAlerta = n.tipoAlerta;
        mensaje = n.mensaje;
        fechaGeneracion = n.fechaGeneracion;
        leida = n.leida;
        if (cuota != null && n.cuota != null) cuota.copy(n.cuota);
    }

    public Notificacion clone() =>
        new Notificacion(idNotificacion, cuota != null ? cuota.clone() : null, tipoAlerta, mensaje)
        {
            fechaGeneracion = fechaGeneracion,
            leida = leida
        };

    public bool equals(Notificacion n) =>
        n != null &&
        idNotificacion == n.idNotificacion &&
        tipoAlerta == n.tipoAlerta &&
        (cuota == null ? n.cuota == null : cuota.equals(n.cuota));

    public override string ToString() => $"[{tipoAlerta}] {mensaje}";
}
