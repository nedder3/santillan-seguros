using Xunit;
using FederacionSantillan.Core;

namespace FederacionSantillan.Tests;

public class TriadaSagradaTests
{
    [Fact]
    public void TestFecha_Clone_Copy_Equals()
    {
        Fecha f1 = new Fecha(10, 5, 2026);
        Fecha f2 = f1.clone(); // Nuevo objeto en Heap con mismo estado

        // Examen: f1 == f2 es FALSE (referencias distintas)
        Assert.False(f1 == f2);
        // Examen: f1.equals(f2) es TRUE (mismo estado)
        Assert.True(f1.equals(f2));

        Fecha f3 = new Fecha(1, 1, 2020);
        f3.copy(f1); // Sobrescribe estado interno de f3

        Assert.False(f3 == f1); // Siguen siendo referencias distintas
        Assert.True(f3.equals(f1)); // Pero con idéntico valor
    }

    [Fact]
    public void TestAsegurado_Aliasing_And_Cloning()
    {
        Asegurado a1 = new Asegurado(1001, "30-11223344-9", "Federico Santillan", "11-4455-6677");
        Asegurado a2 = a1; // Asignación de referencia (Alias)

        // Mismo puntero en Stack
        Assert.True(a1 == a2);
        Assert.True(a1.equals(a2));

        Asegurado a3 = a1.clone();
        Assert.False(a1 == a3); // Distinta instancia
        Assert.True(a1.equals(a3)); // Mismo estado
    }

    [Fact]
    public void TestPoliza_DeepClone_And_Liquidar()
    {
        Fecha inicio = new Fecha(1, 1, 2026);
        Fecha fin = new Fecha(31, 12, 2026);
        Asegurado titular = new Asegurado(101, "20-99887766-4", "Carlos Lopez", "11-2233-4455");

        // Poliza Auto: Cobertura $1,000,000, Deducible $50,000
        Poliza p1 = new Poliza(5001, "AUTO", 12000f, 1000000f, 50000f, inicio, fin, titular);
        Poliza p2 = p1.clone();

        Assert.False(p1 == p2);
        Assert.True(p1.equals(p2));
        // Deep clone: los objetos asociados son instancias nuevas en memoria
        Assert.False(p1.VigenciaInicio == p2.VigenciaInicio);
        Assert.True(p1.VigenciaInicio.equals(p2.VigenciaInicio));

        // Liquidación: Daño peritado $350,000 - Deducible $50,000 = $300,000
        float indemnizacion = p1.liquidarIndemnizacion(350000f);
        Assert.Equal(300000f, indemnizacion);

        // Si el daño es menor o igual al deducible ($40,000 <= $50,000), indemnización = $0
        float bajoDeducible = p1.liquidarIndemnizacion(40000f);
        Assert.Equal(0f, bajoDeducible);
    }

    [Fact]
    public void TestSiniestro_CicloCompleto_Peritaje_Y_Liquidacion()
    {
        Fecha fSiniestro = new Fecha(15, 3, 2026);
        Fecha inicio = new Fecha(1, 1, 2026);
        Fecha fin = new Fecha(31, 12, 2026);
        Asegurado titular = new Asegurado(202, "27-44332211-8", "Maria Santillan", "11-9988-7766");
        Poliza poliza = new Poliza(7001, "HOGAR", 8500f, 500000f, 20000f, inicio, fin, titular);

        Siniestro s = new Siniestro(901, "Inundación por rotura de caño", 80000f, fSiniestro, poliza);
        Assert.Equal("DENUNCIADO", s.Estado);

        // Asignación de peritaje
        Peritaje peri = new Peritaje(401, "Ing. Gomez", 75000f, true, "Daños comprobados en parquet y mampostería.");
        s.registrarPeritaje(peri);
        Assert.Equal("EN_PERITAJE", s.Estado);

        // Liquidación: $75,000 peritado - $20,000 deducible = $55,000
        bool liquidado = s.liquidar();
        Assert.True(liquidado);
        Assert.Equal("LIQUIDADO", s.Estado);
        Assert.Equal(55000f, s.MontoLiquidado);
    }
}
