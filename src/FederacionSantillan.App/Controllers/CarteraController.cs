using FederacionSantillan.Core;
using FederacionSantillan.Core.DTOs;
using FederacionSantillan.Core.Exceptions;
using FederacionSantillan.Core.Validation;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

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

    public string GuardarAsegurado(AseguradoInputDTO dto)
    {
        DtoValidator.Validar(dto);

        string dni = dto.DniCuit.Trim();
        if (_aseguradoDao.ExisteDni(dni, dto.Id))
            throw new EntidadDuplicadaException(MensajesSistema.Formatear(MensajesSistema.DniDuplicado, dni));

        var asegurado = new Asegurado(
            dto.Id,
            dni,
            dto.ApellidoNombre.Trim(),
            dto.Telefono.Trim(),
            dto.Email?.Trim(),
            dto.Domicilio?.Trim());

        if (dto.Id == 0)
        {
            _aseguradoDao.Insertar(asegurado);
            return "Asegurado registrado correctamente.";
        }

        _aseguradoDao.Actualizar(asegurado);
        return "Datos del asegurado actualizados correctamente.";
    }

    public void EliminarAsegurado(int id)
    {
        if (id <= 0)
            throw new NegocioException(MensajesSistema.SeleccionRequerida);

        _aseguradoDao.DarDeBaja(id);
    }

    public string EmitirPoliza(PolizaInputDTO dto)
    {
        DtoValidator.Validar(dto);

        if (dto.FechaFin <= dto.FechaInicio)
            throw new NegocioException(MensajesSistema.VigenciaInvalida);

        if (_polizaDao.ExisteNumeroPoliza(dto.NumeroPoliza))
            throw new EntidadDuplicadaException(MensajesSistema.Formatear(MensajesSistema.PolizaDuplicada, dto.NumeroPoliza));

        _polizaDao.EmitirPolizaConCuotas(
            dto.NumeroPoliza,
            dto.IdAsegurado,
            dto.IdRamo,
            dto.Compania.Trim(),
            dto.BienAsegurado.Trim(),
            dto.PrimaTotal,
            dto.CantidadCuotas,
            dto.FechaInicio,
            dto.FechaFin,
            dto.PrimerVencimiento);

        return $"Póliza #{dto.NumeroPoliza} emitida con {dto.CantidadCuotas} cuotas generadas.";
    }
}
