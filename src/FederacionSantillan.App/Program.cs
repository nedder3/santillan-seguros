using FederacionSantillan.App.Views;

namespace FederacionSantillan.App;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Manejador global para evitar salidas abruptas o excepciones no controladas (Rúbrica)
        Application.ThreadException += (sender, args) =>
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado: {args.Exception.Message}\n\nVerifique la conexión con MariaDB.",
                "Federación Santillán Seguros - Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        };

        bool continuar = true;
        while (continuar)
        {
            using var login = new FormLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                using var principal = new FormPrincipal();
                var res = principal.ShowDialog();
                continuar = (res == DialogResult.Retry); // Retry = Cerrar sesión y volver al Login
            }
            else
            {
                continuar = false;
            }
        }
    }
}
