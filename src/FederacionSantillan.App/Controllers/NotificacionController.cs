using FederacionSantillan.Core;
using FederacionSantillan.Core.Exceptions;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

public class NotificacionController
{
    private readonly CuotaDAO _cuotaDao = new CuotaDAO();

    public List<Cuota> ObtenerCuotasVencidas() => _cuotaDao.ObtenerVencidas();

    public List<Cuota> ObtenerCuotasPorVencer(int dias = 15) => _cuotaDao.ObtenerPorVencer(dias);

    public string RegistrarPagoCuota(int idCuota, string metodoPago, string observaciones = "")
    {
        if (idCuota <= 0 || string.IsNullOrWhiteSpace(metodoPago))
            throw new NegocioException(MensajesSistema.SeleccionRequerida);

        bool pagada = _cuotaDao.RegistrarPago(idCuota, metodoPago, observaciones);
        if (!pagada)
            throw new NegocioException(MensajesSistema.CuotaYaPagada);

        return "Pago registrado correctamente. La alerta fue saldada.";
    }
}
