namespace FederacionSantillan.App.Views;

partial class FormPrincipal
{
    private System.ComponentModel.IContainer components = null;
    private Panel panelTop;
    private Label lblBrand;
    private Label lblUsuarioActivo;
    private Button btnOpciones;
    private Button btnAyuda;
    private Button btnCerrarSesion;
    private TabControl tabMain;
    private TabPage tabAlertas;
    private TabPage tabAsegurados;
    private TabPage tabPolizas;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel toolStripStatusLabel;

    // Tab 1: Alertas
    private Panel panelKpis;
    private Panel cardVencidas;
    private Label lblKpiVencidasTitulo;
    private Label lblKpiVencidasValor;
    private Panel cardPorVencer;
    private Label lblKpiPorVencerTitulo;
    private Label lblKpiPorVencerValor;
    private ComboBox cmbMetodoPagoAlerta;
    private Label lblMetodoPagoAlerta;
    private ComboBox cmbDiasAnticipacion;
    private Button btnRefrescarAlertas;
    private Button btnWhatsAppAviso;
    private Button btnExportarCsv;
    private GroupBox grpVencidas;
    private DataGridView dgvVencidas;
    private Button btnPagarVencida;
    private GroupBox grpPorVencer;
    private DataGridView dgvPorVencer;
    private Button btnPagarPorVencer;

    // Tab 2: Asegurados
    private GroupBox grpFormAsegurado;
    private Label lblAsegDni;
    private TextBox txtAsegDni;
    private Label lblAsegNombre;
    private TextBox txtAsegNombre;
    private Label lblAsegTel;
    private TextBox txtAsegTel;
    private Label lblAsegEmail;
    private TextBox txtAsegEmail;
    private Label lblAsegDom;
    private TextBox txtAsegDom;
    private Button btnAsegGuardar;
    private Button btnAsegNuevo;
    private Button btnAsegEliminar;
    private Label lblBuscarAseg;
    private TextBox txtBuscarAseg;
    private DataGridView dgvAsegurados;

    // Tab 3: Pólizas y Cuotas
    private GroupBox grpNuevaPoliza;
    private Label lblPolNum;
    private TextBox txtPolNum;
    private Label lblPolAseg;
    private ComboBox cmbPolAsegurado;
    private Label lblPolRamo;
    private ComboBox cmbPolRamo;
    private Label lblPolBien;
    private TextBox txtPolBien;
    private Label lblPolPrima;
    private TextBox txtPolPrima;
    private Label lblPolCuotas;
    private NumericUpDown nudPolCuotas;
    private Label lblPolPrimerVto;
    private DateTimePicker dtpPolPrimerVto;
    private Button btnEmitirPoliza;
    private GroupBox grpListaPolizas;
    private DataGridView dgvPolizas;
    private GroupBox grpCuotasPoliza;
    private DataGridView dgvCuotasPoliza;
    private Label lblResumenCuota;
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
        panelTop = new Panel();
        lblBrand = new Label();
        lblUsuarioActivo = new Label();
        btnOpciones = new Button();
        btnAyuda = new Button();
        btnCerrarSesion = new Button();
        tabMain = new TabControl();
        tabAlertas = new TabPage();
        tabAsegurados = new TabPage();
        tabPolizas = new TabPage();
        statusStrip = new StatusStrip();
        toolStripStatusLabel = new ToolStripStatusLabel();
        errorProvider = new ErrorProvider(components);
        lblResumenCuota = new Label();

        // Tab 1 controls
        panelKpis = new Panel();
        cardVencidas = new Panel();
        lblKpiVencidasTitulo = new Label();
        lblKpiVencidasValor = new Label();
        cardPorVencer = new Panel();
        lblKpiPorVencerTitulo = new Label();
        lblKpiPorVencerValor = new Label();
        lblMetodoPagoAlerta = new Label();
        cmbMetodoPagoAlerta = new ComboBox();
        cmbDiasAnticipacion = new ComboBox();
        btnRefrescarAlertas = new Button();
        btnWhatsAppAviso = new Button();
        btnExportarCsv = new Button();
        grpVencidas = new GroupBox();
        dgvVencidas = new DataGridView();
        btnPagarVencida = new Button();
        grpPorVencer = new GroupBox();
        dgvPorVencer = new DataGridView();
        btnPagarPorVencer = new Button();

