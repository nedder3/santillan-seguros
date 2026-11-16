using FederacionSantillan.Core;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

/// <summary>
/// Controlador MVC de Cartera: Orquesta Asegurados, Ramos, Pólizas y Cuotas.
/// </summary>
public class CarteraController
{
    private readonly AseguradoDAO _aseguradoDao = new AseguradoDAO();
    private readonly RamoDAO _ramoDao = new RamoDAO();
    private readonly PolizaDAO _polizaDao = new PolizaDAO();
    private readonly CuotaDAO _cuotaDao = new CuotaDAO();

    public List<Asegurado> ListarAsegurados(string filtro = "") => _aseguradoDao.ObtenerTodos(filtro);

    public List<Ramo> ListarRamos() => _ramoDao.ObtenerTodos();

    public List<PolizaResumenDTO> ListarPolizas(string filtro = "") => _polizaDao.ObtenerTodas(filtro);

    public List<Cuota> ListarCuotasPorPoliza(int idPoliza) => _cuotaDao.ObtenerPorPoliza(idPoliza);

    public (bool Exito, string Mensaje) GuardarAsegurado(int id, string dni, string nombre, string telefono, string email, string domicilio)
    {
        if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(telefono))
            return (false, "DNI/CUIT, Apellido y Nombre, y Teléfono son obligatorios.");

        if (_aseguradoDao.ExisteDni(dni.Trim(), id))
            return (false, $"Ya existe un asegurado registrado con el DNI/CUIT {dni.Trim()}.");

        var asegurado = new Asegurado(id, dni.Trim(), nombre.Trim(), telefono.Trim(), email?.Trim(), domicilio?.Trim());
        if (id == 0)
        {
            _aseguradoDao.Insertar(asegurado);
            return (true, "Asegurado registrado correctamente.");
        }
        else
        {
            _aseguradoDao.Actualizar(asegurado);
            return (true, "Datos del asegurado actualizados correctamente.");
        }
    }

    public (bool Exito, string Mensaje) EliminarAsegurado(int id)
    {
        if (id <= 0) return (false, "Seleccione un asegurado válido.");
        bool ok = _aseguradoDao.DarDeBaja(id);
        return ok ? (true, "Asegurado dado de baja correctamente.") : (false, "No se pudo dar de baja al asegurado.");
    }

    public (bool Exito, string Mensaje) EmitirPoliza(
        int numeroPoliza, int idAsegurado, int idRamo, string compania,
        string bienAsegurado, decimal primaTotal, int cantidadCuotas,
        DateTime fechaInicio, DateTime fechaFin, DateTime primerVencimiento)
    {
        if (numeroPoliza <= 0) return (false, "El número de póliza debe ser mayor a cero.");
        if (idAsegurado <= 0) return (false, "Debe seleccionar un asegurado.");
        if (idRamo <= 0) return (false, "Debe seleccionar un ramo.");
        if (string.IsNullOrWhiteSpace(bienAsegurado)) return (false, "Debe describir el bien asegurado.");
        if (primaTotal <= 0) return (false, "La prima total debe ser mayor a cero.");
        if (cantidadCuotas < 1 || cantidadCuotas > 24) return (false, "La cantidad de cuotas debe estar entre 1 y 24.");
        if (fechaFin <= fechaInicio) return (false, "La fecha de fin debe ser posterior a la fecha de inicio.");
        if (_polizaDao.ExisteNumeroPoliza(numeroPoliza))
            return (false, $"La póliza #{numeroPoliza} ya se encuentra registrada.");

        _polizaDao.EmitirPolizaConCuotas(
            numeroPoliza, idAsegurado, idRamo,
            string.IsNullOrWhiteSpace(compania) ? "Federación Patronal Seguros" : compania.Trim(),
            bienAsegurado.Trim(), primaTotal, cantidadCuotas,
            fechaInicio, fechaFin, primerVencimiento);

        return (true, $"Póliza #{numeroPoliza} emitida con {cantidadCuotas} cuotas generadas.");
    }
}
