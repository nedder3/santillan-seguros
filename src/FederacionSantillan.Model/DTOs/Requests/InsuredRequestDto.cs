using System.ComponentModel.DataAnnotations;

namespace FederacionSantillan.Model.DTOs.Requests;

public class InsuredRequestDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El DNI o CUIT es obligatorio.")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El DNI/CUIT debe tener entre 7 y 20 caracteres.")]
    [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "El DNI/CUIT solo puede contener números y guiones (ej. 20-34567890-1).")]
    public string TaxId { get; set; }

    [Required(ErrorMessage = "El apellido y nombre es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "El teléfono o WhatsApp es obligatorio.")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "El teléfono debe tener al menos 6 caracteres.")]
    [RegularExpression(@"^[0-9+\-\s()]+$", ErrorMessage = "El teléfono solo puede contener números, espacios o guiones (ej. 291-4550011).")]
    public string Phone { get; set; }

    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    [StringLength(180, ErrorMessage = "El domicilio no puede superar los 180 caracteres.")]
    public string Address { get; set; }
}
