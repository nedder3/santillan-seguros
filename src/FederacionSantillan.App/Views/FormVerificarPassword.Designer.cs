namespace FederacionSantillan.App.Views;

partial class FormVerificarPassword
{
    private System.ComponentModel.IContainer components = null;
    private Label lblMensaje;
    private TextBox txtPassword;
    private Button btnConfirmar;
    private Button btnCancelar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblMensaje = new Label();
        txtPassword = new TextBox();
        btnConfirmar = new Button();
        btnCancelar = new Button();
        SuspendLayout();

        lblMensaje.AutoSize = true;
        lblMensaje.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblMensaje.Location = new Point(22, 20);
        lblMensaje.Text = "Ingrese su contraseña actual para acceder a Opciones:";

        txtPassword.AccessibleName = "Contraseña actual";
        txtPassword.Font = new Font("Segoe UI", 10.5F);
        txtPassword.Location = new Point(22, 48);
        txtPassword.MaxLength = 100;
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(340, 26);
        txtPassword.TabIndex = 0;
        txtPassword.UseSystemPasswordChar = true;

        btnConfirmar.BackColor = Color.FromArgb(21, 128, 61);
        btnConfirmar.FlatAppearance.BorderSize = 0;
        btnConfirmar.FlatStyle = FlatStyle.Flat;
        btnConfirmar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnConfirmar.ForeColor = Color.White;
        btnConfirmar.Location = new Point(22, 92);
        btnConfirmar.Size = new Size(162, 36);
        btnConfirmar.TabIndex = 1;
        btnConfirmar.Text = "Confirmar";
        btnConfirmar.Click += btnConfirmar_Click;

        btnCancelar.BackColor = Color.FromArgb(100, 116, 139);
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCancelar.ForeColor = Color.White;
        btnCancelar.Location = new Point(200, 92);
        btnCancelar.Size = new Size(162, 36);
        btnCancelar.TabIndex = 2;
        btnCancelar.Text = "Cancelar";
        btnCancelar.Click += btnCancelar_Click;

        AcceptButton = btnConfirmar;
        CancelButton = btnCancelar;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(385, 148);
        Controls.Add(btnCancelar);
        Controls.Add(btnConfirmar);
        Controls.Add(txtPassword);
        Controls.Add(lblMensaje);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormVerificarPassword";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Seguridad - Verificar Identidad";
        ResumeLayout(false);
        PerformLayout();
    }
}
