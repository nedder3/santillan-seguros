using System.ComponentModel.DataAnnotations;

namespace FederacionSantillan.Core.DTOs;

public class RegistroUsuarioDTO
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre completo debe tener entre 3 y 120 caracteres.")]
    public string NombreCompleto { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Debe confirmar la contraseña.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
    public string ConfirmarPassword { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un rol.")]
    public string Rol { get; set; }
}
