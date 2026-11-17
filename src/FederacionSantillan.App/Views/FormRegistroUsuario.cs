using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;

namespace FederacionSantillan.App.Views;

public partial class FormRegistroUsuario : Form
{
    private readonly AuthController _authController = new AuthController();

    public FormRegistroUsuario()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
        btnGuardar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "check");
        btnCancelar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(51, 65, 85), "close");
    }

    private void Campos_TextChanged(object sender, EventArgs e)
    {
        if (sender is Control ctrl)
            errorProvider.SetError(ctrl, string.Empty);
    }

    private bool ValidarCampos()
    {
        errorProvider.Clear();
        if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
        {
            errorProvider.SetError(txtNombreCompleto, "Ingrese el nombre y apellido.");
            txtNombreCompleto.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            errorProvider.SetError(txtUsername, "Ingrese un nombre de usuario.");
            txtUsername.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            errorProvider.SetError(txtPassword, "Ingrese una contraseña.");
            txtPassword.Focus();
            return false;
        }
        if (txtPassword.Text != txtConfirmarPassword.Text)
        {
            errorProvider.SetError(txtConfirmarPassword, "Las contraseñas no coinciden.");
            txtConfirmarPassword.Focus();
            return false;
        }
        return true;
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!ValidarCampos()) return;

        try
        {
            var (exito, mensaje) = _authController.RegistrarUsuario(
                txtNombreCompleto.Text,
                txtUsername.Text,
                txtPassword.Text,
                txtConfirmarPassword.Text,
                cmbRol.SelectedItem?.ToString() ?? "PRODUCTOR");

            if (exito)
            {
                MessageBox.Show(mensaje, "Usuario Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al registrar usuario en MariaDB: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
