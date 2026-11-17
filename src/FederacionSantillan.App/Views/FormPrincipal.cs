using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Handlers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Core.DTOs;

namespace FederacionSantillan.App.Views;

public partial class FormPrincipal : Form
{
    private readonly AuthController _authController = new AuthController();
    private readonly CarteraController _carteraController = new CarteraController();
    private readonly NotificacionController _notificacionController = new NotificacionController();
    private int _aseguradoSeleccionadoId = 0;

    public FormPrincipal()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
        btnCerrarSesion.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(127, 29, 29), "back");
        btnRefrescarAlertas.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(30, 58, 138), "refresh");
        btnAsegGuardar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "check");
        btnEmitirPoliza.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(15, 92, 45), "plus");
        ConfigurarGrillas();
    }

    private void ConfigurarGrillas()
    {
        DataGridView[] grillas = { dgvVencidas, dgvPorVencer, dgvAsegurados, dgvPolizas, dgvCuotasPoliza };
        foreach (var dgv in grillas)
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
        if (AuthController.UsuarioActual != null)
        {
            lblUsuarioActivo.Text = $"Conectado: {AuthController.UsuarioActual.obtenerNombreCompleto()} ({AuthController.UsuarioActual.obtenerRol()})";
            toolStripStatusLabel.Text = $"Sesión activa: {AuthController.UsuarioActual.obtenerUsername()} | Motor: MariaDB";
        }
        RecargarTodo();
    }

    private void RecargarTodo() => GlobalExceptionHandler.Ejecutar(() =>
    {
        CargarAlertas();
        CargarAsegurados();
        CargarCombosPolizas();
        CargarPolizas();
    });

    private void CargarAlertas()
    {
        var vencidas = _notificacionController.ObtenerCuotasVencidas();
        var porVencer = _notificacionController.ObtenerCuotasPorVencer(15);

        lblKpiVencidasValor.Text = $"{vencidas.Count} cuotas (${vencidas.Sum(c => c.Monto):N0})";
        lblKpiPorVencerValor.Text = $"{porVencer.Count} cuotas (${porVencer.Sum(c => c.Monto):N0})";

        dgvVencidas.DataSource = vencidas.Select(c => new
        {
            IdCuota = c.IdCuota,
            Poliza = c.NumeroPolizaRef,
            Asegurado = c.AseguradoNombreRef,
            Telefono = c.AseguradoTelefonoRef,
            Ramo = c.RamoNombreRef,
            Cuota = $"#{c.NumeroCuota}",
            Monto = c.Monto.ToString("C2"),
            Vencimiento = c.FechaVencimiento.ToString("dd/MM/yyyy"),
            DiasAtraso = $"{c.diasDeAtraso()} días"
        }).ToList();

        if (dgvVencidas.Columns["IdCuota"] != null) dgvVencidas.Columns["IdCuota"].Visible = false;

        dgvPorVencer.DataSource = porVencer.Select(c => new
        {
            IdCuota = c.IdCuota,
            Poliza = c.NumeroPolizaRef,
            Asegurado = c.AseguradoNombreRef,
            Telefono = c.AseguradoTelefonoRef,
            Ramo = c.RamoNombreRef,
            Cuota = $"#{c.NumeroCuota}",
            Monto = c.Monto.ToString("C2"),
            Vencimiento = c.FechaVencimiento.ToString("dd/MM/yyyy"),
            Faltan = $"{c.diasParaVencer()} días"
        }).ToList();

        if (dgvPorVencer.Columns["IdCuota"] != null) dgvPorVencer.Columns["IdCuota"].Visible = false;

        string user = AuthController.UsuarioActual?.obtenerUsername() ?? "sesion";
        toolStripStatusLabel.Text = $"Sesión activa: {user} | Actualizado: {DateTime.Now:HH:mm:ss} | Motor: MariaDB";
    }

    private void btnRefrescarAlertas_Click(object sender, EventArgs e) =>
        GlobalExceptionHandler.Ejecutar(CargarAlertas);

    private void btnPagarVencida_Click(object sender, EventArgs e) => ProcesarPagoDesdeGrilla(dgvVencidas);

    private void btnPagarPorVencer_Click(object sender, EventArgs e) => ProcesarPagoDesdeGrilla(dgvPorVencer);

    private void ProcesarPagoDesdeGrilla(DataGridView grilla)
    {
        if (grilla.CurrentRow == null)
        {
            MessageBox.Show("Seleccione una cuota de la lista para registrar su pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int idCuota = Convert.ToInt32(grilla.CurrentRow.Cells["IdCuota"].Value);
        string asegurado = grilla.CurrentRow.Cells["Asegurado"].Value?.ToString();
        string metodo = cmbMetodoPagoAlerta.SelectedItem?.ToString() ?? "Transferencia";

        if (MessageBox.Show($"¿Confirma registrar el pago de la cuota seleccionada para {asegurado} vía {metodo}?", "Confirmar Cobro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        GlobalExceptionHandler.Ejecutar(() =>
        {
            string mensaje = _notificacionController.RegistrarPagoCuota(idCuota, metodo);
            MessageBox.Show(mensaje, "Cobro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RecargarTodo();
        });
    }

    private void CargarAsegurados()
    {
        var lista = _carteraController.ListarAsegurados(txtBuscarAseg.Text);
        dgvAsegurados.DataSource = lista.Select(a => new
        {
            Id = a.obtenerCodigo(),
            DNI_CUIT = a.obtenerDni(),
            Nombre = a.obtenerNombre(),
            Telefono = a.obtenerTelefono(),
            Email = a.obtenerEmail(),
            Domicilio = a.obtenerDomicilio()
        }).ToList();

        if (dgvAsegurados.Columns["Id"] != null) dgvAsegurados.Columns["Id"].Visible = false;
    }

    private void txtBuscarAseg_TextChanged(object sender, EventArgs e)
    {
        try { CargarAsegurados(); }
        catch { }
    }

    private void dgvAsegurados_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvAsegurados.CurrentRow == null) return;
        _aseguradoSeleccionadoId = Convert.ToInt32(dgvAsegurados.CurrentRow.Cells["Id"].Value);
        txtAsegDni.Text = dgvAsegurados.CurrentRow.Cells["DNI_CUIT"].Value?.ToString();
        txtAsegNombre.Text = dgvAsegurados.CurrentRow.Cells["Nombre"].Value?.ToString();
        txtAsegTel.Text = dgvAsegurados.CurrentRow.Cells["Telefono"].Value?.ToString();
        txtAsegEmail.Text = dgvAsegurados.CurrentRow.Cells["Email"].Value?.ToString();
        txtAsegDom.Text = dgvAsegurados.CurrentRow.Cells["Domicilio"].Value?.ToString();
    }

    private void btnAsegNuevo_Click(object sender, EventArgs e) => LimpiarFormularioAsegurado();

    private void LimpiarFormularioAsegurado()
    {
        _aseguradoSeleccionadoId = 0;
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
        var dto = new AseguradoInputDTO
        {
            Id = _aseguradoSeleccionadoId,
            DniCuit = txtAsegDni.Text,
            ApellidoNombre = txtAsegNombre.Text,
            Telefono = txtAsegTel.Text,
            Email = txtAsegEmail.Text,
            Domicilio = txtAsegDom.Text
        };

        GlobalExceptionHandler.Ejecutar(() =>
        {
            string mensaje = _carteraController.GuardarAsegurado(dto);
            MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormularioAsegurado();
            RecargarTodo();
        }, txtAsegDni);
    }

    private void btnAsegEliminar_Click(object sender, EventArgs e)
    {
        if (_aseguradoSeleccionadoId <= 0)
        {
            MessageBox.Show("Seleccione un asegurado de la grilla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (MessageBox.Show("¿Desea dar de baja este asegurado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        GlobalExceptionHandler.Ejecutar(() =>
        {
            _carteraController.EliminarAsegurado(_aseguradoSeleccionadoId);
            MessageBox.Show("Asegurado dado de baja correctamente.", "Baja Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormularioAsegurado();
            RecargarTodo();
        });
    }

    private void CargarCombosPolizas()
    {
        cmbPolAsegurado.DataSource = _carteraController.ListarAsegurados();
        cmbPolAsegurado.DisplayMember = "Nombre";
        cmbPolAsegurado.ValueMember = "Codigo";

        cmbPolRamo.DataSource = _carteraController.ListarRamos();
        cmbPolRamo.DisplayMember = "Nombre";
        cmbPolRamo.ValueMember = "IdRamo";
    }

    private void CargarPolizas()
    {
        var polizas = _carteraController.ListarPolizas();
        dgvPolizas.DataSource = polizas.Select(p => new
        {
            IdPoliza = p.IdPoliza,
            Numero = p.NumeroPoliza,
            Asegurado = p.AseguradoNombre,
            Ramo = p.RamoNombre,
            Bien = p.BienAsegurado,
            Prima = p.PrimaTotal.ToString("C2"),
            Estado = p.Estado
        }).ToList();

        if (dgvPolizas.Columns["IdPoliza"] != null) dgvPolizas.Columns["IdPoliza"].Visible = false;
    }

    private void dgvPolizas_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgvPolizas.CurrentRow == null) return;
            int idPoliza = Convert.ToInt32(dgvPolizas.CurrentRow.Cells["IdPoliza"].Value);
            var cuotas = _carteraController.ListarCuotasPorPoliza(idPoliza);
            dgvCuotasPoliza.DataSource = cuotas.Select(c => new
            {
                Cuota = $"#{c.NumeroCuota}",
                Monto = c.Monto.ToString("C2"),
                Vencimiento = c.FechaVencimiento.ToString("dd/MM/yyyy"),
                Estado = c.Estado,
                Pago = c.FechaPago.HasValue ? c.FechaPago.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList();
        }
        catch { }
    }

    private void ActualizarPreviewCuota_Event(object sender, EventArgs e)
    {
        errorProvider.SetError(txtPolPrima, string.Empty);
        if (decimal.TryParse(txtPolPrima.Text, out decimal prima) && prima > 0 && nudPolCuotas.Value > 0)
        {
            decimal valorCuota = prima / nudPolCuotas.Value;
            lblResumenCuota.Text = $"Valor estimado por cuota: {valorCuota:C2} ({(int)nudPolCuotas.Value} cuotas)";
        }
        else
        {
            lblResumenCuota.Text = "Valor estimado por cuota: --";
        }
    }

    private void btnEmitirPoliza_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        int.TryParse(txtPolNum.Text, out int numPoliza);
        decimal.TryParse(txtPolPrima.Text, out decimal prima);

        var dto = new PolizaInputDTO
        {
            NumeroPoliza = numPoliza,
            IdAsegurado = cmbPolAsegurado.SelectedValue != null ? Convert.ToInt32(cmbPolAsegurado.SelectedValue) : 0,
            IdRamo = cmbPolRamo.SelectedValue != null ? Convert.ToInt32(cmbPolRamo.SelectedValue) : 0,
            Compania = "Federación Patronal Seguros",
            BienAsegurado = txtPolBien.Text,
            PrimaTotal = prima,
            CantidadCuotas = (int)nudPolCuotas.Value,
            FechaInicio = DateTime.Today,
            FechaFin = DateTime.Today.AddYears(1),
            PrimerVencimiento = dtpPolPrimerVto.Value
        };

        GlobalExceptionHandler.Ejecutar(() =>
        {
            string mensaje = _carteraController.EmitirPoliza(dto);
            MessageBox.Show(mensaje, "Póliza Emitida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPolNum.Clear();
            txtPolBien.Clear();
            txtPolPrima.Clear();
            RecargarTodo();
        }, txtPolNum);
    }

    private void btnAyuda_Click(object sender, EventArgs e)
    {
        using var ayuda = new FormAyuda();
        ayuda.ShowDialog(this);
    }

    private void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        _authController.CerrarSesion();
        DialogResult = DialogResult.Retry;
        Close();
    }
}
