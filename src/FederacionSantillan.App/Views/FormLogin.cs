using FederacionSantillan.App.Advice;
using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Data.Factories;
using FederacionSantillan.Model.DTOs.Requests;

namespace FederacionSantillan.App.Views;

public partial class FormLogin : Form
{
    private readonly AuthController _authController = new();

    public FormLogin()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
        btnIngresar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "check");
        btnCrearUsuario.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(30, 64, 175), "plus");
        btnSalir.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(51, 65, 85), "close");
    }

    private void FormLogin_Load(object sender, EventArgs e)
    {
        CheckDatabaseConnection();
        txtUsuario.Focus();
    }

    private void CheckDatabaseConnection()
    {
        bool connected = ConnectionFactory.TestConnection(out string error);
        if (connected)
        {
            lblEstadoDb.Visible = false;
        }
        else
        {
            lblEstadoDb.Text = $"Sin conexión con el servidor de datos: {error}";
            lblEstadoDb.ForeColor = Color.FromArgb(185, 28, 28);
            lblEstadoDb.Visible = true;
        }
    }

    private void Campos_TextChanged(object sender, EventArgs e)
    {
        if (sender is Control ctrl)
            errorProvider.SetError(ctrl, string.Empty);
    }

    private void chkMostrarPassword_CheckedChanged(object sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !chkMostrarPassword.Checked;
        txtPassword.PasswordChar = chkMostrarPassword.Checked ? '\0' : '*';
    }

    private void btnIngresar_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        var dto = new SignInRequestDto { Username = txtUsuario.Text, Password = txtPassword.Text };

        ControllerPipeline.Execute(dto, req =>
        {
            _authController.SignIn(req);
            DialogResult = DialogResult.OK;
            Close();
        }, txtPassword);
    }

    private void btnCrearUsuario_Click(object sender, EventArgs e) =>
        ControllerPipeline.Execute(() =>
        {
            using var formReg = new FormRegistroUsuario();
            formReg.ShowDialog(this);
        });

    private void btnSalir_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
