using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Handlers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Core.DTOs;

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

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        var dto = new RegistroUsuarioDTO
        {
            NombreCompleto = txtNombreCompleto.Text,
            Username = txtUsername.Text,
            Password = txtPassword.Text,
            ConfirmarPassword = txtConfirmarPassword.Text,
            Rol = cmbRol.SelectedItem?.ToString() ?? "PRODUCTOR"
        };

        GlobalExceptionHandler.Ejecutar(() =>
        {
            _authController.RegistrarUsuario(dto);
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
