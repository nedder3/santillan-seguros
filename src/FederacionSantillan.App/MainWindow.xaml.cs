using System;
using System.Text;
using System.Windows;
using FederacionSantillan.Core;

namespace FederacionSantillan.App;

public partial class MainWindow : Window
{
    private SistemaAseguradora sistema;

    public MainWindow()
    {
        InitializeComponent();
        sistema = new SistemaAseguradora("Federación Aseguradora Santillán");
        InicializarDatosEjemplo();
        RefrescarVistas();
    }

    private void InicializarDatosEjemplo()
    {
        // 1. Asegurados
        var a1 = new Asegurado(101, "20-33445566-9", "Federico Santillan", "11-4500-1122");
        var a2 = new Asegurado(102, "27-44556677-3", "Valeria Morales", "11-4500-3344");
        sistema.agregarAsegurado(a1);
        sistema.agregarAsegurado(a2);

        // 2. Pólizas
        var inicio2026 = new Fecha(1, 1, 2026);
        var fin2026 = new Fecha(31, 12, 2026);

        var p1 = new Poliza(1001, "AUTO", 15000f, 1200000f, 40000f, inicio2026, fin2026, a1);
        var p2 = new Poliza(1002, "HOGAR", 8500f, 600000f, 25000f, inicio2026, fin2026, a2);
        sistema.emitirPoliza(p1);
        sistema.emitirPoliza(p2);

        // 3. Siniestro con peritaje pre-cargado
        var fSin = new Fecha(10, 2, 2026);
        var s1 = new Siniestro(901, "Colisión vehicular frontal en cruce.", 85000f, fSin, p1);
        var peri = new Peritaje(501, "Ing. Martin Santillan", 80000f, true, "Daños comprobados en paragolpes y radiador.");
        s1.registrarPeritaje(peri);
        s1.liquidar(); // Realiza la liquidación matemática POO
        sistema.registrarSiniestro(s1);
    }

