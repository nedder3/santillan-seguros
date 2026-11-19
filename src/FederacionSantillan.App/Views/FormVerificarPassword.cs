using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Model.Exceptions;

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
            MessageBox.Show(SystemMessages.InvalidCredentials, "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.Clear();
            txtPassword.Focus();
        }
    }
}
