using Xunit;
using FederacionSantillan.Core;
using FederacionSantillan.Core.Services;
using FederacionSantillan.Core.Services.Strategies;

namespace FederacionSantillan.Tests;

/// <summary>
/// Pruebas FIRST que verifican SOLID:
/// Single Responsibility (LiquidacionService solo calcula e imputa),
/// Open/Closed (estrategias por ramo extensibles sin modificar el servicio).
/// </summary>
public class SolidStrategyTests
{
    [Fact]
    public void Test_SOLID_Estrategia_Auto_Deduce_Franquicia()
    {
        var estrategia = new LiquidacionAutoStrategy();
        // Cobertura: $1,000,000, Deducible: $50,000, Daño peritado: $200,000
        // Neto esperado: $200,000 - $50,000 = $150,000
        float neto = estrategia.CalcularIndemnizacion(200000f, 1000000f, 50000f);
        Assert.Equal(150000f, neto);

        // Si el daño es menor al deducible, neto = 0
        float subDeducible = estrategia.CalcularIndemnizacion(30000f, 1000000f, 50000f);
        Assert.Equal(0f, subDeducible);
    }

    [Fact]
    public void Test_SOLID_Estrategia_Vida_Sin_Deducible()
    {
        var estrategia = new LiquidacionVidaStrategy();
        // En seguro de vida se paga el monto íntegro hasta el límite
        float liquidado = estrategia.CalcularIndemnizacion(500000f, 400000f, 0f);
        Assert.Equal(400000f, liquidado); // Tope de cobertura
    }

    [Fact]
    public void Test_SOLID_LiquidacionService_Workflow()
    {
        var service = new LiquidacionService();
        var inicio = new Fecha(1, 1, 2026);
        var fin = new Fecha(31, 12, 2026);
        var titular = new Asegurado(101, "20-11223344-9", "Federico", "11-4455-6677");
        var poliza = new Poliza(1001, "AUTO", 15000f, 800000f, 30000f, inicio, fin, titular);

        var siniestro = new Siniestro(901, "Choque trasero", 60000f, inicio, poliza);

        // Sin peritaje no puede liquidarse y permanece en su estado actual (DENUNCIADO)
        Assert.False(service.LiquidarSiniestro(siniestro));
        Assert.Equal("DENUNCIADO", siniestro.Estado);

        // Con peritaje validado: $50,000 daño - $30,000 deducible = $20,000
        var peritaje = new Peritaje(501, "Perito Perez", 50000f, true, "Daños conformes.");
        siniestro.registrarPeritaje(peritaje);

        Assert.True(service.LiquidarSiniestro(siniestro));
        Assert.Equal("LIQUIDADO", siniestro.Estado);
        Assert.Equal(20000f, siniestro.MontoLiquidado);
    }
}