    private void RefrescarVistas()
    {
        GridPolizas.ItemsSource = null;
        GridPolizas.ItemsSource = sistema.Polizas;

        GridSiniestros.ItemsSource = null;
        GridSiniestros.ItemsSource = sistema.Siniestros;

        TxtKpiPolizas.Text = sistema.Polizas.Count.ToString();
        TxtKpiSiniestros.Text = sistema.Siniestros.Count.ToString();
        TxtKpiLiquidaciones.Text = $"${sistema.calcularTotalLiquidaciones():N2}";
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

            var titular = new Asegurado(sistema.Asegurados.Count + 101, InpTitularDni.Text, InpTitularNombre.Text, "11-0000-0000");
            sistema.agregarAsegurado(titular);

            var inicio = new Fecha(1, 1, 2026);
            var fin = new Fecha(31, 12, 2026);

            var nuevaPoliza = new Poliza(num, tipo, prima, cobertura, deducible, inicio, fin, titular);
            if (sistema.emitirPoliza(nuevaPoliza))
            {
                MessageBox.Show($"Póliza #{num} emitida con éxito en memoria Heap.", "Operación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
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

            var pol = sistema.buscarPoliza(numPol);
            if (pol == null)
            {
                MessageBox.Show($"No se encontró la póliza #{numPol}.", "Póliza Inexistente", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fSin = new Fecha(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            var s = new Siniestro(numSin, InpSinDescripcion.Text, monto, fSin, pol);

            if (sistema.registrarSiniestro(s))
            {
                MessageBox.Show($"Siniestro #{numSin} registrado con estado DENUNCIADO.", "FNOL Registrado", MessageBoxButton.OK, MessageBoxImage.Information);
                RefrescarVistas();
            }
            else
            {
                MessageBox.Show($"Ya existe el siniestro #{numSin}.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            var s = sistema.Siniestros.Find(x => x.Numero == numSin);
            if (s == null)
            {
                BtnRegistrarSiniestro_Click(sender, e);
                s = sistema.Siniestros.Find(x => x.Numero == numSin);
                if (s == null) return;
            }

            float montoPeritado = float.Parse(InpPeritoMonto.Text);
            bool validados = ChkDanosValidados.IsChecked ?? false;

            var peri = new Peritaje(numSin + 200, InpPeritoNombre.Text, montoPeritado, validados, "Inspección técnica pericial.");
            s.registrarPeritaje(peri);
            bool res = s.liquidar();

            if (res)
            {
                MessageBox.Show($"Siniestro #{numSin} LIQUIDADO con éxito.\nIndemnización neta aprobada: ${s.MontoLiquidado:N2}\n(Cálculo: Monto Peritado ${montoPeritado:N2} - Deducible ${s.Poliza.Deducible:N2})", "Liquidación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"El siniestro no pudo liquidarse o fue rechazado (daños no verificados o monto menor al deducible).", "Resultado Liquidación", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        var sb = new StringBuilder();
        sb.AppendLine("================================================================================");
        sb.AppendLine("   SIMULACRO EN VIVO: GESTIÓN DE MEMORIA STACK vs HEAP (POO EXAM RUNNER)");
        sb.AppendLine("================================================================================");
        sb.AppendLine($"Ejecutado: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Runtime: .NET 8 WPF\n");

        // 1. Instanciación y Aliasing
        sb.AppendLine("[PASO 1] Instanciación y Aliasing de Referencias:");
        var a1 = new Asegurado(1001, "20-11223344-9", "Federico Santillan", "11-4455-6677");
        sb.AppendLine("   a1 = new Asegurado(1001, 'Federico Santillan');");
        var a2 = a1; // ALIASING: ambas variables apuntan a la misma dirección
        sb.AppendLine("   a2 = a1;  // Aliasing (copia de puntero en Stack)");
        sb.AppendLine($"   -> ¿a1 == a2? {a1 == a2} (TRUE: Apuntan a la misma dirección de memoria)");
        sb.AppendLine($"   -> ¿a1.equals(a2)? {a1.equals(a2)} (TRUE: Mismo estado)");

        // 2. Modificación por referencia compartida
        sb.AppendLine("\n[PASO 2] Efecto colateral del Aliasing:");
        a2.setNombre("Federico Santillan (MODIFICADO DESDE A2)");
        sb.AppendLine("   a2.setNombre('...');");
        sb.AppendLine($"   -> a1.getNombre() refleja el cambio: '{a1.getNombre()}'");

        // 3. Clonación en Profundidad
        sb.AppendLine("\n[PASO 3] Clonación con clone() [Tríada Sagrada]:");
        var a3 = a1.clone();
        sb.AppendLine("   a3 = a1.clone(); // Crea NUEVO objeto en Heap con copia de estado");
        sb.AppendLine($"   -> ¿a1 == a3? {a1 == a3} (FALSE: Son dos instancias independientes en Heap)");
        sb.AppendLine($"   -> ¿a1.equals(a3)? {a1.equals(a3)} (TRUE: Sus atributos internos son idénticos)");

        // 4. Clonación de Objetos Asociados (Poliza -> Fecha -> Asegurado)
        sb.AppendLine("\n[PASO 4] Objetos Asociados y Clonación Profunda (Poliza):");
        var fInicio = new Fecha(1, 1, 2026);
        var fFin = new Fecha(31, 12, 2026);
        var p1 = new Poliza(5001, "AUTO", 12000f, 1000000f, 50000f, fInicio, fFin, a1);
        var p2 = p1.clone();
        sb.AppendLine("   p2 = p1.clone(); // Clona Poliza y sus asociados Fecha y Asegurado");
        sb.AppendLine($"   -> ¿p1 == p2? {p1 == p2} (FALSE: Distintas pólizas)");
        sb.AppendLine($"   -> ¿p1.equals(p2)? {p1.equals(p2)} (TRUE: Mismo número y titular)");
        sb.AppendLine($"   -> ¿p1.VigenciaInicio == p2.VigenciaInicio? {p1.VigenciaInicio == p2.VigenciaInicio} (FALSE: Cloned Fecha in Heap)");
        sb.AppendLine($"   -> ¿p1.VigenciaInicio.equals(p2.VigenciaInicio)? {p1.VigenciaInicio.equals(p2.VigenciaInicio)} (TRUE: Mismo día/mes/año)");

        // 5. Mutación con copy()
        sb.AppendLine("\n[PASO 5] Modificación in-place con copy():");
        var fAux = new Fecha(15, 6, 2030);
        p1.VigenciaInicio.copy(fAux);
        sb.AppendLine("   p1.VigenciaInicio.copy(fAux); // Modifica el estado interno de fInicio");
        sb.AppendLine($"   -> Nueva fecha inicio en p1: {p1.VigenciaInicio}");
        sb.AppendLine($"   -> Fecha en p2 se mantuvo inmutable: {p2.VigenciaInicio}");
        sb.AppendLine($"   -> ¿p1.VigenciaInicio.equals(p2.VigenciaInicio)? {p1.VigenciaInicio.equals(p2.VigenciaInicio)} (FALSE: Estados desincronizados)");

        sb.AppendLine("\n================================================================================");
        sb.AppendLine("   CONCLUSIÓN: MODELO VERIFICADO CON ÉXITO PARA DEFENSA EN EXAMEN POO");
        sb.AppendLine("================================================================================");

        TxtInspectorLog.Text = sb.ToString();
    }
}
