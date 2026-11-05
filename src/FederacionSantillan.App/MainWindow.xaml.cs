using System;
using System.Windows;
using FederacionSantillan.Core;
using FederacionSantillan.Core.Controllers;

namespace FederacionSantillan.App;

/// <summary>
/// Vista Principal (V en MVC):
/// Responsabilidad única: Renderizado y captura de eventos de usuario.
/// Desacoplada de lógica de negocio y repositorios mediante AseguradoraController.
/// </summary>
public partial class MainWindow : Window
{
    private readonly AseguradoraController _controller;

    public MainWindow()
    {
        InitializeComponent();
        _controller = new AseguradoraController();
        InicializarDatosEjemplo();
        RefrescarVistas();
    }

    private void InicializarDatosEjemplo()
    {
        var inicio = new Fecha(1, 1, 2026);
        var fin = new Fecha(31, 12, 2026);

        // Pólizas iniciales a través del controlador
        _controller.EmitirPoliza(1001, "AUTO", 15000f, 1200000f, 40000f, inicio, fin, 101, "20-33445566-9", "Federico Santillan", "11-4500-1122");
        _controller.EmitirPoliza(1002, "HOGAR", 8500f, 600000f, 25000f, inicio, fin, 102, "27-44556677-3", "Valeria Morales", "11-4500-3344");

        // Siniestro y liquidación de muestra
        var fSin = new Fecha(10, 2, 2026);
        _controller.RegistrarSiniestro(901, 1001, "Colisión vehicular frontal en cruce.", 85000f, fSin);
        _controller.LiquidarSiniestro(901, "Ing. Martin Santillan", 80000f, true);
    }

    private void RefrescarVistas()
    {
        GridPolizas.ItemsSource = null;
        GridPolizas.ItemsSource = _controller.ObtenerPolizas();

        GridSiniestros.ItemsSource = null;
        GridSiniestros.ItemsSource = _controller.ObtenerSiniestros();

        TxtKpiPolizas.Text = _controller.TotalPolizas.ToString();
        TxtKpiSiniestros.Text = _controller.TotalSiniestros.ToString();
        TxtKpiLiquidaciones.Text = $"${_controller.CalcularTotalLiquidado():N2}";
    }

    private void BtnEmitirPoliza_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int num = int.Parse(InpPolNumero.Text);
            string tipo = CmbPolTipo.Text;
            float cobertura = float.Parse(InpPolCobertura.Text);
            float deducible = float.Parse(InpPolDeducible.Text);
            float prima = float.Parse(InpPolPrima.Text);

            var inicio = new Fecha(1, 1, 2026);
            var fin = new Fecha(31, 12, 2026);

            int codTitular = _controller.TotalPolizas + 101;
            bool exito = _controller.EmitirPoliza(
                num, tipo, prima, cobertura, deducible, inicio, fin,
                codTitular, InpTitularDni.Text, InpTitularNombre.Text, "11-0000-0000"
            );

            if (exito)
            {
                MessageBox.Show($"Póliza #{num} emitida con éxito en memoria Heap (Indexación O(1)).", "Operación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                RefrescarVistas();
            }
            else
            {
                MessageBox.Show($"Ya existe una póliza con el número #{num}.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al emitir póliza: {ex.Message}", "Excepción de Formato", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnRegistrarSiniestro_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int numSin = int.Parse(InpSinNumero.Text);
            int numPol = int.Parse(InpSinPolizaNum.Text);
            float monto = float.Parse(InpSinMonto.Text);

            var fSin = new Fecha(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            bool exito = _controller.RegistrarSiniestro(numSin, numPol, InpSinDescripcion.Text, monto, fSin);

            if (exito)
            {
                MessageBox.Show($"Siniestro #{numSin} registrado con estado DENUNCIADO.", "FNOL Registrado", MessageBoxButton.OK, MessageBoxImage.Information);
                RefrescarVistas();
            }
            else
            {
                MessageBox.Show($"No se pudo registrar el siniestro #{numSin} (Póliza inexistente o ID duplicado).", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnLiquidar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int numSin = int.Parse(InpSinNumero.Text);
            float montoPeritado = float.Parse(InpPeritoMonto.Text);
            bool validados = ChkDanosValidados.IsChecked ?? false;

            var resultado = _controller.LiquidarSiniestro(numSin, InpPeritoNombre.Text, montoPeritado, validados);

            if (resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Liquidación Exitosa (SOLID Strategy)", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Resultado Liquidación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            RefrescarVistas();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error en liquidación: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnSimulacroMemoria_Click(object sender, RoutedEventArgs e)
    {
        TxtInspectorLog.Text = _controller.EjecutarSimulacroMemoria();
    }
}
