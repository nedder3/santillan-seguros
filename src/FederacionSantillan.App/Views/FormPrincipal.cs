using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;
using FederacionSantillan.Core;

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
        btnCerrarSesion.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(127, 29, 29), "←");
        btnRefrescarAlertas.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(30, 58, 138), "↻");
        btnAsegGuardar.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(20, 83, 45), "✓");
        btnEmitirPoliza.Image = IconHelper.CrearIconoCirculo(Color.FromArgb(20, 83, 45), "+");
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

    private void RecargarTodo()
    {
        try
        {
            CargarAlertas();
            CargarAsegurados();
            CargarCombosPolizas();
            CargarPolizas();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar datos desde MariaDB: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ==================== ALERTAS Y NOTIFICACIONES ====================
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
    }

    private void btnRefrescarAlertas_Click(object sender, EventArgs e)
    {
        try { CargarAlertas(); }
        catch (Exception ex) { MessageBox.Show($"Error al actualizar alertas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void btnPagarVencida_Click(object sender, EventArgs e) => ProcesarPagoDesdeGrilla(dgvVencidas);

    private void btnPagarPorVencer_Click(object sender, EventArgs e) => ProcesarPagoDesdeGrilla(dgvPorVencer);

    private void ProcesarPagoDesdeGrilla(DataGridView grilla)
    {
        try
        {
            if (grilla.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota de la lista para registrar su pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idCuota = Convert.ToInt32(grilla.CurrentRow.Cells["IdCuota"].Value);
            string asegurado = grilla.CurrentRow.Cells["Asegurado"].Value?.ToString();
            string metodo = cmbMetodoPagoAlerta.SelectedItem?.ToString() ?? "Transferencia";

            var conf = MessageBox.Show(
                $"¿Confirma registrar el pago de la cuota seleccionada para {asegurado} vía {metodo}?",
                "Confirmar Cobro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (conf != DialogResult.Yes) return;

            var (exito, mensaje) = _notificacionController.RegistrarPagoCuota(idCuota, metodo);
            MessageBox.Show(mensaje, exito ? "Cobro Exitoso" : "Error", MessageBoxButtons.OK, exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (exito) RecargarTodo();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al registrar el pago: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ==================== ASEGURADOS ====================
    private void CargarAsegurados()
    {
        var lista = _carteraController.ListarAsegurados(txtBuscarAseg.Text);
        dgvAsegurados.DataSource = lista.Select(a => new
        {
            Id = a.getCodigo(),
            DNI_CUIT = a.getDni(),
            Nombre = a.getNombre(),
            Telefono = a.getTelefono(),
            Email = a.getEmail(),
            Domicilio = a.getDomicilio()
        }).ToList();

        if (dgvAsegurados.Columns["Id"] != null) dgvAsegurados.Columns["Id"].Visible = false;
    }

    private void txtBuscarAseg_TextChanged(object sender, EventArgs e)
    {
        try { CargarAsegurados(); }
        catch { /* Ignorar errores transitorios al teclear rápido */ }
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

    private void btnAsegNuevo_Click(object sender, EventArgs e)
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
        try
        {
            var (exito, mensaje) = _carteraController.GuardarAsegurado(
                _aseguradoSeleccionadoId, txtAsegDni.Text, txtAsegNombre.Text,
                txtAsegTel.Text, txtAsegEmail.Text, txtAsegDom.Text);

            MessageBox.Show(mensaje, exito ? "Éxito" : "Validación", MessageBoxButtons.OK, exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (exito)
            {
                btnAsegNuevo_Click(null, null);
                RecargarTodo();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar asegurado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnAsegEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (_aseguradoSeleccionadoId <= 0)
            {
                MessageBox.Show("Seleccione un asegurado de la grilla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("¿Desea dar de baja este asegurado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var (exito, mensaje) = _carteraController.EliminarAsegurado(_aseguradoSeleccionadoId);
            MessageBox.Show(mensaje, exito ? "Baja Exitosa" : "Error", MessageBoxButtons.OK, exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (exito)
            {
                btnAsegNuevo_Click(null, null);
                RecargarTodo();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al dar de baja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ==================== PÓLIZAS Y CUOTAS ====================
    private void CargarCombosPolizas()
    {
        var asegurados = _carteraController.ListarAsegurados();
        cmbPolAsegurado.DataSource = asegurados;
        cmbPolAsegurado.DisplayMember = "Nombre";
        cmbPolAsegurado.ValueMember = "Codigo";

        var ramos = _carteraController.ListarRamos();
        cmbPolRamo.DataSource = ramos;
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
        catch { /* Ignorar errores transitorios de selección */ }
    }

    private void btnEmitirPoliza_Click(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(txtPolNum.Text, out int numPoliza))
            {
                MessageBox.Show("Ingrese un número de póliza válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtPolPrima.Text, out decimal prima))
            {
                MessageBox.Show("Ingrese una prima total numérica válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idAseg = cmbPolAsegurado.SelectedValue != null ? Convert.ToInt32(cmbPolAsegurado.SelectedValue) : 0;
            int idRamo = cmbPolRamo.SelectedValue != null ? Convert.ToInt32(cmbPolRamo.SelectedValue) : 0;

            var (exito, mensaje) = _carteraController.EmitirPoliza(
                numPoliza, idAseg, idRamo, "Federación Patronal Seguros",
                txtPolBien.Text, prima, (int)nudPolCuotas.Value,
                DateTime.Today, DateTime.Today.AddYears(1), dtpPolPrimerVto.Value);

            MessageBox.Show(mensaje, exito ? "Póliza Emitida" : "Validación", MessageBoxButtons.OK, exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (exito)
            {
                txtPolNum.Clear();
                txtPolBien.Clear();
                txtPolPrima.Clear();
                RecargarTodo();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al emitir póliza: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        _authController.CerrarSesion();
        DialogResult = DialogResult.Retry;
        Close();
    }
}
