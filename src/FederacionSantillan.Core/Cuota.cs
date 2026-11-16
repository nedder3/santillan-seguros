using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

/// <summary>
/// Entidad de Dominio: Cuota / Plan de Cobranzas de una Póliza (Estándar ACORD Billing/Installment).
/// </summary>
public class Cuota : ICopiable<Cuota>, IClonable<Cuota>, IIdentificable<int>
{
    private int idCuota, idPoliza, numeroCuota;
    private decimal monto;
    private DateTime fechaVencimiento;
    private string estado, metodoPago, observaciones;
    private DateTime? fechaPago;

    // Datos informativos asociados para vista de alertas (desacoplado de SQL)
    private int numeroPolizaRef;
    private string aseguradoNombreRef, aseguradoTelefonoRef, ramoNombreRef;

    public int Id => idCuota;

    public Cuota(int id, int polizaId, int nroCuota, decimal m, DateTime vencimiento, string est)
    {
        idCuota = id;
        idPoliza = polizaId;
        numeroCuota = nroCuota;
        monto = m;
        fechaVencimiento = vencimiento;
        estado = est;
        fechaPago = null;
        metodoPago = null;
        observaciones = string.Empty;
    }

    public int IdCuota { get => idCuota; set => idCuota = value; }
    public int IdPoliza { get => idPoliza; set => idPoliza = value; }
    public int NumeroCuota { get => numeroCuota; set => numeroCuota = value; }
    public decimal Monto { get => monto; set => monto = value; }
    public DateTime FechaVencimiento { get => fechaVencimiento; set => fechaVencimiento = value; }
    public string Estado { get => estado; set => estado = value; }
    public DateTime? FechaPago { get => fechaPago; set => fechaPago = value; }
    public string MetodoPago { get => metodoPago; set => metodoPago = value; }
    public string Observaciones { get => observaciones; set => observaciones = value; }

    public int NumeroPolizaRef { get => numeroPolizaRef; set => numeroPolizaRef = value; }
    public string AseguradoNombreRef { get => aseguradoNombreRef; set => aseguradoNombreRef = value; }
    public string AseguradoTelefonoRef { get => aseguradoTelefonoRef; set => aseguradoTelefonoRef = value; }
    public string RamoNombreRef { get => ramoNombreRef; set => ramoNombreRef = value; }

    // Getters y Setters explícitos
    public int obtenerIdCuota() => idCuota;
    public void establecerIdCuota(int id) => idCuota = id;
    public int obtenerNumeroCuota() => numeroCuota;
    public void establecerNumeroCuota(int n) => numeroCuota = n;
    public decimal obtenerMonto() => monto;
    public void establecerMonto(decimal m) => monto = m;
    public string obtenerEstado() => estado;
    public void establecerEstado(string e) => estado = e;

    // Lógica de dominio (Early Return)
    public bool estaVencida() => estado != "PAGADA" && fechaVencimiento.Date < DateTime.Today;

    public bool estaPorVencer(int diasAnticipacion) =>
        estado == "PENDIENTE" &&
        fechaVencimiento.Date >= DateTime.Today &&
        fechaVencimiento.Date <= DateTime.Today.AddDays(diasAnticipacion);

    public int diasDeAtraso() =>
        estaVencida() ? (DateTime.Today - fechaVencimiento.Date).Days : 0;

    public int diasParaVencer() =>
        (fechaVencimiento.Date - DateTime.Today).Days;

    public bool registrarPago(string metodo)
    {
        if (estado == "PAGADA" || string.IsNullOrWhiteSpace(metodo)) return false;
        estado = "PAGADA";
        fechaPago = DateTime.Now;
        metodoPago = metodo;
        return true;
    }

    // Tríada Sagrada
    public void copy(Cuota c)
    {
        if (c == null) return;
        idCuota = c.idCuota;
        idPoliza = c.idPoliza;
        numeroCuota = c.numeroCuota;
        monto = c.monto;
        fechaVencimiento = c.fechaVencimiento;
        estado = c.estado;
        fechaPago = c.fechaPago;
        metodoPago = c.metodoPago;
        observaciones = c.observaciones;
    }

    public Cuota clone() =>
        new Cuota(idCuota, idPoliza, numeroCuota, monto, fechaVencimiento, estado)
        {
            fechaPago = fechaPago,
            metodoPago = metodoPago,
            observaciones = observaciones
        };

    public bool equals(Cuota c) =>
        c != null && idCuota == c.idCuota && idPoliza == c.idPoliza && numeroCuota == c.numeroCuota && monto == c.monto;

    public override string ToString() => $"Cuota #{numeroCuota} - ${monto:N2} ({estado} - Vto: {fechaVencimiento:dd/MM/yyyy})";
}
