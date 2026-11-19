using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.Model.DTOs.Requests;

public class PolicyRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "El número de póliza debe ser mayor a cero.")]
    public int PolicyNumber { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un asegurado titular.")]
    public int InsuredId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un ramo de cobertura.")]
    public int BranchId { get; set; }

    [Required(ErrorMessage = "La compañía aseguradora es obligatoria.")]
    public string Company { get; set; }

    [Required(ErrorMessage = "Debe describir el bien asegurado.")]
    [StringLength(180, MinimumLength = 3, ErrorMessage = "La descripción del bien debe tener al menos 3 caracteres.")]
    public string InsuredAsset { get; set; }

    [Range(1.0, 999999999.0, ErrorMessage = "La prima total debe ser mayor a cero.")]
    public decimal TotalPremium { get; set; }

    [Range(1, 24, ErrorMessage = "La cantidad de cuotas debe estar entre 1 y 24.")]
    public int InstallmentsCount { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today.AddYears(1);
    public DateTime FirstDueDate { get; set; } = DateTime.Today.AddMonths(1);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate <= StartDate)
            yield return new ValidationResult(SystemMessages.InvalidPolicyDates, new[] { nameof(EndDate) });

        if (string.IsNullOrWhiteSpace(InsuredAsset))
            yield return new ValidationResult("Debe describir el bien asegurado.", new[] { nameof(InsuredAsset) });

        if (FirstDueDate.Date < StartDate.Date)
            yield return new ValidationResult(SystemMessages.InvalidFirstDueDate, new[] { nameof(FirstDueDate) });
    }
}
