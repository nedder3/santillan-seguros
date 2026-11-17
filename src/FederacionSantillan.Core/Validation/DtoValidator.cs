using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Core.Exceptions;

namespace FederacionSantillan.Core.Validation;

public static class DtoValidator
{
    public static void Validar(object dto)
    {
        if (dto == null)
            throw new NegocioException(MensajesSistema.SeleccionRequerida);

        var contexto = new ValidationContext(dto);
        var errores = new List<ValidationResult>();

        bool esValido = Validator.TryValidateObject(dto, contexto, errores, validateAllProperties: true);
        if (!esValido)
        {
            string detalle = string.Join(Environment.NewLine, errores.Select(e => $"- {e.ErrorMessage}"));
            throw new NegocioException(detalle);
        }
    }
}