        // Tab 2 controls
        grpFormAsegurado = new GroupBox();
        lblAsegDni = new Label();
        txtAsegDni = new TextBox();
        lblAsegNombre = new Label();
        txtAsegNombre = new TextBox();
        lblAsegTel = new Label();
        txtAsegTel = new TextBox();
        lblAsegEmail = new Label();
        txtAsegEmail = new TextBox();
        lblAsegDom = new Label();
        txtAsegDom = new TextBox();
        btnAsegGuardar = new Button();
        btnAsegNuevo = new Button();
        btnAsegEliminar = new Button();
        lblBuscarAseg = new Label();
        txtBuscarAseg = new TextBox();
        dgvAsegurados = new DataGridView();

        // Tab 3 controls
        grpNuevaPoliza = new GroupBox();
        lblPolNum = new Label();
        txtPolNum = new TextBox();
        lblPolAseg = new Label();
        cmbPolAsegurado = new ComboBox();
        lblPolRamo = new Label();
        cmbPolRamo = new ComboBox();
        lblPolBien = new Label();
        txtPolBien = new TextBox();
        lblPolPrima = new Label();
        txtPolPrima = new TextBox();
        lblPolCuotas = new Label();
        nudPolCuotas = new NumericUpDown();
        lblPolPrimerVto = new Label();
        dtpPolPrimerVto = new DateTimePicker();
        btnEmitirPoliza = new Button();
        grpListaPolizas = new GroupBox();
        dgvPolizas = new DataGridView();
        grpCuotasPoliza = new GroupBox();
        dgvCuotasPoliza = new DataGridView();

