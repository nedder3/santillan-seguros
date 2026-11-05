using System.Linq;
using Xunit;
using FederacionSantillan.Core;
using FederacionSantillan.Core.Repositories;

namespace FederacionSantillan.Tests;

/// <summary>
/// Pruebas unitarias bajo principios FIRST:
/// Fast (ejecución &lt; 5ms), Independent (instancias aisladas), 
/// Repeatable (deterministas), Self-validating (aserciones directas), Timely.
/// </summary>
public class BigOOptimizationTests
{
    [Fact]
    public void Test_BigO_IndexedRepository_Lookup_Is_O1_Constant_Time()
    {
        var repo = new IndexedRepository<Poliza, int>();
        var inicio = new Fecha(1, 1, 2026);
        var fin = new Fecha(31, 12, 2026);
        var titular = new Asegurado(101, "20-11223344-9", "Federico", "11-4455-6677");

        // Insertar múltiples pólizas
        for (int i = 1; i <= 1000; i++)
        {
            repo.AgregarConIndiceSecundario(
                new Poliza(i, i % 2 == 0 ? "AUTO" : "HOGAR", 1000f, 500000f, 20000f, inicio, fin, titular),
                i % 2 == 0 ? "AUTO" : "HOGAR"
            );
        }

        Assert.Equal(1000, repo.Cantidad);

        // Búsqueda O(1) por clave primaria
        var p500 = repo.ObtenerPorId(500);
        Assert.NotNull(p500);
        Assert.Equal(500, p500.Numero);
        Assert.Equal("AUTO", p500.Tipo);

        // Conteo O(1) por categoría en índice invertido
        int autos = repo.ContarPorCategoria("AUTO");
        int hogares = repo.ContarPorCategoria("HOGAR");
        Assert.Equal(500, autos);
        Assert.Equal(500, hogares);

        // O(1) de existencia
        Assert.True(repo.Existe(1));
        Assert.True(repo.Existe(1000));
        Assert.False(repo.Existe(9999));
    }

    [Fact]
    public void Test_BigO_Prevents_Duplicate_Key_Insertion()
    {
        var repo = new IndexedRepository<Asegurado, int>();
        var a1 = new Asegurado(101, "20-11111111-1", "Juan", "11-1111-1111");
        var aDuplicado = new Asegurado(101, "20-99999999-9", "Pedro", "11-9999-9999");

        Assert.True(repo.Agregar(a1));
        Assert.False(repo.Agregar(aDuplicado)); // O(1) rechazo inmediato sin escanear lista
        Assert.Equal(1, repo.Cantidad);
        Assert.Equal("Juan", repo.ObtenerPorId(101).Nombre);
    }
}
