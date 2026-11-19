using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.Model.DTOs.Requests;

public class SignInRequestDto : IValidatableObject
{
    [Required(AllowEmptyStrings = false, ErrorMessage = SystemMessages.CredentialsRequired)]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = SystemMessages.CredentialsRequired)]
    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            yield return new ValidationResult(SystemMessages.CredentialsRequired);
    }
}

public class PaymentRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = SystemMessages.SelectionRequired)]
    public int InstallmentId { get; set; }

    [Required(ErrorMessage = SystemMessages.SelectionRequired)]
    [RegularExpression(@"^(Transferencia|Efectivo|Débito Automático|MercadoPago)$", ErrorMessage = "Debe seleccionar un medio de pago válido.")]
    public string PaymentMethod { get; set; }

    public string Notes { get; set; } = string.Empty;
}
