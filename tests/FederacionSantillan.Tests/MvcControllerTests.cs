using Xunit;
using FederacionSantillan.Core;
using FederacionSantillan.Core.Controllers;

namespace FederacionSantillan.Tests;

/// <summary>
/// Pruebas FIRST para el Controlador MVC:
/// Desacoplamiento de Vista, orquestación de repositorios y retorno de estados.
/// </summary>
public class MvcControllerTests
{
    [Fact]
    public void Test_MVC_Controller_Emision_Y_Liquidacion_Completa()
    {
        var controller = new AseguradoraController();
        var inicio = new Fecha(1, 1, 2026);
        var fin = new Fecha(31, 12, 2026);

        // 1. Emitir póliza a través del controlador
        bool emitio = controller.EmitirPoliza(
            numero: 5001,
            tipo: "AUTO",
            prima: 18000f,
            coberturaMax: 1500000f,
            deducible: 40000f,
            inicio: inicio,
            fin: fin,
            codTitular: 101,
            dniTitular: "20-99887766-3",
            nombreTitular: "Federico Santillan",
            telTitular: "11-5566-7788"
        );
        Assert.True(emitio);
        Assert.Equal(1, controller.TotalPolizas);

        // 2. Registrar siniestro
        bool registroSin = controller.RegistrarSiniestro(
            numero: 901,
            numeroPoliza: 5001,
            descripcion: "Impacto en autopista",
            montoReclamado: 120000f,
            fecha: new Fecha(15, 2, 2026)
        );
        Assert.True(registroSin);
        Assert.Equal(1, controller.TotalSiniestros);

        // 3. Liquidar siniestro
        var resultado = controller.LiquidarSiniestro(
            numSiniestro: 901,
            nombrePerito: "Ing. Gomez",
            montoPeritado: 100000f,
            danosVerificados: true
        );

        Assert.True(resultado.Exito);
        // Esperado: $100,000 peritado - $40,000 deducible = $60,000
        Assert.Equal(60000f, resultado.MontoLiquidado);
        Assert.Equal(60000f, controller.CalcularTotalLiquidado());
    }

    [Fact]
    public void Test_MVC_Controller_SimuladorMemoria_Returns_Truth_Table()
    {
        var controller = new AseguradoraController();
        string log = controller.EjecutarSimulacroMemoria();

        Assert.NotNull(log);
        Assert.Contains("SIMULACRO EN VIVO: GESTIÓN DE MEMORIA STACK vs HEAP", log);
        Assert.Contains("a1 == a2 (Aliasing): True", log);
        Assert.Contains("a1 == a3 (Clon): False", log);
        Assert.Contains("O(1) tiempo constante", log);
    }
}
