using System.ComponentModel.DataAnnotations;

namespace FederacionSantillan.Core.DTOs;

public class PolizaInputDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "El número de póliza debe ser mayor a cero.")]
    public int NumeroPoliza { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un asegurado titular.")]
    public int IdAsegurado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un ramo de cobertura.")]
    public int IdRamo { get; set; }

    [Required(ErrorMessage = "La compañía aseguradora es obligatoria.")]
    public string Compania { get; set; }

    [Required(ErrorMessage = "Debe describir el bien asegurado.")]
    [StringLength(180, MinimumLength = 3, ErrorMessage = "La descripción del bien debe tener al menos 3 caracteres.")]
    public string BienAsegurado { get; set; }

    [Range(1.0, 999999999.0, ErrorMessage = "La prima total debe ser mayor a cero.")]
    public decimal PrimaTotal { get; set; }

    [Range(1, 24, ErrorMessage = "La cantidad de cuotas debe estar entre 1 y 24.")]
    public int CantidadCuotas { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime PrimerVencimiento { get; set; }
}
