using System.ComponentModel.DataAnnotations;

namespace FederacionSantillan.Core.DTOs;

public class AseguradoInputDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El DNI o CUIT es obligatorio.")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El DNI/CUIT debe tener entre 7 y 20 caracteres.")]
    public string DniCuit { get; set; }

    [Required(ErrorMessage = "El apellido y nombre es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    public string ApellidoNombre { get; set; }

    [Required(ErrorMessage = "El teléfono o WhatsApp es obligatorio.")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "El teléfono debe tener al menos 6 caracteres.")]
    public string Telefono { get; set; }

    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    [StringLength(180, ErrorMessage = "El domicilio no puede superar los 180 caracteres.")]
    public string Domicilio { get; set; }
}
