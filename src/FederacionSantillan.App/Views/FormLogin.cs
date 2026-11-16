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
        btnIngresar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(20, 83, 45), "✓");
        btnCrearUsuario.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(30, 64, 175), "+");
        btnSalir.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(51, 65, 85), "✕");
    }

    private void FormLogin_Load(object sender, EventArgs e)
    {
        try
        {
            if (ConnectionFactory.TestConnection(out string err))
            {
                lblEstadoDb.Text = "● Conectado a MariaDB (santillan_seguros)";
                lblEstadoDb.ForeColor = Color.FromArgb(22, 163, 74);
            }
            else
            {
                lblEstadoDb.Text = $"● Sin conexión a MariaDB: {err}";
                lblEstadoDb.ForeColor = Color.Crimson;
            }
        }
        catch (Exception ex)
        {
            lblEstadoDb.Text = $"● Error de red: {ex.Message}";
            lblEstadoDb.ForeColor = Color.Crimson;
        }
        txtUsuario.Focus();
    }

    private void chkMostrarPassword_CheckedChanged(object sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !chkMostrarPassword.Checked;
        txtPassword.PasswordChar = chkMostrarPassword.Checked ? '\0' : '*';
    }

    private void btnIngresar_Click(object sender, EventArgs e)
    {
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
