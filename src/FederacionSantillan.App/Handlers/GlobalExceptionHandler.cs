using FederacionSantillan.Core.Exceptions;

namespace FederacionSantillan.App.Handlers;

public static class GlobalExceptionHandler
{
    public static void Ejecutar(Action accion, Control focoEnError = null)
    {
        try
        {
            accion();
        }
        catch (EntidadDuplicadaException ex)
        {
            MessageBox.Show(ex.Message, "Registro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focoEnError?.Focus();
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focoEnError?.Focus();
        }
        catch (PersistenciaException ex)
        {
            MessageBox.Show(ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                MensajesSistema.Formatear(MensajesSistema.ErrorPersistenciaGenerico, ex.Message),
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
