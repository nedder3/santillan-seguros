using System.ComponentModel.DataAnnotations;

namespace FederacionSantillan.Model.DTOs.Requests;

public class RegisterUserRequestDto : IValidatableObject
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre y apellido es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre y apellido debe tener entre 3 y 120 caracteres.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s.'\-]+$", ErrorMessage = "El nombre y apellido solo puede contener letras y espacios (no se permiten números).")]
    public string FullName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9._]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, puntos o guiones bajos.")]
    public string Username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
    public string Password { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Debe confirmar la contraseña.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un rol.")]
    [RegularExpression(@"^(ADMIN|PRODUCTOR|OPERADOR)$", ErrorMessage = "Debe seleccionar un rol válido (ADMIN, PRODUCTOR u OPERADOR).")]
    public string Role { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FullName))
            yield return new ValidationResult("El nombre y apellido es obligatorio.", new[] { nameof(FullName) });
        else if (FullName.Any(char.IsDigit))
            yield return new ValidationResult("El nombre y apellido no puede contener números.", new[] { nameof(FullName) });

        if (string.IsNullOrWhiteSpace(Username))
            yield return new ValidationResult("El nombre de usuario es obligatorio.", new[] { nameof(Username) });

        if (string.IsNullOrWhiteSpace(Password))
            yield return new ValidationResult("La contraseña es obligatoria.", new[] { nameof(Password) });
    }
}
