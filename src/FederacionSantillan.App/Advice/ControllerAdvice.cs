using FederacionSantillan.Model.DTOs.Responses;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.App.Advice;

public static class ControllerAdvice
{
    public static ErrorResponseDto Resolve(Exception exception) => exception switch
    {
        DuplicateResourceException dup => new(dup.Title, dup.Message, IsFatal: false),
        ValidationException val => new(val.Title, val.Message, IsFatal: false),
        ResourceNotFoundException notFound => new(notFound.Title, notFound.Message, IsFatal: false),
        DatabaseUnavailableException db => new(db.Title, db.Message, IsFatal: true),
        AppException app => new(app.Title, app.Message, IsFatal: false),
        _ => new("Error Inesperado", SystemMessages.Format(SystemMessages.GenericPersistenceError, exception.Message), IsFatal: true)
    };

    public static void Handle(Exception exception, Control focusOnError = null)
    {
        var response = Resolve(exception);
        var icon = response.IsFatal ? MessageBoxIcon.Error : MessageBoxIcon.Warning;
        MessageBox.Show(response.Message, response.Title, MessageBoxButtons.OK, icon);
        if (!response.IsFatal)
            focusOnError?.Focus();
    }
}
