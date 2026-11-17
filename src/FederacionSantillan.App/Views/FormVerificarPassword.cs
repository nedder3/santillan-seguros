using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;

namespace FederacionSantillan.App.Views;

public partial class FormVerificarPassword : Form
{
    private readonly AuthController _authController = new AuthController();

    public FormVerificarPassword()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
    }

    private void btnConfirmar_Click(object sender, EventArgs e)
    {
        if (_authController.VerifyCurrentPassword(txtPassword.Text))
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show("Contraseña incorrecta. Acceso denegado.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.Clear();
            txtPassword.Focus();
        }
    }
}
