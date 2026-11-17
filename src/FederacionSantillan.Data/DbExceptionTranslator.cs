using FederacionSantillan.Core.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data;

public static class DbExceptionTranslator
{
    public static T Ejecutar<T>(Func<T> operacion, string mensajeDuplicado = null)
    {
        try
        {
            return operacion();
        }
        catch (MySqlException ex) when (ex.Number == 1062 && !string.IsNullOrEmpty(mensajeDuplicado))
        {
            throw new EntidadDuplicadaException(mensajeDuplicado, ex);
        }
        catch (MySqlException ex) when (ex.Number == 1042 || ex.Number == 0)
        {
            throw new PersistenciaException(MensajesSistema.ErrorConexionDb, ex);
        }
        catch (MySqlException ex)
        {
            throw new PersistenciaException(MensajesSistema.Formatear(MensajesSistema.ErrorPersistenciaGenerico, ex.Message), ex);
        }
    }

    public static void Ejecutar(Action operacion, string mensajeDuplicado = null) =>
        Ejecutar(() => { operacion(); return true; }, mensajeDuplicado);
}
