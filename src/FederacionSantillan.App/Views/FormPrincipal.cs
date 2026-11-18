using FederacionSantillan.App.Advice;
using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.DTOs.Responses;
using FederacionSantillan.Model.Entities;

namespace FederacionSantillan.App.Views;

public partial class FormPrincipal : Form
{
    private readonly AuthController _authController = new();
    private readonly PortfolioController _portfolioController = new();
    private readonly NotificationController _notificationController = new();
    private AlertsDashboardResponseDto _currentAlerts = new();
    private int _selectedInsuredId = 0;

    public FormPrincipal()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
        btnCerrarSesion.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(127, 29, 29), "back");
        btnRefrescarAlertas.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(30, 58, 138), "refresh");
        btnAsegGuardar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "check");
        btnEmitirPoliza.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "plus");
        ConfigureGrids();
    }

    private void ConfigureGrids()
    {
        DataGridView[] grids = { dgvVencidas, dgvPorVencer, dgvAsegurados, dgvPolizas, dgvCuotasPoliza };
        foreach (var dgv in grids)
        {
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);
            dgv.RowTemplate.Height = 28;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeight = 32;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 129);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
        }
    }

    private void FormPrincipal_Load(object sender, EventArgs e)
    {
        if (AuthController.CurrentUser != null)
        {
            lblUsuarioActivo.Text = $"Conectado: {AuthController.CurrentUser.getFullName()} ({AuthController.CurrentUser.getRole()})";
            toolStripStatusLabel.Text = $"Sesión activa: {AuthController.CurrentUser.getUsername()} | Motor: MariaDB";
        }
        ReloadAll();
    }

    private void ReloadAll() => ControllerPipeline.Execute(() =>
    {
        LoadAlerts();
        LoadInsureds();
        LoadPolicyCombos();
        LoadPolicies();
    });

    private int GetSelectedDaysAhead() => cmbDiasAnticipacion.SelectedIndex switch
    {
        0 => 7,
        2 => 30,
        _ => 15
    };

    private void LoadAlerts()
    {
        int days = GetSelectedDaysAhead();
        _currentAlerts = _notificationController.LoadAlertsDashboard(days);

        lblKpiVencidasValor.Text = $"{_currentAlerts.Overdue.Count} cuotas (${_currentAlerts.OverdueTotal:N0})";
        lblKpiPorVencerValor.Text = $"{_currentAlerts.Upcoming.Count} cuotas (${_currentAlerts.UpcomingTotal:N0})";

        dgvVencidas.DataSource = _currentAlerts.Overdue.Select(c => new
        {
            InstallmentId = c.InstallmentId,
            Poliza = c.PolicyNumberRef,
            Asegurado = c.InsuredNameRef,
            Telefono = c.InsuredPhoneRef,
            Ramo = c.BranchNameRef,
            Cuota = $"#{c.InstallmentNumber}",
            Monto = c.Amount.ToString("C2"),
            Vencimiento = c.DueDate.ToString("dd/MM/yyyy"),
            DiasAtraso = $"{c.getDaysOverdue()} días"
        }).ToList();

        if (dgvVencidas.Columns["InstallmentId"] != null) dgvVencidas.Columns["InstallmentId"].Visible = false;

        dgvPorVencer.DataSource = _currentAlerts.Upcoming.Select(c => new
        {
            InstallmentId = c.InstallmentId,
            Poliza = c.PolicyNumberRef,
            Asegurado = c.InsuredNameRef,
            Telefono = c.InsuredPhoneRef,
            Ramo = c.BranchNameRef,
            Cuota = $"#{c.InstallmentNumber}",
            Monto = c.Amount.ToString("C2"),
            Vencimiento = c.DueDate.ToString("dd/MM/yyyy"),
            Faltan = $"{c.getDaysUntilDue()} días"
        }).ToList();

        if (dgvPorVencer.Columns["InstallmentId"] != null) dgvPorVencer.Columns["InstallmentId"].Visible = false;

        string user = AuthController.CurrentUser?.getUsername() ?? "sesion";
        toolStripStatusLabel.Text = $"Sesión activa: {user} | Actualizado: {DateTime.Now:HH:mm:ss} | Ventana: {days} días | MariaDB";
    }

    private void btnRefrescarAlertas_Click(object sender, EventArgs e) =>
        ControllerPipeline.Execute(LoadAlerts);

    private void btnPagarVencida_Click(object sender, EventArgs e) => ProcessPaymentFromGrid(dgvVencidas);

    private void btnPagarPorVencer_Click(object sender, EventArgs e) => ProcessPaymentFromGrid(dgvPorVencer);

    private void ProcessPaymentFromGrid(DataGridView grid)
    {
        if (grid.CurrentRow == null)
        {
            MessageBox.Show("Seleccione una cuota de la lista para registrar su pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var dto = new PaymentRequestDto
        {
            InstallmentId = Convert.ToInt32(grid.CurrentRow.Cells["InstallmentId"].Value),
            PaymentMethod = cmbMetodoPagoAlerta.SelectedItem?.ToString() ?? "Transferencia"
        };
        string insuredName = grid.CurrentRow.Cells["Asegurado"].Value?.ToString();

        if (MessageBox.Show($"¿Confirma registrar el pago de la cuota seleccionada para {insuredName} vía {dto.PaymentMethod}?", "Confirmar Cobro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        ControllerPipeline.Execute(dto, req =>
        {
            string msg = _notificationController.RegisterInstallmentPayment(req);
            MessageBox.Show(msg, "Cobro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReloadAll();
        });
    }

    private void btnWhatsAppAviso_Click(object sender, EventArgs e)
    {
        DataGridView activeGrid = dgvVencidas.Focused || dgvPorVencer.CurrentRow == null ? dgvVencidas : dgvPorVencer;
        if (activeGrid.CurrentRow == null) activeGrid = dgvPorVencer.CurrentRow != null ? dgvPorVencer : dgvVencidas;

        if (activeGrid.CurrentRow == null)
        {
            MessageBox.Show("Seleccione una cuota vencida o por vencer para generar el aviso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int installmentId = Convert.ToInt32(activeGrid.CurrentRow.Cells["InstallmentId"].Value);
        if (!_currentAlerts.InstallmentIndex.TryGetValue(installmentId, out Installment installment))
            return;

        ControllerPipeline.Execute(() =>
        {
            string reminder = _notificationController.LogAndBuildWhatsAppReminder(installment);
            Clipboard.SetText(reminder);
            MessageBox.Show($"Mensaje copiado al portapapeles y registrado en MariaDB:\n\n{reminder}", "Aviso WhatsApp Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnExportarCsv_Click(object sender, EventArgs e)
    {
        if (_currentAlerts.Overdue.Count == 0)
        {
            MessageBox.Show("No hay cuotas vencidas para exportar.", "Exportar CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "Archivos CSV (*.csv)|*.csv",
            FileName = $"morosos_santillan_{DateTime.Today:yyyyMMdd}.csv"
        };

        if (sfd.ShowDialog(this) != DialogResult.OK) return;

        ControllerPipeline.Execute(() =>
        {
            string csv = _notificationController.ExportOverdueToCsv(_currentAlerts.Overdue);
            File.WriteAllText(sfd.FileName, csv, System.Text.Encoding.UTF8);
            MessageBox.Show($"Listado exportado correctamente a:\n{sfd.FileName}", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void LoadInsureds()
    {
        var list = _portfolioController.ListInsureds(txtBuscarAseg.Text);
        dgvAsegurados.DataSource = list.Select(a => new
        {
            Id = a.getInsuredId(),
            DNI_CUIT = a.getTaxId(),
            Nombre = a.getFullName(),
            Telefono = a.getPhone(),
            Email = a.getEmail(),
            Domicilio = a.getAddress()
        }).ToList();

        if (dgvAsegurados.Columns["Id"] != null) dgvAsegurados.Columns["Id"].Visible = false;
    }

    private void txtBuscarAseg_TextChanged(object sender, EventArgs e) =>
        ControllerPipeline.Execute(LoadInsureds);

    private void dgvAsegurados_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvAsegurados.CurrentRow == null) return;
        _selectedInsuredId = Convert.ToInt32(dgvAsegurados.CurrentRow.Cells["Id"].Value);
        txtAsegDni.Text = dgvAsegurados.CurrentRow.Cells["DNI_CUIT"].Value?.ToString();
        txtAsegNombre.Text = dgvAsegurados.CurrentRow.Cells["Nombre"].Value?.ToString();
        txtAsegTel.Text = dgvAsegurados.CurrentRow.Cells["Telefono"].Value?.ToString();
        txtAsegEmail.Text = dgvAsegurados.CurrentRow.Cells["Email"].Value?.ToString();
        txtAsegDom.Text = dgvAsegurados.CurrentRow.Cells["Domicilio"].Value?.ToString();
    }

    private void btnAsegNuevo_Click(object sender, EventArgs e) => ClearInsuredForm();

    private void ClearInsuredForm()
    {
        _selectedInsuredId = 0;
        txtAsegDni.Clear();
        txtAsegNombre.Clear();
        txtAsegTel.Clear();
        txtAsegEmail.Clear();
        txtAsegDom.Clear();
        txtAsegDni.Focus();
    }

    private void btnAsegGuardar_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        var dto = new InsuredRequestDto
        {
            Id = _selectedInsuredId,
            TaxId = txtAsegDni.Text,
            FullName = txtAsegNombre.Text,
            Phone = txtAsegTel.Text,
            Email = txtAsegEmail.Text,
            Address = txtAsegDom.Text
        };

        ControllerPipeline.Execute(dto, req =>
        {
            string msg = _portfolioController.SaveInsured(req);
            MessageBox.Show(msg, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInsuredForm();
            ReloadAll();
        }, txtAsegDni);
    }

    private void btnAsegEliminar_Click(object sender, EventArgs e)
    {
        if (_selectedInsuredId <= 0)
        {
            MessageBox.Show("Seleccione un asegurado de la grilla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (MessageBox.Show("¿Desea dar de baja este asegurado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        ControllerPipeline.Execute(() =>
        {
            _portfolioController.DeleteInsured(_selectedInsuredId);
            MessageBox.Show("Asegurado dado de baja correctamente.", "Baja Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInsuredForm();
            ReloadAll();
        });
    }

    private void LoadPolicyCombos()
    {
        cmbPolAsegurado.DataSource = _portfolioController.ListInsureds();
        cmbPolAsegurado.DisplayMember = "FullName";
        cmbPolAsegurado.ValueMember = "InsuredId";

        cmbPolRamo.DataSource = _portfolioController.ListBranches();
        cmbPolRamo.DisplayMember = "Name";
        cmbPolRamo.ValueMember = "BranchId";
    }

    private void LoadPolicies()
    {
        var policies = _portfolioController.ListPolicies();
        dgvPolizas.DataSource = policies.Select(p => new
        {
            PolicyId = p.PolicyId,
            Numero = p.PolicyNumber,
            Asegurado = p.InsuredName,
            Ramo = p.BranchName,
            Bien = p.InsuredAsset,
            Prima = p.TotalPremium.ToString("C2"),
            Estado = p.Status
        }).ToList();

        if (dgvPolizas.Columns["PolicyId"] != null) dgvPolizas.Columns["PolicyId"].Visible = false;
    }

    private void dgvPolizas_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvPolizas.CurrentRow == null) return;
        int policyId = Convert.ToInt32(dgvPolizas.CurrentRow.Cells["PolicyId"].Value);
        ControllerPipeline.Execute(() =>
        {
            var installments = _portfolioController.ListInstallmentsByPolicy(policyId);
            dgvCuotasPoliza.DataSource = installments.Select(c => new
            {
                Cuota = $"#{c.InstallmentNumber}",
                Monto = c.Amount.ToString("C2"),
                Vencimiento = c.DueDate.ToString("dd/MM/yyyy"),
                Estado = c.Status,
                Pago = c.PaymentDate.HasValue ? c.PaymentDate.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList();
        });
    }

    private void ActualizarPreviewCuota_Event(object sender, EventArgs e)
    {
        errorProvider.SetError(txtPolPrima, string.Empty);
        if (decimal.TryParse(txtPolPrima.Text, out decimal premium) && premium > 0 && nudPolCuotas.Value > 0)
        {
            decimal installmentValue = premium / nudPolCuotas.Value;
            lblResumenCuota.Text = $"Valor estimado por cuota: {installmentValue:C2} ({(int)nudPolCuotas.Value} cuotas)";
        }
        else
        {
            lblResumenCuota.Text = "Valor estimado por cuota: --";
        }
    }

    private void btnEmitirPoliza_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        int.TryParse(txtPolNum.Text, out int policyNumber);
        decimal.TryParse(txtPolPrima.Text, out decimal premium);

        var dto = new PolicyInputDtoFactory().Create(
            policyNumber,
            cmbPolAsegurado.SelectedValue != null ? Convert.ToInt32(cmbPolAsegurado.SelectedValue) : 0,
            cmbPolRamo.SelectedValue != null ? Convert.ToInt32(cmbPolRamo.SelectedValue) : 0,
            txtPolBien.Text,
            premium,
            (int)nudPolCuotas.Value,
            dtpPolPrimerVto.Value);

        ControllerPipeline.Execute(dto, req =>
        {
            string msg = _portfolioController.IssuePolicy(req);
            MessageBox.Show(msg, "Póliza Emitida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPolNum.Clear();
            txtPolBien.Clear();
            txtPolPrima.Clear();
            ReloadAll();
        }, txtPolNum);
    }

    private void btnAyuda_Click(object sender, EventArgs e)
    {
        using var help = new FormAyuda();
        help.ShowDialog(this);
    }

    private void btnOpciones_Click(object sender, EventArgs e)
    {
        using var verify = new FormVerificarPassword();
        if (verify.ShowDialog(this) != DialogResult.OK) return;

        using var options = new FormOpciones();
        if (options.ShowDialog(this) == DialogResult.OK)
            ApplyAccessibilityTheme();
    }

    private void ApplyAccessibilityTheme()
    {
        var palette = AccessibilityTheme.GetCurrentPalette();
        panelTop.BackColor = palette.HeaderBack;
        tabAlertas.BackColor = palette.SurfaceBack;
        tabAsegurados.BackColor = palette.SurfaceBack;
        tabPolizas.BackColor = palette.SurfaceBack;

        cardVencidas.BackColor = palette.OverdueCardBack;
        lblKpiVencidasTitulo.ForeColor = palette.OverdueCardFore;
        lblKpiVencidasValor.ForeColor = palette.OverdueCardFore;
        grpVencidas.ForeColor = palette.OverdueCardFore;

        cardPorVencer.BackColor = palette.UpcomingCardBack;
        lblKpiPorVencerTitulo.ForeColor = palette.UpcomingCardFore;
        lblKpiPorVencerValor.ForeColor = palette.UpcomingCardFore;
        grpPorVencer.ForeColor = palette.UpcomingCardFore;

        btnPagarVencida.BackColor = palette.PrimaryAction;
        btnPagarPorVencer.BackColor = palette.PrimaryAction;
        btnWhatsAppAviso.BackColor = palette.PrimaryAction;
        btnAsegGuardar.BackColor = palette.PrimaryAction;
        btnEmitirPoliza.BackColor = palette.PrimaryAction;
        btnAsegEliminar.BackColor = palette.DangerAction;
        btnCerrarSesion.BackColor = palette.DangerAction;

        tabMain.Font = new Font("Segoe UI", palette.BaseFontSize);
        foreach (DataGridView dgv in new[] { dgvVencidas, dgvPorVencer, dgvAsegurados, dgvPolizas, dgvCuotasPoliza })
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = palette.HeaderBack;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", palette.BaseFontSize);
        }
    }

    private void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        _authController.SignOut();
        DialogResult = DialogResult.Retry;
        Close();
    }
}

internal sealed class PolicyInputDtoFactory
{
    public PolicyRequestDto Create(int number, int insuredId, int branchId, string asset, decimal premium, int count, DateTime firstDue) =>
        new()
        {
            PolicyNumber = number,
            InsuredId = insuredId,
            BranchId = branchId,
            Company = "Federación Patronal Seguros",
            InsuredAsset = asset,
            TotalPremium = premium,
            InstallmentsCount = count,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1),
            FirstDueDate = firstDue
        };
}
