using FederacionSantillan.App.Advice;
using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Views;
using FederacionSantillan.Model.DTOs.Requests;
using System.Drawing.Imaging;
using System.Reflection;

namespace FederacionSantillan.App;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Contains("--capture-docs"))
        {
            string outDir = args.SkipWhile(a => a != "--capture-docs").Skip(1).FirstOrDefault() ?? "screenshots";
            CaptureDocumentationScreenshots(outDir);
            return;
        }

        Application.ThreadException += (_, e) => ControllerAdvice.Handle(e.Exception);

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

    private static void CaptureDocumentationScreenshots(string outDir)
    {
        Directory.CreateDirectory(outDir);

        static void SaveFormBitmap(Form form, string filePath, Action afterShown = null)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(40, 40);
            form.Show();
            Application.DoEvents();
            afterShown?.Invoke();
            Application.DoEvents();
            Thread.Sleep(250);
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Hide();
        }

        using (var login = new FormLogin())
        {
            SaveFormBitmap(login, Path.Combine(outDir, "01_form_login.png"));
        }

        using (var register = new FormRegistroUsuario())
        {
            SaveFormBitmap(register, Path.Combine(outDir, "02_form_registro_usuario.png"));
        }

        new AuthController().SignIn(new SignInRequestDto { Username = "santillan", Password = "seguros2026" });

        using (var main = new FormPrincipal())
        {
            var tabField = typeof(FormPrincipal).GetField("tabMain", BindingFlags.Instance | BindingFlags.NonPublic);
            var tabControl = (TabControl)tabField?.GetValue(main);

            SaveFormBitmap(main, Path.Combine(outDir, "03_form_principal_alertas.png"), () =>
            {
                if (tabControl != null) tabControl.SelectedIndex = 0;
            });

            SaveFormBitmap(main, Path.Combine(outDir, "04_form_principal_asegurados.png"), () =>
            {
                if (tabControl != null) tabControl.SelectedIndex = 1;
            });

            SaveFormBitmap(main, Path.Combine(outDir, "05_form_principal_polizas.png"), () =>
            {
                if (tabControl != null) tabControl.SelectedIndex = 2;
            });
        }

        using (var verify = new FormVerificarPassword())
        {
            SaveFormBitmap(verify, Path.Combine(outDir, "06_form_verificar_password.png"));
        }

        using (var options = new FormOpciones())
        {
            SaveFormBitmap(options, Path.Combine(outDir, "07_form_opciones.png"));
        }

        using (var help = new FormAyuda())
        {
            SaveFormBitmap(help, Path.Combine(outDir, "08_form_ayuda.png"));
        }
    }
}
