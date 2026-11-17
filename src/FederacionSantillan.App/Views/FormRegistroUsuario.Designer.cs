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
    private ErrorProvider errorProvider;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
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
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();

        // errorProvider
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;

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
        lblNombre.ForeColor = Color.FromArgb(15, 23, 42);
        lblNombre.Location = new Point(28, 66);
        lblNombre.Name = "lblNombre";
        lblNombre.Text = "Nombre y Apellido:";

        // txtNombreCompleto
        txtNombreCompleto.AccessibleDescription = "Ingrese nombre y apellido completo";
        txtNombreCompleto.AccessibleName = "Nombre y Apellido";
        txtNombreCompleto.BorderStyle = BorderStyle.FixedSingle;
        txtNombreCompleto.Font = new Font("Segoe UI", 10F);
        txtNombreCompleto.Location = new Point(28, 86);
        txtNombreCompleto.Name = "txtNombreCompleto";
        txtNombreCompleto.Size = new Size(364, 25);
        txtNombreCompleto.TabIndex = 0;
        txtNombreCompleto.TextChanged += Campos_TextChanged;

        // lblUsuario
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUsuario.ForeColor = Color.FromArgb(15, 23, 42);
        lblUsuario.Location = new Point(28, 122);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Text = "Nombre de Usuario:";

        // txtUsername
        txtUsername.AccessibleDescription = "Ingrese un identificador de usuario sin espacios";
        txtUsername.AccessibleName = "Nombre de Usuario";
        txtUsername.BorderStyle = BorderStyle.FixedSingle;
        txtUsername.Font = new Font("Segoe UI", 10F);
        txtUsername.Location = new Point(28, 142);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(364, 25);
        txtUsername.TabIndex = 1;
        txtUsername.TextChanged += Campos_TextChanged;

        // lblPass
        lblPass.AutoSize = true;
        lblPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPass.ForeColor = Color.FromArgb(15, 23, 42);
        lblPass.Location = new Point(28, 178);
        lblPass.Name = "lblPass";
        lblPass.Text = "Contraseña:";

        // txtPassword
        txtPassword.AccessibleDescription = "Ingrese una contraseña de al menos 6 caracteres";
        txtPassword.AccessibleName = "Contraseña";
        txtPassword.BorderStyle = BorderStyle.FixedSingle;
        txtPassword.Font = new Font("Segoe UI", 10F);
        txtPassword.Location = new Point(28, 198);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(364, 25);
        txtPassword.TabIndex = 2;
        txtPassword.UseSystemPasswordChar = true;
        txtPassword.TextChanged += Campos_TextChanged;

        // lblConfirmar
        lblConfirmar.AutoSize = true;
        lblConfirmar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblConfirmar.ForeColor = Color.FromArgb(15, 23, 42);
        lblConfirmar.Location = new Point(28, 234);
        lblConfirmar.Name = "lblConfirmar";
        lblConfirmar.Text = "Confirmar Contraseña:";

        // txtConfirmarPassword
        txtConfirmarPassword.AccessibleDescription = "Repita la contraseña ingresada";
        txtConfirmarPassword.AccessibleName = "Confirmar Contraseña";
        txtConfirmarPassword.BorderStyle = BorderStyle.FixedSingle;
        txtConfirmarPassword.Font = new Font("Segoe UI", 10F);
        txtConfirmarPassword.Location = new Point(28, 254);
        txtConfirmarPassword.Name = "txtConfirmarPassword";
        txtConfirmarPassword.PasswordChar = '*';
        txtConfirmarPassword.Size = new Size(364, 25);
        txtConfirmarPassword.TabIndex = 3;
        txtConfirmarPassword.UseSystemPasswordChar = true;
        txtConfirmarPassword.TextChanged += Campos_TextChanged;

        // lblRol
        lblRol.AutoSize = true;
        lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblRol.ForeColor = Color.FromArgb(15, 23, 42);
        lblRol.Location = new Point(28, 290);
        lblRol.Name = "lblRol";
        lblRol.Text = "Rol del Usuario:";

        // cmbRol
        cmbRol.AccessibleName = "Rol del Usuario";
        cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRol.Font = new Font("Segoe UI", 10F);
        cmbRol.Items.AddRange(new object[] { "PRODUCTOR", "OPERADOR", "ADMIN" });
        cmbRol.Location = new Point(28, 310);
        cmbRol.Name = "cmbRol";
        cmbRol.SelectedIndex = 0;
        cmbRol.Size = new Size(364, 25);
        cmbRol.TabIndex = 4;

        // btnGuardar
        btnGuardar.AccessibleName = "Crear Usuario";
        btnGuardar.BackColor = Color.FromArgb(21, 128, 61);
        btnGuardar.Cursor = Cursors.Hand;
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnGuardar.ForeColor = Color.White;
        btnGuardar.Location = new Point(28, 355);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(176, 38);
        btnGuardar.TabIndex = 5;
        btnGuardar.Text = "  Crear Usuario";
        btnGuardar.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnGuardar.UseVisualStyleBackColor = false;
        btnGuardar.Click += btnGuardar_Click;

        // btnCancelar
        btnCancelar.AccessibleName = "Cancelar";
        btnCancelar.BackColor = Color.FromArgb(71, 85, 105);
        btnCancelar.Cursor = Cursors.Hand;
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCancelar.ForeColor = Color.White;
        btnCancelar.Location = new Point(216, 355);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(176, 38);
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
        BackColor = Color.FromArgb(248, 250, 252);
        ClientSize = new Size(420, 414);
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
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
