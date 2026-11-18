using FederacionSantillan.App.Advice;
using FederacionSantillan.App.Views;

namespace FederacionSantillan.App;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.ThreadException += (_, args) => ControllerAdvice.Handle(args.Exception);

        bool keepRunning = true;
        while (keepRunning)
        {
            using var login = new FormLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                using var main = new FormPrincipal();
                keepRunning = main.ShowDialog() == DialogResult.Retry;
            }
            else
            {
                keepRunning = false;
            }
        }
    }
}
