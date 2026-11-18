namespace FederacionSantillan.Model.Exceptions;

public abstract class AppException(string title, string message, Exception inner = null)
    : Exception(message, inner)
{
    public string Title { get; } = title;
}

public sealed class ValidationException(string message)
    : AppException("Validación de Datos", message);

public sealed class DuplicateResourceException(string message, Exception inner = null)
    : AppException("Registro Duplicado", message, inner);

public sealed class ResourceNotFoundException(string message)
    : AppException("Recurso No Encontrado", message);

public sealed class DatabaseUnavailableException(string message, Exception inner = null)
    : AppException("Error de Base de Datos", message, inner);
