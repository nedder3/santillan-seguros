namespace FederacionSantillan.App.Views;

partial class FormLogin
{
    private System.ComponentModel.IContainer components = null;
    private Panel panelHeader;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Label lblUsuario;
    private TextBox txtUsuario;
    private Label lblPassword;
    private TextBox txtPassword;
    private CheckBox chkMostrarPassword;
    private Button btnIngresar;
    private Button btnCrearUsuario;
    private Button btnSalir;
    private Label lblEstadoDb;
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
        panelHeader = new Panel();
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        lblUsuario = new Label();
        txtUsuario = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        chkMostrarPassword = new CheckBox();
        btnIngresar = new Button();
        btnCrearUsuario = new Button();
        btnSalir = new Button();
        lblEstadoDb = new Label();
        errorProvider = new ErrorProvider(components);

        panelHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();

        // errorProvider
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;

        // panelHeader
        panelHeader.BackColor = Color.FromArgb(15, 76, 129);
        panelHeader.Controls.Add(lblSubtitulo);
        panelHeader.Controls.Add(lblTitulo);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Location = new Point(0, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new Size(440, 82);
        panelHeader.TabIndex = 0;

        // lblTitulo
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(20, 14);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(310, 25);
        lblTitulo.Text = "Federación Santillán Seguros";

        // lblSubtitulo
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        lblSubtitulo.ForeColor = Color.FromArgb(219, 234, 254);
        lblSubtitulo.Location = new Point(22, 44);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(290, 17);
        lblSubtitulo.Text = "Productor Asesor — Federación Patronal Seguros";

        // lblUsuario
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblUsuario.ForeColor = Color.FromArgb(15, 23, 42);
        lblUsuario.Location = new Point(35, 105);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(60, 17);
        lblUsuario.Text = "Usuario:";

        // txtUsuario
        txtUsuario.AccessibleDescription = "Ingrese su nombre de usuario asignado";
        txtUsuario.AccessibleName = "Usuario";
        txtUsuario.BorderStyle = BorderStyle.FixedSingle;
        txtUsuario.Font = new Font("Segoe UI", 10.5F);
        txtUsuario.Location = new Point(35, 126);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(368, 26);
        txtUsuario.TabIndex = 1;
        txtUsuario.TextChanged += Campos_TextChanged;

        // lblPassword
        lblPassword.AutoSize = true;
        lblPassword.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPassword.ForeColor = Color.FromArgb(15, 23, 42);
        lblPassword.Location = new Point(35, 165);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(85, 17);
        lblPassword.Text = "Contraseña:";

        // txtPassword
        txtPassword.AccessibleDescription = "Ingrese su contraseña";
        txtPassword.AccessibleName = "Contraseña";
        txtPassword.BorderStyle = BorderStyle.FixedSingle;
        txtPassword.Font = new Font("Segoe UI", 10.5F);
        txtPassword.Location = new Point(35, 186);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(368, 26);
        txtPassword.TabIndex = 2;
        txtPassword.UseSystemPasswordChar = true;
        txtPassword.TextChanged += Campos_TextChanged;

        // chkMostrarPassword
        chkMostrarPassword.AccessibleName = "Mostrar contraseña";
        chkMostrarPassword.AutoSize = true;
        chkMostrarPassword.Cursor = Cursors.Hand;
        chkMostrarPassword.Font = new Font("Segoe UI", 9F);
        chkMostrarPassword.ForeColor = Color.FromArgb(51, 65, 85);
        chkMostrarPassword.Location = new Point(35, 218);
        chkMostrarPassword.Name = "chkMostrarPassword";
        chkMostrarPassword.Size = new Size(140, 19);
        chkMostrarPassword.TabIndex = 3;
        chkMostrarPassword.Text = "Mostrar contraseña";
        chkMostrarPassword.UseVisualStyleBackColor = true;
        chkMostrarPassword.CheckedChanged += chkMostrarPassword_CheckedChanged;

        // btnIngresar
        btnIngresar.AccessibleName = "Ingresar al Sistema";
        btnIngresar.BackColor = Color.FromArgb(21, 128, 61);
        btnIngresar.Cursor = Cursors.Hand;
        btnIngresar.FlatAppearance.BorderSize = 0;
        btnIngresar.FlatStyle = FlatStyle.Flat;
        btnIngresar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnIngresar.ForeColor = Color.White;
        btnIngresar.Location = new Point(35, 252);
        btnIngresar.Name = "btnIngresar";
        btnIngresar.Size = new Size(368, 40);
        btnIngresar.TabIndex = 4;
        btnIngresar.Text = "  Ingresar al Sistema";
        btnIngresar.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnIngresar.UseVisualStyleBackColor = false;
        btnIngresar.Click += btnIngresar_Click;

        // btnCrearUsuario
        btnCrearUsuario.AccessibleName = "Nuevo Usuario";
        btnCrearUsuario.BackColor = Color.FromArgb(29, 78, 216);
        btnCrearUsuario.Cursor = Cursors.Hand;
        btnCrearUsuario.FlatAppearance.BorderSize = 0;
        btnCrearUsuario.FlatStyle = FlatStyle.Flat;
        btnCrearUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCrearUsuario.ForeColor = Color.White;
        btnCrearUsuario.Location = new Point(35, 302);
        btnCrearUsuario.Name = "btnCrearUsuario";
        btnCrearUsuario.Size = new Size(180, 36);
        btnCrearUsuario.TabIndex = 5;
        btnCrearUsuario.Text = "  Nuevo Usuario";
        btnCrearUsuario.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnCrearUsuario.UseVisualStyleBackColor = false;
        btnCrearUsuario.Click += btnCrearUsuario_Click;

        // btnSalir
        btnSalir.AccessibleName = "Salir";
        btnSalir.BackColor = Color.FromArgb(71, 85, 105);
        btnSalir.Cursor = Cursors.Hand;
        btnSalir.FlatAppearance.BorderSize = 0;
        btnSalir.FlatStyle = FlatStyle.Flat;
        btnSalir.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnSalir.ForeColor = Color.White;
        btnSalir.Location = new Point(223, 302);
        btnSalir.Name = "btnSalir";
        btnSalir.Size = new Size(180, 36);
        btnSalir.TabIndex = 6;
        btnSalir.Text = "  Salir";
        btnSalir.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnSalir.UseVisualStyleBackColor = false;
        btnSalir.Click += btnSalir_Click;

        // lblEstadoDb
        lblEstadoDb.AccessibleName = "Estado de conexión a la base de datos";
        lblEstadoDb.AutoSize = true;
        lblEstadoDb.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        lblEstadoDb.ForeColor = Color.FromArgb(71, 85, 105);
        lblEstadoDb.Location = new Point(35, 352);
        lblEstadoDb.Name = "lblEstadoDb";
        lblEstadoDb.Size = new Size(210, 15);
        lblEstadoDb.Text = "Verificando conexión con MariaDB...";

        // FormLogin
        AcceptButton = btnIngresar;
        CancelButton = btnSalir;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 250, 252);
        ClientSize = new Size(440, 388);
        Controls.Add(lblEstadoDb);
        Controls.Add(btnSalir);
        Controls.Add(btnCrearUsuario);
        Controls.Add(btnIngresar);
        Controls.Add(chkMostrarPassword);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtUsuario);
        Controls.Add(lblUsuario);
        Controls.Add(panelHeader);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Iniciar Sesión - Federación Santillán Seguros";
        Load += FormLogin_Load;

        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
