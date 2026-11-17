namespace FederacionSantillan.App.Views;

partial class FormOpciones
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitulo;
    private GroupBox grpAccesibilidad;
    private Label lblModoVisual;
    private ComboBox cmbModoVisual;
    private CheckBox chkTextoAmpliado;
    private GroupBox grpBackup;
    private Label lblBackupDesc;
    private Button btnExportarDump;
    private Button btnRestablecer;
    private Button btnAplicarCerrar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        grpAccesibilidad = new GroupBox();
        lblModoVisual = new Label();
        cmbModoVisual = new ComboBox();
        chkTextoAmpliado = new CheckBox();
        grpBackup = new GroupBox();
        lblBackupDesc = new Label();
        btnExportarDump = new Button();
        btnRestablecer = new Button();
        btnAplicarCerrar = new Button();

        grpAccesibilidad.SuspendLayout();
        grpBackup.SuspendLayout();
        SuspendLayout();

        lblTitulo.BackColor = Color.FromArgb(15, 76, 129);
        lblTitulo.Dock = DockStyle.Top;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Padding = new Padding(16, 0, 0, 0);
        lblTitulo.Size = new Size(520, 48);
        lblTitulo.Text = "Opciones del Sistema, Accesibilidad y Respaldo SQL";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

        // GroupBox Accesibilidad (Ley de Proximidad - Gestalt)
        grpAccesibilidad.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpAccesibilidad.Location = new Point(20, 64);
        grpAccesibilidad.Size = new Size(480, 130);
        grpAccesibilidad.Text = "Accesibilidad Visual y Modo Daltonismo (WCAG 2.1 AA)";
        grpAccesibilidad.Controls.Add(chkTextoAmpliado);
        grpAccesibilidad.Controls.Add(cmbModoVisual);
        grpAccesibilidad.Controls.Add(lblModoVisual);

        lblModoVisual.AutoSize = true;
        lblModoVisual.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        lblModoVisual.Location = new Point(16, 30);
        lblModoVisual.Text = "Paleta de colores:";

        cmbModoVisual.AccessibleName = "Modo de accesibilidad visual";
        cmbModoVisual.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbModoVisual.Font = new Font("Segoe UI", 9.5F);
        cmbModoVisual.Items.AddRange(new object[]
        {
            "Estándar (WCAG AA)",
            "Modo Daltonismo (Protanopia / Deuteranopia - Azul/Naranja)",
            "Alto Contraste (Baja Visión WCAG AAA)"
        });
        cmbModoVisual.Location = new Point(16, 52);
        cmbModoVisual.Size = new Size(444, 24);
        cmbModoVisual.TabIndex = 0;

        chkTextoAmpliado.AccessibleName = "Activar texto ampliado";
        chkTextoAmpliado.AutoSize = true;
        chkTextoAmpliado.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        chkTextoAmpliado.Location = new Point(16, 90);
        chkTextoAmpliado.TabIndex = 1;
        chkTextoAmpliado.Text = "Texto ampliado (11pt) para lectura descansada";

        // GroupBox Respaldo DUMP SQL
        grpBackup.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpBackup.Location = new Point(20, 206);
        grpBackup.Size = new Size(480, 125);
        grpBackup.Text = "Respaldo de Base de Datos (DUMP SQL MariaDB)";
        grpBackup.Controls.Add(btnExportarDump);
        grpBackup.Controls.Add(lblBackupDesc);

        lblBackupDesc.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        lblBackupDesc.Location = new Point(16, 26);
        lblBackupDesc.Size = new Size(444, 38);
        lblBackupDesc.Text = "Genera un archivo .sql completo con estructura, claves foráneas y datos actuales listo para importar en DBeaver.";

        btnExportarDump.AccessibleName = "Exportar DUMP SQL de MariaDB";
        btnExportarDump.BackColor = Color.FromArgb(29, 78, 216);
        btnExportarDump.FlatAppearance.BorderSize = 0;
        btnExportarDump.FlatStyle = FlatStyle.Flat;
        btnExportarDump.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnExportarDump.ForeColor = Color.White;
        btnExportarDump.Location = new Point(16, 72);
        btnExportarDump.Size = new Size(444, 38);
        btnExportarDump.TabIndex = 2;
        btnExportarDump.Text = "Exportar DUMP SQL (.sql)";
        btnExportarDump.Click += btnExportarDump_Click;

        // Botones inferiores (Fitts's Law: altura 38px)
        btnRestablecer.AccessibleName = "Restablecer valores por defecto";
        btnRestablecer.BackColor = Color.FromArgb(100, 116, 139);
        btnRestablecer.FlatAppearance.BorderSize = 0;
        btnRestablecer.FlatStyle = FlatStyle.Flat;
        btnRestablecer.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnRestablecer.ForeColor = Color.White;
        btnRestablecer.Location = new Point(20, 350);
        btnRestablecer.Size = new Size(210, 38);
        btnRestablecer.TabIndex = 3;
        btnRestablecer.Text = "Restablecer Defecto";
        btnRestablecer.Click += btnRestablecer_Click;

        btnAplicarCerrar.AccessibleName = "Aplicar y cerrar opciones";
        btnAplicarCerrar.BackColor = Color.FromArgb(21, 128, 61);
        btnAplicarCerrar.FlatAppearance.BorderSize = 0;
        btnAplicarCerrar.FlatStyle = FlatStyle.Flat;
        btnAplicarCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAplicarCerrar.ForeColor = Color.White;
        btnAplicarCerrar.Location = new Point(250, 350);
        btnAplicarCerrar.Size = new Size(250, 38);
        btnAplicarCerrar.TabIndex = 4;
        btnAplicarCerrar.Text = "Aplicar y Cerrar";
        btnAplicarCerrar.Click += btnAplicarCerrar_Click;

        AcceptButton = btnAplicarCerrar;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(520, 408);
        Controls.Add(btnAplicarCerrar);
        Controls.Add(btnRestablecer);
        Controls.Add(grpBackup);
        Controls.Add(grpAccesibilidad);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormOpciones";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Opciones Avanzadas";
        Load += FormOpciones_Load;

        grpAccesibilidad.ResumeLayout(false);
        grpAccesibilidad.PerformLayout();
        grpBackup.ResumeLayout(false);
        ResumeLayout(false);
    }
}
