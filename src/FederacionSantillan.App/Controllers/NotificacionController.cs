using FederacionSantillan.Core;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

/// <summary>
/// Controlador MVC de Notificaciones y Cobranzas: Detecta morosos y cuotas próximas a vencer.
/// </summary>
public class NotificacionController
{
    private readonly CuotaDAO _cuotaDao = new CuotaDAO();

    public List<Cuota> ObtenerCuotasVencidas() => _cuotaDao.ObtenerVencidas();

    public List<Cuota> ObtenerCuotasPorVencer(int dias = 15) => _cuotaDao.ObtenerPorVencer(dias);

    public (decimal TotalVencido, decimal TotalPorVencer, int CantVencidas, int CantPorVencer) ObtenerResumenAlertas(int dias = 15)
    {
        var vencidas = ObtenerCuotasVencidas();
        var porVencer = ObtenerCuotasPorVencer(dias);
        return (
            vencidas.Sum(c => c.Monto),
            porVencer.Sum(c => c.Monto),
            vencidas.Count,
            porVencer.Count
        );
    }

    public (bool Exito, string Mensaje) RegistrarPagoCuota(int idCuota, string metodoPago, string observaciones = "")
    {
        if (idCuota <= 0) return (false, "Seleccione una cuota válida.");
        if (string.IsNullOrWhiteSpace(metodoPago)) return (false, "Seleccione un método de pago.");

        bool ok = _cuotaDao.RegistrarPago(idCuota, metodoPago, observaciones);
        return ok
            ? (true, "Pago registrado correctamente. La alerta fue saldada.")
            : (false, "No se pudo registrar el pago (verifique si la cuota ya estaba pagada).");
    }
}
