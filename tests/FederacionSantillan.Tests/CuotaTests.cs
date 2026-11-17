using FederacionSantillan.Core;

namespace FederacionSantillan.Tests;

public class CuotaTests
{
    [Fact]
    public void EstaVencida_CuotaPendientePasada_RetornaTrue()
    {
        var cuota = new Cuota(1, 10, 1, 25000m, DateTime.Today.AddDays(-5), "PENDIENTE");

        Assert.True(cuota.estaVencida());
        Assert.Equal(5, cuota.diasDeAtraso());
    }

    [Fact]
    public void EstaPorVencer_DentroDeQuinceDias_RetornaTrue()
    {
        var cuota = new Cuota(2, 10, 2, 25000m, DateTime.Today.AddDays(7), "PENDIENTE");

        Assert.True(cuota.estaPorVencer(15));
        Assert.False(cuota.estaVencida());
    }

    [Fact]
    public void RegistrarPago_CuotaPendiente_CambiaEstadoAPagada()
    {
        var cuota = new Cuota(3, 10, 1, 25000m, DateTime.Today, "PENDIENTE");

        bool resultado = cuota.registrarPago("Transferencia");

        Assert.True(resultado);
        Assert.Equal("PAGADA", cuota.Estado);
        Assert.NotNull(cuota.FechaPago);
        Assert.False(cuota.registrarPago("Efectivo"));
    }

    [Fact]
    public void Clone_CreaInstanciaIndependienteConMismoEstado()
    {
        var original = new Cuota(1, 10, 1, 30000m, DateTime.Today, "PENDIENTE");
        var clon = original.clone();

        Assert.NotSame(original, clon);
        Assert.True(original.equals(clon));
    }
}
