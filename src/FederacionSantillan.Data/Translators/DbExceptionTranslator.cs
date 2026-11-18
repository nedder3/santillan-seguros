using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.Translators;

public static class DbExceptionTranslator
{
    public static T Execute<T>(Func<T> operation, string duplicateMessage = null)
    {
        try
        {
            return operation();
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry && !string.IsNullOrEmpty(duplicateMessage))
        {
            throw new DuplicateResourceException(duplicateMessage, ex);
        }
        catch (MySqlException ex) when (ex.ErrorCode is MySqlErrorCode.UnableToConnectToHost or MySqlErrorCode.None)
        {
            throw new DatabaseUnavailableException(SystemMessages.DatabaseConnectionError, ex);
        }
        catch (MySqlException ex)
        {
            throw new DatabaseUnavailableException(SystemMessages.Format(SystemMessages.GenericPersistenceError, ex.Message), ex);
        }
    }

    public static void Execute(Action operation, string duplicateMessage = null) =>
        Execute(() => { operation(); return true; }, duplicateMessage);
}
