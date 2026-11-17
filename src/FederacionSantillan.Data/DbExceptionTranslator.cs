using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data;

public static class DbExceptionTranslator
{
    public static T Execute<T>(Func<T> operation, string duplicateMessage = null)
    {
        try
        {
            return operation();
        }
        catch (MySqlException ex) when (ex.Number == 1062 && !string.IsNullOrEmpty(duplicateMessage))
        {
            throw new DuplicateEntityException(duplicateMessage, ex);
        }
        catch (MySqlException ex) when (ex.Number == 1042 || ex.Number == 0)
        {
            throw new PersistenceException(SystemMessages.DatabaseConnectionError, ex);
        }
        catch (MySqlException ex)
        {
            throw new PersistenceException(SystemMessages.Format(SystemMessages.GenericPersistenceError, ex.Message), ex);
        }
    }

    public static void Execute(Action operation, string duplicateMessage = null) =>
        Execute(() => { operation(); return true; }, duplicateMessage);
}
