using System.ComponentModel.DataAnnotations;
using FederacionSantillan.Model.Exceptions;
using ValidationException = FederacionSantillan.Model.Exceptions.ValidationException;

namespace FederacionSantillan.App.Advice;

public static class ControllerPipeline
{
    public static void Validate<TDto>(TDto dto)
    {
        if (dto == null)
            throw new ValidationException(SystemMessages.SelectionRequired);

        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(dto, context, results, validateAllProperties: true))
        {
            string details = string.Join(Environment.NewLine, results.Select(r => $"- {r.ErrorMessage}"));
            throw new ValidationException(details);
        }
    }

    public static void Execute(Action action, Control focusOnError = null)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            ControllerAdvice.Handle(ex, focusOnError);
        }
    }

    public static void Execute<TDto>(TDto dto, Action<TDto> action, Control focusOnError = null) =>
        Execute(() =>
        {
            Validate(dto);
            action(dto);
        }, focusOnError);
}
