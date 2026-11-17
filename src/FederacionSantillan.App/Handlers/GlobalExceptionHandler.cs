using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.App.Handlers;

public static class GlobalExceptionHandler
{
    public static void Execute(Action action, Control focusOnError = null)
    {
        try
        {
            action();
        }
        catch (DuplicateEntityException ex)
        {
            MessageBox.Show(ex.Message, "Registro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusOnError?.Focus();
        }
        catch (BusinessRuleException ex)
        {
            MessageBox.Show(ex.Message, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusOnError?.Focus();
        }
        catch (PersistenceException ex)
        {
            MessageBox.Show(ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                SystemMessages.Format(SystemMessages.GenericPersistenceError, ex.Message),
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