        panelTop.SuspendLayout();
        tabMain.SuspendLayout();
        tabAlertas.SuspendLayout();
        tabAsegurados.SuspendLayout();
        tabPolizas.SuspendLayout();
        panelKpis.SuspendLayout();
        cardVencidas.SuspendLayout();
        cardPorVencer.SuspendLayout();
        grpVencidas.SuspendLayout();
        grpPorVencer.SuspendLayout();
        grpFormAsegurado.SuspendLayout();
        grpNuevaPoliza.SuspendLayout();
        grpListaPolizas.SuspendLayout();
        grpCuotasPoliza.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVencidas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPorVencer).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvAsegurados).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPolizas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvCuotasPoliza).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudPolCuotas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();

        // errorProvider
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;

        // panelTop
        panelTop.BackColor = Color.FromArgb(15, 76, 129);
        panelTop.Controls.Add(btnOpciones);
        panelTop.Controls.Add(btnAyuda);
        panelTop.Controls.Add(btnCerrarSesion);
        panelTop.Controls.Add(lblUsuarioActivo);
        panelTop.Controls.Add(lblBrand);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(1120, 56);

        lblBrand.AutoSize = true;
        lblBrand.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblBrand.ForeColor = Color.White;
        lblBrand.Location = new Point(16, 16);
        lblBrand.Name = "lblBrand";
        lblBrand.Text = "Federación Santillán Seguros — Panel de Cartera";

        lblUsuarioActivo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblUsuarioActivo.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        lblUsuarioActivo.ForeColor = Color.FromArgb(219, 234, 254);
        lblUsuarioActivo.Location = new Point(440, 19);
        lblUsuarioActivo.Name = "lblUsuarioActivo";
        lblUsuarioActivo.Size = new Size(320, 20);
        lblUsuarioActivo.TextAlign = ContentAlignment.MiddleRight;
        lblUsuarioActivo.Text = "Usuario: --";

        btnOpciones.AccessibleName = "Opciones";
        btnOpciones.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnOpciones.BackColor = Color.FromArgb(51, 65, 85);
        btnOpciones.Cursor = Cursors.Hand;
        btnOpciones.FlatAppearance.BorderSize = 0;
        btnOpciones.FlatStyle = FlatStyle.Flat;
        btnOpciones.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnOpciones.ForeColor = Color.White;
        btnOpciones.Location = new Point(770, 12);
        btnOpciones.Name = "btnOpciones";
        btnOpciones.Size = new Size(96, 32);
        btnOpciones.TabIndex = 0;
        btnOpciones.Text = "Opciones";
        btnOpciones.Click += btnOpciones_Click;

        btnAyuda.AccessibleName = "Ayuda";
        btnAyuda.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAyuda.BackColor = Color.FromArgb(30, 64, 175);
        btnAyuda.Cursor = Cursors.Hand;
        btnAyuda.FlatAppearance.BorderSize = 0;
        btnAyuda.FlatStyle = FlatStyle.Flat;
        btnAyuda.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnAyuda.ForeColor = Color.White;
        btnAyuda.Location = new Point(874, 12);
        btnAyuda.Name = "btnAyuda";
        btnAyuda.Size = new Size(96, 32);
        btnAyuda.TabIndex = 0;
        btnAyuda.Text = "Ayuda (?)";
        btnAyuda.Click += btnAyuda_Click;

        btnCerrarSesion.AccessibleName = "Cerrar Sesión";
        btnCerrarSesion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCerrarSesion.BackColor = Color.FromArgb(185, 28, 28);
        btnCerrarSesion.Cursor = Cursors.Hand;
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnCerrarSesion.ForeColor = Color.White;
        btnCerrarSesion.Location = new Point(980, 12);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Size = new Size(125, 32);
        btnCerrarSesion.TabIndex = 0;
        btnCerrarSesion.Text = "  Cerrar Sesión";
        btnCerrarSesion.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnCerrarSesion.Click += btnCerrarSesion_Click;

        // tabMain
        tabMain.Controls.Add(tabAlertas);
        tabMain.Controls.Add(tabAsegurados);
        tabMain.Controls.Add(tabPolizas);
        tabMain.Dock = DockStyle.Fill;
        tabMain.Font = new Font("Segoe UI", 9.5F);
        tabMain.Location = new Point(0, 56);
        tabMain.Name = "tabMain";
        tabMain.SelectedIndex = 0;
        tabMain.Size = new Size(1120, 622);
        tabMain.TabIndex = 1;

        // ==================== TAB 1: ALERTAS ====================
        tabAlertas.BackColor = Color.FromArgb(248, 250, 252);
        tabAlertas.Controls.Add(grpPorVencer);
        tabAlertas.Controls.Add(grpVencidas);
        tabAlertas.Controls.Add(panelKpis);
        tabAlertas.Name = "tabAlertas";
        tabAlertas.Padding = new Padding(10);
        tabAlertas.Text = "Notificaciones de Vencimientos";

        panelKpis.Dock = DockStyle.Top;
        panelKpis.Height = 86;
        panelKpis.Controls.Add(btnExportarCsv);
        panelKpis.Controls.Add(btnWhatsAppAviso);
        panelKpis.Controls.Add(btnRefrescarAlertas);
        panelKpis.Controls.Add(cmbDiasAnticipacion);
        panelKpis.Controls.Add(cmbMetodoPagoAlerta);
        panelKpis.Controls.Add(lblMetodoPagoAlerta);
        panelKpis.Controls.Add(cardPorVencer);
        panelKpis.Controls.Add(cardVencidas);

        cardVencidas.BackColor = Color.FromArgb(254, 226, 226);
        cardVencidas.BorderStyle = BorderStyle.FixedSingle;
        cardVencidas.Controls.Add(lblKpiVencidasValor);
        cardVencidas.Controls.Add(lblKpiVencidasTitulo);
        cardVencidas.Location = new Point(4, 6);
        cardVencidas.Size = new Size(270, 72);

        lblKpiVencidasTitulo.AutoSize = true;
        lblKpiVencidasTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblKpiVencidasTitulo.ForeColor = Color.FromArgb(153, 27, 27);
        lblKpiVencidasTitulo.Location = new Point(10, 8);
        lblKpiVencidasTitulo.Text = "CUOTAS VENCIDAS (MOROSOS)";

        lblKpiVencidasValor.AutoSize = true;
        lblKpiVencidasValor.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblKpiVencidasValor.ForeColor = Color.FromArgb(185, 28, 28);
        lblKpiVencidasValor.Location = new Point(10, 32);
        lblKpiVencidasValor.Text = "0 cuotas ($0)";

        cardPorVencer.BackColor = Color.FromArgb(254, 243, 199);
        cardPorVencer.BorderStyle = BorderStyle.FixedSingle;
        cardPorVencer.Controls.Add(lblKpiPorVencerValor);
        cardPorVencer.Controls.Add(lblKpiPorVencerTitulo);
        cardPorVencer.Location = new Point(286, 6);
        cardPorVencer.Size = new Size(270, 72);

        lblKpiPorVencerTitulo.AutoSize = true;
        lblKpiPorVencerTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblKpiPorVencerTitulo.ForeColor = Color.FromArgb(146, 64, 14);
        lblKpiPorVencerTitulo.Location = new Point(10, 8);
        lblKpiPorVencerTitulo.Text = "POR VENCER (PRÓXIMOS 15 DÍAS)";

        lblKpiPorVencerValor.AutoSize = true;
        lblKpiPorVencerValor.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblKpiPorVencerValor.ForeColor = Color.FromArgb(180, 83, 9);
        lblKpiPorVencerValor.Location = new Point(10, 32);
        lblKpiPorVencerValor.Text = "0 cuotas ($0)";

        lblMetodoPagoAlerta.AutoSize = true;
        lblMetodoPagoAlerta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMetodoPagoAlerta.Location = new Point(580, 14);
        lblMetodoPagoAlerta.Text = "Medio de pago para imputar:";

        cmbMetodoPagoAlerta.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMetodoPagoAlerta.Items.AddRange(new object[] { "Transferencia", "Efectivo", "Débito Automático", "MercadoPago" });
        cmbMetodoPagoAlerta.Location = new Point(568, 36);
        cmbMetodoPagoAlerta.SelectedIndex = 0;
        cmbMetodoPagoAlerta.Size = new Size(155, 24);
        cmbMetodoPagoAlerta.TabIndex = 0;

        cmbDiasAnticipacion.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDiasAnticipacion.Items.AddRange(new object[] { "7 días", "15 días", "30 días" });
        cmbDiasAnticipacion.Location = new Point(732, 36);
        cmbDiasAnticipacion.SelectedIndex = 1;
        cmbDiasAnticipacion.Size = new Size(82, 24);
        cmbDiasAnticipacion.TabIndex = 1;
        cmbDiasAnticipacion.SelectedIndexChanged += btnRefrescarAlertas_Click;

        btnRefrescarAlertas.AccessibleName = "Actualizar Alertas";
        btnRefrescarAlertas.BackColor = Color.FromArgb(29, 78, 216);
        btnRefrescarAlertas.Cursor = Cursors.Hand;
        btnRefrescarAlertas.FlatAppearance.BorderSize = 0;
        btnRefrescarAlertas.FlatStyle = FlatStyle.Flat;
        btnRefrescarAlertas.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnRefrescarAlertas.ForeColor = Color.White;
        btnRefrescarAlertas.Location = new Point(822, 8);
        btnRefrescarAlertas.Size = new Size(135, 30);
        btnRefrescarAlertas.TabIndex = 2;
        btnRefrescarAlertas.Text = "  Actualizar";
        btnRefrescarAlertas.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnRefrescarAlertas.Click += btnRefrescarAlertas_Click;

        btnWhatsAppAviso.AccessibleName = "Copiar Aviso WhatsApp";
        btnWhatsAppAviso.BackColor = Color.FromArgb(21, 128, 61);
        btnWhatsAppAviso.Cursor = Cursors.Hand;
        btnWhatsAppAviso.FlatAppearance.BorderSize = 0;
        btnWhatsAppAviso.FlatStyle = FlatStyle.Flat;
        btnWhatsAppAviso.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnWhatsAppAviso.ForeColor = Color.White;
        btnWhatsAppAviso.Location = new Point(822, 44);
        btnWhatsAppAviso.Size = new Size(135, 30);
        btnWhatsAppAviso.TabIndex = 3;
        btnWhatsAppAviso.Text = "Aviso WhatsApp";
        btnWhatsAppAviso.Click += btnWhatsAppAviso_Click;

        btnExportarCsv.AccessibleName = "Exportar Morosos CSV";
        btnExportarCsv.BackColor = Color.FromArgb(71, 85, 105);
        btnExportarCsv.Cursor = Cursors.Hand;
        btnExportarCsv.FlatAppearance.BorderSize = 0;
        btnExportarCsv.FlatStyle = FlatStyle.Flat;
        btnExportarCsv.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnExportarCsv.ForeColor = Color.White;
        btnExportarCsv.Location = new Point(964, 44);
        btnExportarCsv.Size = new Size(125, 30);
        btnExportarCsv.TabIndex = 4;
        btnExportarCsv.Text = "Exportar CSV";
        btnExportarCsv.Click += btnExportarCsv_Click;

        // Grilla Vencidas
        grpVencidas.Dock = DockStyle.Top;
        grpVencidas.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpVencidas.ForeColor = Color.FromArgb(153, 27, 27);
        grpVencidas.Height = 245;
        grpVencidas.Text = "Morosos / Cuotas Vencidas (Requieren gestión inmediata)";
        grpVencidas.Controls.Add(dgvVencidas);
        grpVencidas.Controls.Add(btnPagarVencida);

        btnPagarVencida.AccessibleName = "Registrar pago de cuota vencida seleccionada";
        btnPagarVencida.BackColor = Color.FromArgb(21, 128, 61);
        btnPagarVencida.Cursor = Cursors.Hand;
        btnPagarVencida.Dock = DockStyle.Right;
        btnPagarVencida.FlatAppearance.BorderSize = 0;
        btnPagarVencida.FlatStyle = FlatStyle.Flat;
        btnPagarVencida.ForeColor = Color.White;
        btnPagarVencida.Size = new Size(155, 215);
        btnPagarVencida.TabIndex = 2;
        btnPagarVencida.Text = "Registrar Pago\n(Seleccionada)";
        btnPagarVencida.Click += btnPagarVencida_Click;

        dgvVencidas.AccessibleName = "Grilla de cuotas vencidas";
        dgvVencidas.AllowUserToAddRows = false;
        dgvVencidas.AllowUserToDeleteRows = false;
        dgvVencidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvVencidas.BackgroundColor = Color.White;
        dgvVencidas.Dock = DockStyle.Fill;
        dgvVencidas.MultiSelect = false;
        dgvVencidas.ReadOnly = true;
        dgvVencidas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // Grilla Por Vencer
        grpPorVencer.Dock = DockStyle.Fill;
        grpPorVencer.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpPorVencer.ForeColor = Color.FromArgb(146, 64, 14);
        grpPorVencer.Text = "Cuotas Próximas a Vencer (Próximos 15 días)";
        grpPorVencer.Controls.Add(dgvPorVencer);
        grpPorVencer.Controls.Add(btnPagarPorVencer);

        btnPagarPorVencer.AccessibleName = "Registrar pago de cuota próxima a vencer seleccionada";
        btnPagarPorVencer.BackColor = Color.FromArgb(21, 128, 61);
        btnPagarPorVencer.Cursor = Cursors.Hand;
        btnPagarPorVencer.Dock = DockStyle.Right;
        btnPagarPorVencer.FlatAppearance.BorderSize = 0;
        btnPagarPorVencer.FlatStyle = FlatStyle.Flat;
        btnPagarPorVencer.ForeColor = Color.White;
        btnPagarPorVencer.Size = new Size(155, 215);
        btnPagarPorVencer.TabIndex = 3;
        btnPagarPorVencer.Text = "Registrar Pago\n(Seleccionada)";
        btnPagarPorVencer.Click += btnPagarPorVencer_Click;

        dgvPorVencer.AccessibleName = "Grilla de cuotas próximas a vencer";

        dgvPorVencer.AllowUserToAddRows = false;
        dgvPorVencer.AllowUserToDeleteRows = false;
        dgvPorVencer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPorVencer.BackgroundColor = Color.White;
        dgvPorVencer.Dock = DockStyle.Fill;
        dgvPorVencer.MultiSelect = false;
        dgvPorVencer.ReadOnly = true;
        dgvPorVencer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // ==================== TAB 2: ASEGURADOS ====================
        tabAsegurados.BackColor = Color.FromArgb(248, 250, 252);
        tabAsegurados.Controls.Add(dgvAsegurados);
        tabAsegurados.Controls.Add(txtBuscarAseg);
        tabAsegurados.Controls.Add(lblBuscarAseg);
        tabAsegurados.Controls.Add(grpFormAsegurado);
        tabAsegurados.Name = "tabAsegurados";
        tabAsegurados.Padding = new Padding(10);
        tabAsegurados.Text = "Cartera de Asegurados";

        grpFormAsegurado.Dock = DockStyle.Left;
        grpFormAsegurado.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpFormAsegurado.Text = "Datos del Asegurado";
        grpFormAsegurado.Width = 330;
        grpFormAsegurado.Controls.Add(btnAsegEliminar);
        grpFormAsegurado.Controls.Add(btnAsegNuevo);
        grpFormAsegurado.Controls.Add(btnAsegGuardar);
        grpFormAsegurado.Controls.Add(txtAsegDom);
        grpFormAsegurado.Controls.Add(lblAsegDom);
        grpFormAsegurado.Controls.Add(txtAsegEmail);
        grpFormAsegurado.Controls.Add(lblAsegEmail);
        grpFormAsegurado.Controls.Add(txtAsegTel);
        grpFormAsegurado.Controls.Add(lblAsegTel);
        grpFormAsegurado.Controls.Add(txtAsegNombre);
        grpFormAsegurado.Controls.Add(lblAsegNombre);
        grpFormAsegurado.Controls.Add(txtAsegDni);
        grpFormAsegurado.Controls.Add(lblAsegDni);

        lblAsegDni.AutoSize = true; lblAsegDni.Location = new Point(16, 32); lblAsegDni.Text = "DNI / CUIT:";
        txtAsegDni.Font = new Font("Segoe UI", 9.5F); txtAsegDni.Location = new Point(16, 52); txtAsegDni.Size = new Size(295, 24); txtAsegDni.TabIndex = 0;

        lblAsegNombre.AutoSize = true; lblAsegNombre.Location = new Point(16, 86); lblAsegNombre.Text = "Apellido y Nombre / Razón Social:";
        txtAsegNombre.Font = new Font("Segoe UI", 9.5F); txtAsegNombre.Location = new Point(16, 106); txtAsegNombre.Size = new Size(295, 24); txtAsegNombre.TabIndex = 1;

        lblAsegTel.AutoSize = true; lblAsegTel.Location = new Point(16, 140); lblAsegTel.Text = "Teléfono / WhatsApp:";
        txtAsegTel.Font = new Font("Segoe UI", 9.5F); txtAsegTel.Location = new Point(16, 160); txtAsegTel.Size = new Size(295, 24); txtAsegTel.TabIndex = 2;

        lblAsegEmail.AutoSize = true; lblAsegEmail.Location = new Point(16, 194); lblAsegEmail.Text = "Email:";
        txtAsegEmail.Font = new Font("Segoe UI", 9.5F); txtAsegEmail.Location = new Point(16, 214); txtAsegEmail.Size = new Size(295, 24); txtAsegEmail.TabIndex = 3;

        lblAsegDom.AutoSize = true; lblAsegDom.Location = new Point(16, 248); lblAsegDom.Text = "Domicilio:";
        txtAsegDom.Font = new Font("Segoe UI", 9.5F); txtAsegDom.Location = new Point(16, 268); txtAsegDom.Size = new Size(295, 24); txtAsegDom.TabIndex = 4;

        btnAsegGuardar.AccessibleName = "Guardar Asegurado";
        btnAsegGuardar.BackColor = Color.FromArgb(21, 128, 61); btnAsegGuardar.Cursor = Cursors.Hand; btnAsegGuardar.FlatStyle = FlatStyle.Flat; btnAsegGuardar.FlatAppearance.BorderSize = 0;
        btnAsegGuardar.ForeColor = Color.White; btnAsegGuardar.Location = new Point(16, 315); btnAsegGuardar.Size = new Size(295, 36); btnAsegGuardar.TabIndex = 5;
        btnAsegGuardar.Text = "  Guardar Asegurado"; btnAsegGuardar.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnAsegGuardar.Click += btnAsegGuardar_Click;

        btnAsegNuevo.AccessibleName = "Limpiar formulario para nuevo asegurado";
        btnAsegNuevo.BackColor = Color.FromArgb(29, 78, 216); btnAsegNuevo.Cursor = Cursors.Hand; btnAsegNuevo.FlatStyle = FlatStyle.Flat; btnAsegNuevo.FlatAppearance.BorderSize = 0;
        btnAsegNuevo.ForeColor = Color.White; btnAsegNuevo.Location = new Point(16, 360); btnAsegNuevo.Size = new Size(142, 34); btnAsegNuevo.TabIndex = 6;
        btnAsegNuevo.Text = "Limpiar / Nuevo";
        btnAsegNuevo.Click += btnAsegNuevo_Click;

        btnAsegEliminar.AccessibleName = "Dar de baja asegurado seleccionado";
        btnAsegEliminar.BackColor = Color.FromArgb(185, 28, 28); btnAsegEliminar.Cursor = Cursors.Hand; btnAsegEliminar.FlatStyle = FlatStyle.Flat; btnAsegEliminar.FlatAppearance.BorderSize = 0;
        btnAsegEliminar.ForeColor = Color.White; btnAsegEliminar.Location = new Point(169, 360); btnAsegEliminar.Size = new Size(142, 34); btnAsegEliminar.TabIndex = 7;
        btnAsegEliminar.Text = "Dar de Baja";
        btnAsegEliminar.Click += btnAsegEliminar_Click;

        lblBuscarAseg.AutoSize = true;
        lblBuscarAseg.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblBuscarAseg.Location = new Point(352, 18);
        lblBuscarAseg.Text = "Buscar por Nombre o DNI:";

        txtBuscarAseg.AccessibleName = "Buscar asegurado por Nombre o DNI";
        txtBuscarAseg.Location = new Point(530, 14);
        txtBuscarAseg.Size = new Size(340, 24);
        txtBuscarAseg.TabIndex = 8;
        txtBuscarAseg.TextChanged += txtBuscarAseg_TextChanged;

        dgvAsegurados.AccessibleName = "Grilla de asegurados";

        dgvAsegurados.AllowUserToAddRows = false;
        dgvAsegurados.AllowUserToDeleteRows = false;
        dgvAsegurados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvAsegurados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAsegurados.BackgroundColor = Color.White;
        dgvAsegurados.Location = new Point(352, 48);
        dgvAsegurados.MultiSelect = false;
        dgvAsegurados.ReadOnly = true;
        dgvAsegurados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAsegurados.Size = new Size(748, 520);
        dgvAsegurados.SelectionChanged += dgvAsegurados_SelectionChanged;

        // ==================== TAB 3: PÓLIZAS Y CUOTAS ====================
        tabPolizas.BackColor = Color.FromArgb(248, 250, 252);
        tabPolizas.Controls.Add(grpCuotasPoliza);
        tabPolizas.Controls.Add(grpListaPolizas);
        tabPolizas.Controls.Add(grpNuevaPoliza);
        tabPolizas.Name = "tabPolizas";
        tabPolizas.Padding = new Padding(10);
        tabPolizas.Text = "Pólizas y Plan de Cuotas";

        grpNuevaPoliza.Dock = DockStyle.Top;
        grpNuevaPoliza.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpNuevaPoliza.Height = 140;
        grpNuevaPoliza.Text = "Emitir Nueva Póliza (Genera Plan de Cuotas Automático)";
        grpNuevaPoliza.Controls.Add(lblResumenCuota);
        grpNuevaPoliza.Controls.Add(btnEmitirPoliza);
        grpNuevaPoliza.Controls.Add(dtpPolPrimerVto);
        grpNuevaPoliza.Controls.Add(lblPolPrimerVto);
        grpNuevaPoliza.Controls.Add(nudPolCuotas);
        grpNuevaPoliza.Controls.Add(lblPolCuotas);
        grpNuevaPoliza.Controls.Add(txtPolPrima);
        grpNuevaPoliza.Controls.Add(lblPolPrima);
        grpNuevaPoliza.Controls.Add(txtPolBien);
        grpNuevaPoliza.Controls.Add(lblPolBien);
        grpNuevaPoliza.Controls.Add(cmbPolRamo);
        grpNuevaPoliza.Controls.Add(lblPolRamo);
        grpNuevaPoliza.Controls.Add(cmbPolAsegurado);
        grpNuevaPoliza.Controls.Add(lblPolAseg);
        grpNuevaPoliza.Controls.Add(txtPolNum);
        grpNuevaPoliza.Controls.Add(lblPolNum);

        lblPolNum.AutoSize = true; lblPolNum.Location = new Point(14, 24); lblPolNum.Text = "N° Póliza:";
        txtPolNum.Location = new Point(14, 42); txtPolNum.Size = new Size(100, 23); txtPolNum.TabIndex = 0;

        lblPolAseg.AutoSize = true; lblPolAseg.Location = new Point(126, 24); lblPolAseg.Text = "Asegurado:";
        cmbPolAsegurado.DropDownStyle = ComboBoxStyle.DropDownList; cmbPolAsegurado.Location = new Point(126, 42); cmbPolAsegurado.Size = new Size(250, 23); cmbPolAsegurado.TabIndex = 1;

        lblPolRamo.AutoSize = true; lblPolRamo.Location = new Point(388, 24); lblPolRamo.Text = "Ramo:";
        cmbPolRamo.DropDownStyle = ComboBoxStyle.DropDownList; cmbPolRamo.Location = new Point(388, 42); cmbPolRamo.Size = new Size(180, 23); cmbPolRamo.TabIndex = 2;

        lblPolBien.AutoSize = true; lblPolBien.Location = new Point(580, 24); lblPolBien.Text = "Bien Asegurado / Descripción:";
        txtPolBien.Location = new Point(580, 42); txtPolBien.Size = new Size(320, 23); txtPolBien.TabIndex = 3;

        lblPolPrima.AutoSize = true; lblPolPrima.Location = new Point(14, 76); lblPolPrima.Text = "Prima Total ($):";
        txtPolPrima.AccessibleName = "Prima Total en pesos";
        txtPolPrima.Location = new Point(14, 94); txtPolPrima.Size = new Size(130, 23); txtPolPrima.TabIndex = 4;
        txtPolPrima.TextChanged += ActualizarPreviewCuota_Event;

        lblPolCuotas.AutoSize = true; lblPolCuotas.Location = new Point(156, 76); lblPolCuotas.Text = "Cuotas:";
        nudPolCuotas.AccessibleName = "Cantidad de cuotas";
        nudPolCuotas.Location = new Point(156, 94); nudPolCuotas.Maximum = 24; nudPolCuotas.Minimum = 1; nudPolCuotas.Size = new Size(70, 23); nudPolCuotas.TabIndex = 5; nudPolCuotas.Value = 6;
        nudPolCuotas.ValueChanged += ActualizarPreviewCuota_Event;

        lblPolPrimerVto.AutoSize = true; lblPolPrimerVto.Location = new Point(238, 76); lblPolPrimerVto.Text = "1er Vencimiento:";
        dtpPolPrimerVto.AccessibleName = "Fecha de primer vencimiento";
        dtpPolPrimerVto.Format = DateTimePickerFormat.Short; dtpPolPrimerVto.Location = new Point(238, 94); dtpPolPrimerVto.Size = new Size(120, 23); dtpPolPrimerVto.TabIndex = 6;

        btnEmitirPoliza.AccessibleName = "Emitir Póliza y Cuotas";
        btnEmitirPoliza.BackColor = Color.FromArgb(21, 128, 61); btnEmitirPoliza.Cursor = Cursors.Hand; btnEmitirPoliza.FlatStyle = FlatStyle.Flat; btnEmitirPoliza.FlatAppearance.BorderSize = 0;
        btnEmitirPoliza.ForeColor = Color.White; btnEmitirPoliza.Location = new Point(388, 86); btnEmitirPoliza.Size = new Size(210, 36); btnEmitirPoliza.TabIndex = 7;
        btnEmitirPoliza.Text = "  Emitir Póliza y Cuotas"; btnEmitirPoliza.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnEmitirPoliza.Click += btnEmitirPoliza_Click;

        lblResumenCuota.AutoSize = true;
        lblResumenCuota.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        lblResumenCuota.ForeColor = Color.FromArgb(15, 76, 129);
        lblResumenCuota.Location = new Point(615, 96);
        lblResumenCuota.Name = "lblResumenCuota";
        lblResumenCuota.Text = "Valor estimado por cuota: --";

        grpListaPolizas.Dock = DockStyle.Left;
        grpListaPolizas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpListaPolizas.Text = "Pólizas Emitidas (Click para ver cuotas)";
        grpListaPolizas.Width = 580;
        grpListaPolizas.Controls.Add(dgvPolizas);

        dgvPolizas.AllowUserToAddRows = false; dgvPolizas.AllowUserToDeleteRows = false;
        dgvPolizas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPolizas.BackgroundColor = Color.White; dgvPolizas.Dock = DockStyle.Fill;
        dgvPolizas.MultiSelect = false; dgvPolizas.ReadOnly = true;
        dgvPolizas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPolizas.SelectionChanged += dgvPolizas_SelectionChanged;

        grpCuotasPoliza.Dock = DockStyle.Fill;
        grpCuotasPoliza.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpCuotasPoliza.Text = "Plan de Cuotas de la Póliza Seleccionada";
        grpCuotasPoliza.Controls.Add(dgvCuotasPoliza);

        dgvCuotasPoliza.AllowUserToAddRows = false; dgvCuotasPoliza.AllowUserToDeleteRows = false;
        dgvCuotasPoliza.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCuotasPoliza.BackgroundColor = Color.White; dgvCuotasPoliza.Dock = DockStyle.Fill;
        dgvCuotasPoliza.MultiSelect = false; dgvCuotasPoliza.ReadOnly = true;
        dgvCuotasPoliza.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // StatusStrip
        statusStrip.Items.Add(toolStripStatusLabel);
        statusStrip.Location = new Point(0, 678);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1120, 22);
        toolStripStatusLabel.Text = "Listo | Base de datos: MariaDB (santillan_seguros)";

        // FormPrincipal
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1120, 700);
        Controls.Add(tabMain);
        Controls.Add(statusStrip);
        Controls.Add(panelTop);
        MinimumSize = new Size(980, 620);
        Name = "FormPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Federación Santillán Seguros - Gestión de Cartera y Vencimientos";
        Load += FormPrincipal_Load;

        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        tabMain.ResumeLayout(false);
        tabAlertas.ResumeLayout(false);
        tabAsegurados.ResumeLayout(false);
        tabAsegurados.PerformLayout();
        tabPolizas.ResumeLayout(false);
        panelKpis.ResumeLayout(false);
        panelKpis.PerformLayout();
        cardVencidas.ResumeLayout(false);
        cardVencidas.PerformLayout();
        cardPorVencer.ResumeLayout(false);
        cardPorVencer.PerformLayout();
        grpVencidas.ResumeLayout(false);
        grpPorVencer.ResumeLayout(false);
        grpFormAsegurado.ResumeLayout(false);
        grpFormAsegurado.PerformLayout();
        grpNuevaPoliza.ResumeLayout(false);
        grpNuevaPoliza.PerformLayout();
        grpListaPolizas.ResumeLayout(false);
        grpCuotasPoliza.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvVencidas).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPorVencer).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvAsegurados).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPolizas).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvCuotasPoliza).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudPolCuotas).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
