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

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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

        panelHeader.SuspendLayout();
        SuspendLayout();

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
        lblSubtitulo.ForeColor = Color.FromArgb(200, 225, 250);
        lblSubtitulo.Location = new Point(22, 44);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(290, 17);
        lblSubtitulo.Text = "Productor Asesor • Federación Patronal Seguros";

        // lblUsuario
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblUsuario.Location = new Point(35, 105);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(60, 17);
        lblUsuario.Text = "Usuario:";

        // txtUsuario
        txtUsuario.Font = new Font("Segoe UI", 10.5F);
        txtUsuario.Location = new Point(35, 126);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(368, 26);
        txtUsuario.TabIndex = 1;

        // lblPassword
        lblPassword.AutoSize = true;
        lblPassword.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPassword.Location = new Point(35, 165);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(85, 17);
        lblPassword.Text = "Contraseña:";

        // txtPassword (Enmascarada por defecto - Rúbrica)
        txtPassword.Font = new Font("Segoe UI", 10.5F);
        txtPassword.Location = new Point(35, 186);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(368, 26);
        txtPassword.TabIndex = 2;
        txtPassword.UseSystemPasswordChar = true;

        // chkMostrarPassword
        chkMostrarPassword.AutoSize = true;
        chkMostrarPassword.Font = new Font("Segoe UI", 8.5F);
        chkMostrarPassword.Location = new Point(35, 218);
        chkMostrarPassword.Name = "chkMostrarPassword";
        chkMostrarPassword.Size = new Size(135, 19);
        chkMostrarPassword.TabIndex = 3;
        chkMostrarPassword.Text = "Mostrar contraseña";
        chkMostrarPassword.UseVisualStyleBackColor = true;
        chkMostrarPassword.CheckedChanged += chkMostrarPassword_CheckedChanged;

        // btnIngresar
        btnIngresar.BackColor = Color.FromArgb(22, 163, 74);
        btnIngresar.FlatAppearance.BorderSize = 0;
        btnIngresar.FlatStyle = FlatStyle.Flat;
        btnIngresar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnIngresar.ForeColor = Color.White;
        btnIngresar.Location = new Point(35, 252);
        btnIngresar.Name = "btnIngresar";
        btnIngresar.Size = new Size(368, 38);
        btnIngresar.TabIndex = 4;
        btnIngresar.Text = "  Ingresar al Sistema";
        btnIngresar.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnIngresar.UseVisualStyleBackColor = false;
        btnIngresar.Click += btnIngresar_Click;

        // btnCrearUsuario
        btnCrearUsuario.BackColor = Color.FromArgb(37, 99, 235);
        btnCrearUsuario.FlatAppearance.BorderSize = 0;
        btnCrearUsuario.FlatStyle = FlatStyle.Flat;
        btnCrearUsuario.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCrearUsuario.ForeColor = Color.White;
        btnCrearUsuario.Location = new Point(35, 300);
        btnCrearUsuario.Name = "btnCrearUsuario";
        btnCrearUsuario.Size = new Size(180, 34);
        btnCrearUsuario.TabIndex = 5;
        btnCrearUsuario.Text = "  Nuevo Usuario";
        btnCrearUsuario.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnCrearUsuario.UseVisualStyleBackColor = false;
        btnCrearUsuario.Click += btnCrearUsuario_Click;

        // btnSalir
        btnSalir.BackColor = Color.FromArgb(100, 116, 139);
        btnSalir.FlatAppearance.BorderSize = 0;
        btnSalir.FlatStyle = FlatStyle.Flat;
        btnSalir.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnSalir.ForeColor = Color.White;
        btnSalir.Location = new Point(223, 300);
        btnSalir.Name = "btnSalir";
        btnSalir.Size = new Size(180, 34);
        btnSalir.TabIndex = 6;
        btnSalir.Text = "  Salir";
        btnSalir.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnSalir.UseVisualStyleBackColor = false;
        btnSalir.Click += btnSalir_Click;

        // lblEstadoDb
        lblEstadoDb.AutoSize = true;
        lblEstadoDb.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
        lblEstadoDb.ForeColor = Color.DimGray;
        lblEstadoDb.Location = new Point(35, 348);
        lblEstadoDb.Name = "lblEstadoDb";
        lblEstadoDb.Size = new Size(200, 15);
        lblEstadoDb.Text = "Verificando conexión MariaDB...";

        // FormLogin
        AcceptButton = btnIngresar;
        CancelButton = btnSalir;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(440, 380);
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
        ResumeLayout(false);
        PerformLayout();
    }
}
