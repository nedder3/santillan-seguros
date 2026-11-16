namespace FederacionSantillan.App.Views;

partial class FormRegistroUsuario
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitulo;
    private Label lblNombre;
    private TextBox txtNombreCompleto;
    private Label lblUsuario;
    private TextBox txtUsername;
    private Label lblPass;
    private TextBox txtPassword;
    private Label lblConfirmar;
    private TextBox txtConfirmarPassword;
    private Label lblRol;
    private ComboBox cmbRol;
    private Button btnGuardar;
    private Button btnCancelar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        lblNombre = new Label();
        txtNombreCompleto = new TextBox();
        lblUsuario = new Label();
        txtUsername = new TextBox();
        lblPass = new Label();
        txtPassword = new TextBox();
        lblConfirmar = new Label();
        txtConfirmarPassword = new TextBox();
        lblRol = new Label();
        cmbRol = new ComboBox();
        btnGuardar = new Button();
        btnCancelar = new Button();
        SuspendLayout();

        // lblTitulo
        lblTitulo.BackColor = Color.FromArgb(15, 76, 129);
        lblTitulo.Dock = DockStyle.Top;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(0, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Padding = new Padding(16, 0, 0, 0);
        lblTitulo.Size = new Size(420, 48);
        lblTitulo.Text = "Registro de Nuevo Usuario";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

        // lblNombre
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.Location = new Point(28, 66);
        lblNombre.Name = "lblNombre";
        lblNombre.Text = "Nombre y Apellido:";

        // txtNombreCompleto
        txtNombreCompleto.Font = new Font("Segoe UI", 10F);
        txtNombreCompleto.Location = new Point(28, 86);
        txtNombreCompleto.Name = "txtNombreCompleto";
        txtNombreCompleto.Size = new Size(364, 25);
        txtNombreCompleto.TabIndex = 0;

        // lblUsuario
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUsuario.Location = new Point(28, 122);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Text = "Nombre de Usuario:";

        // txtUsername
        txtUsername.Font = new Font("Segoe UI", 10F);
        txtUsername.Location = new Point(28, 142);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(364, 25);
        txtUsername.TabIndex = 1;

        // lblPass
        lblPass.AutoSize = true;
        lblPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPass.Location = new Point(28, 178);
        lblPass.Name = "lblPass";
        lblPass.Text = "Contraseña:";

        // txtPassword
        txtPassword.Font = new Font("Segoe UI", 10F);
        txtPassword.Location = new Point(28, 198);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(364, 25);
        txtPassword.TabIndex = 2;
        txtPassword.UseSystemPasswordChar = true;

        // lblConfirmar
        lblConfirmar.AutoSize = true;
        lblConfirmar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblConfirmar.Location = new Point(28, 234);
        lblConfirmar.Name = "lblConfirmar";
        lblConfirmar.Text = "Confirmar Contraseña:";

        // txtConfirmarPassword
        txtConfirmarPassword.Font = new Font("Segoe UI", 10F);
        txtConfirmarPassword.Location = new Point(28, 254);
        txtConfirmarPassword.Name = "txtConfirmarPassword";
        txtConfirmarPassword.PasswordChar = '*';
        txtConfirmarPassword.Size = new Size(364, 25);
        txtConfirmarPassword.TabIndex = 3;
        txtConfirmarPassword.UseSystemPasswordChar = true;

        // lblRol
        lblRol.AutoSize = true;
        lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblRol.Location = new Point(28, 290);
        lblRol.Name = "lblRol";
        lblRol.Text = "Rol del Usuario:";

        // cmbRol
        cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRol.Font = new Font("Segoe UI", 10F);
        cmbRol.Items.AddRange(new object[] { "PRODUCTOR", "OPERADOR", "ADMIN" });
        cmbRol.Location = new Point(28, 310);
        cmbRol.Name = "cmbRol";
        cmbRol.SelectedIndex = 0;
        cmbRol.Size = new Size(364, 25);
        cmbRol.TabIndex = 4;

        // btnGuardar
        btnGuardar.BackColor = Color.FromArgb(22, 163, 74);
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnGuardar.ForeColor = Color.White;
        btnGuardar.Location = new Point(28, 355);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(176, 36);
        btnGuardar.TabIndex = 5;
        btnGuardar.Text = "  Crear Usuario";
        btnGuardar.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnGuardar.UseVisualStyleBackColor = false;
        btnGuardar.Click += btnGuardar_Click;

        // btnCancelar
        btnCancelar.BackColor = Color.FromArgb(100, 116, 139);
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCancelar.ForeColor = Color.White;
        btnCancelar.Location = new Point(216, 355);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(176, 36);
        btnCancelar.TabIndex = 6;
        btnCancelar.Text = "  Cancelar";
        btnCancelar.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;

        // FormRegistroUsuario
        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(420, 412);
        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);
        Controls.Add(cmbRol);
        Controls.Add(lblRol);
        Controls.Add(txtConfirmarPassword);
        Controls.Add(lblConfirmar);
        Controls.Add(txtPassword);
        Controls.Add(lblPass);
        Controls.Add(txtUsername);
        Controls.Add(lblUsuario);
        Controls.Add(txtNombreCompleto);
        Controls.Add(lblNombre);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormRegistroUsuario";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Alta de Usuario - Federación Santillán";
        ResumeLayout(false);
        PerformLayout();
    }
}
