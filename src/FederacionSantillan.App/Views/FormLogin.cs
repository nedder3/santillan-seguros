using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Data;

namespace FederacionSantillan.App.Views;

public partial class FormLogin : Form
{
    private readonly AuthController _authController = new AuthController();

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
        try
        {
            if (ConnectionFactory.TestConnection(out string err))
            {
                lblEstadoDb.Text = "Base de datos: conectada (MariaDB - santillan_seguros)";
                lblEstadoDb.ForeColor = Color.FromArgb(21, 128, 61);
            }
            else
            {
                lblEstadoDb.Text = $"Sin conexión a MariaDB: {err}";
                lblEstadoDb.ForeColor = Color.FromArgb(185, 28, 28);
            }
        }
        catch (Exception ex)
        {
            lblEstadoDb.Text = $"Error de red: {ex.Message}";
            lblEstadoDb.ForeColor = Color.FromArgb(185, 28, 28);
        }
        txtUsuario.Focus();
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
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            errorProvider.SetError(txtUsuario, "Ingrese su nombre de usuario.");
            txtUsuario.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            errorProvider.SetError(txtPassword, "Ingrese su contraseña.");
            txtPassword.Focus();
            return;
        }

        try
        {
            var (exito, mensaje, _) = _authController.IniciarSesion(txtUsuario.Text, txtPassword.Text);
            if (exito)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(mensaje, "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo conectar con la base de datos MariaDB.\nDetalle: {ex.Message}",
                "Error de Conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnCrearUsuario_Click(object sender, EventArgs e)
    {
        try
        {
            using var formReg = new FormRegistroUsuario();
            formReg.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnSalir_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
