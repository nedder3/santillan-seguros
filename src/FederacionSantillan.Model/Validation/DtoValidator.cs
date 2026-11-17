using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.Model.Validation;

public static class DtoValidator
{
    public static void Validate(object dto)
    {
        if (dto == null)
            throw new BusinessRuleException(SystemMessages.SelectionRequired);

        var context = new ValidationContext(dto);
        var errors = new List<ValidationResult>();

        if (!Validator.TryValidateObject(dto, context, errors, validateAllProperties: true))
        {
            string details = string.Join(Environment.NewLine, errors.Select(e => $"- {e.ErrorMessage}"));
            throw new BusinessRuleException(details);
        }
    }
}
