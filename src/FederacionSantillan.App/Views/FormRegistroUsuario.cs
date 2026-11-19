using FederacionSantillan.App.Advice;
using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Model.DTOs.Requests;

namespace FederacionSantillan.App.Views;

public partial class FormRegistroUsuario : Form
{
    private readonly AuthController _authController = new();

    public FormRegistroUsuario()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
        btnGuardar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "check");
        btnCancelar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(51, 65, 85), "close");

        txtNombreCompleto.KeyPress += (_, e) =>
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar is not ('.' or '\'' or '-'))
                e.Handled = true;
        };

        txtUsername.KeyPress += (_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar is not ('.' or '_'))
                e.Handled = true;
        };
    }

    private void Campos_TextChanged(object sender, EventArgs e)
    {
        if (sender is Control ctrl)
            errorProvider.SetError(ctrl, string.Empty);
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        var dto = new RegisterUserRequestDto
        {
            FullName = txtNombreCompleto.Text,
            Username = txtUsername.Text,
            Password = txtPassword.Text,
            ConfirmPassword = txtConfirmarPassword.Text,
            Role = cmbRol.SelectedItem?.ToString() ?? "PRODUCTOR"
        };

        ControllerPipeline.Execute(dto, req =>
        {
            _authController.RegisterUser(req);
            MessageBox.Show("Usuario registrado exitosamente. Ya puede iniciar sesión.", "Usuario Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }, txtNombreCompleto);
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
