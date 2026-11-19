using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Model.Validators;

namespace FederacionSantillan.Model.DTOs.Requests;

public class InsuredRequestDto : IValidatableObject
{
    public int Id { get; set; }

    public string DocumentType { get; set; } = "DNI";

    [Required(AllowEmptyStrings = false, ErrorMessage = "El DNI o CUIT es obligatorio.")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El DNI/CUIT debe tener entre 7 y 20 caracteres.")]
    public string TaxId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido y nombre es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s.'\-]+$", ErrorMessage = "El apellido y nombre contiene caracteres no válidos.")]
    public string FullName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El teléfono o WhatsApp es obligatorio.")]
    [StringLength(15, MinimumLength = 8, ErrorMessage = "El teléfono debe tener entre 8 y 15 dígitos numéricos.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono solo puede contener números (sin guiones ni espacios).")]
    public string Phone { get; set; }

    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    [StringLength(180, ErrorMessage = "El domicilio no puede superar los 180 caracteres.")]
    public string Address { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(TaxId))
            yield return new ValidationResult("El DNI o CUIT es obligatorio.", new[] { nameof(TaxId) });

        if (!ArgentineDocumentValidator.ValidateDocument(DocumentType, TaxId, out string docError))
            yield return new ValidationResult(docError, new[] { nameof(TaxId) });

        if (string.IsNullOrWhiteSpace(FullName))
            yield return new ValidationResult("El apellido y nombre es obligatorio.", new[] { nameof(FullName) });

        if (string.IsNullOrWhiteSpace(Phone))
            yield return new ValidationResult("El teléfono o WhatsApp es obligatorio.", new[] { nameof(Phone) });
    }
}
