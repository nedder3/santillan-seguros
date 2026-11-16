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
        btnGuardar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(20, 83, 45), "✓");
        btnCancelar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(51, 65, 85), "✕");
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
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
