namespace FederacionSantillan.App.Views;

partial class FormAyuda
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitulo;
    private RichTextBox rtbContenido;
    private Button btnCerrar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        rtbContenido = new RichTextBox();
        btnCerrar = new Button();
        SuspendLayout();

        lblTitulo.BackColor = Color.FromArgb(15, 76, 129);
        lblTitulo.Dock = DockStyle.Top;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Padding = new Padding(16, 0, 0, 0);
        lblTitulo.Size = new Size(640, 48);
        lblTitulo.Text = "Centro de Ayuda - Federación Santillán Seguros";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

        rtbContenido.BackColor = Color.White;
        rtbContenido.BorderStyle = BorderStyle.None;
        rtbContenido.Dock = DockStyle.Fill;
        rtbContenido.Font = new Font("Segoe UI", 10F);
        rtbContenido.Padding = new Padding(16);
        rtbContenido.ReadOnly = true;
        rtbContenido.TabIndex = 0;

        btnCerrar.BackColor = Color.FromArgb(37, 99, 235);
        btnCerrar.Dock = DockStyle.Bottom;
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.FlatStyle = FlatStyle.Flat;
        btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCerrar.ForeColor = Color.White;
        btnCerrar.Height = 42;
        btnCerrar.TabIndex = 1;
        btnCerrar.Text = "Cerrar Ayuda";
        btnCerrar.Click += btnCerrar_Click;

        AcceptButton = btnCerrar;
        CancelButton = btnCerrar;
        ClientSize = new Size(640, 460);
        Controls.Add(rtbContenido);
        Controls.Add(btnCerrar);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormAyuda";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Ayuda y Guía Rápida";
        Load += FormAyuda_Load;
        ResumeLayout(false);
    }
}
