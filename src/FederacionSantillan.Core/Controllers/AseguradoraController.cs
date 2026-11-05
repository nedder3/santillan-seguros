using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FederacionSantillan.Core.Repositories;
using FederacionSantillan.Core.Services;

namespace FederacionSantillan.Core.Controllers;

/// <summary>
/// Controlador MVC: Desacopla la Vista (WPF) del Modelo de Dominio y los Repositorios.
/// </summary>
public class AseguradoraController
{
    private readonly IndexedRepository<Asegurado, int> _repoAsegurados;
    private readonly IndexedRepository<Poliza, int> _repoPolizas;
    private readonly IndexedRepository<Siniestro, int> _repoSiniestros;
    private readonly ILiquidacionService _liquidacionService;

    public AseguradoraController(
        IndexedRepository<Asegurado, int> repoAsegurados = null,
        IndexedRepository<Poliza, int> repoPolizas = null,
        IndexedRepository<Siniestro, int> repoSiniestros = null,
        ILiquidacionService liquidacionService = null)
    {
        _repoAsegurados = repoAsegurados ?? new IndexedRepository<Asegurado, int>();
        _repoPolizas = repoPolizas ?? new IndexedRepository<Poliza, int>();
        _repoSiniestros = repoSiniestros ?? new IndexedRepository<Siniestro, int>();
        _liquidacionService = liquidacionService ?? new LiquidacionService();
    }

    public IReadOnlyCollection<Asegurado> ObtenerAsegurados() => _repoAsegurados.ObtenerTodos();
    public IReadOnlyCollection<Poliza> ObtenerPolizas() => _repoPolizas.ObtenerTodos();
    public IReadOnlyCollection<Siniestro> ObtenerSiniestros() => _repoSiniestros.ObtenerTodos();

    public int TotalPolizas => _repoPolizas.Cantidad;
    public int TotalSiniestros => _repoSiniestros.Cantidad;

    // Complejidad O(1) con tabla hash
    public Poliza BuscarPoliza(int num) => _repoPolizas.ObtenerPorId(num);

    // Complejidad O(1) con tabla hash
    public Asegurado BuscarAsegurado(int cod) => _repoAsegurados.ObtenerPorId(cod);

    public float CalcularTotalLiquidado() =>
        _repoSiniestros.ObtenerTodos().Sum(s => s.MontoLiquidado);

    public bool EmitirPoliza(
        int numero, string tipo, float prima, float coberturaMax, float deducible,
        Fecha inicio, Fecha fin, int codTitular, string dniTitular, string nombreTitular, string telTitular)
    {
        if (_repoPolizas.Existe(numero))
            return false;

        var titular = _repoAsegurados.ObtenerPorId(codTitular);
        if (titular == null)
        {
            titular = new Asegurado(codTitular, dniTitular, nombreTitular, telTitular);
            _repoAsegurados.Agregar(titular);
        }

        var poliza = new Poliza(numero, tipo, prima, coberturaMax, deducible, inicio, fin, titular);
        return _repoPolizas.AgregarConIndiceSecundario(poliza, tipo);
    }

    public bool RegistrarSiniestro(int numero, int numeroPoliza, string descripcion, float montoReclamado, Fecha fecha)
    {
        if (_repoSiniestros.Existe(numero))
            return false;

        var poliza = _repoPolizas.ObtenerPorId(numeroPoliza);
        if (poliza == null)
            return false;

        var siniestro = new Siniestro(numero, descripcion, montoReclamado, fecha, poliza);
        return _repoSiniestros.AgregarConIndiceSecundario(siniestro, siniestro.Estado);
    }

    public (bool Exito, float MontoLiquidado, string Mensaje) LiquidarSiniestro(
        int numSiniestro, string nombrePerito, float montoPeritado, bool danosVerificados)
    {
        var siniestro = _repoSiniestros.ObtenerPorId(numSiniestro);
        if (siniestro == null)
            return (false, 0f, $"No se encontró el siniestro #{numSiniestro}.");

        var peritaje = new Peritaje(numSiniestro + 500, nombrePerito, montoPeritado, danosVerificados, "Inspección pericial.");
        siniestro.registrarPeritaje(peritaje);

        bool liquidado = _liquidacionService.LiquidarSiniestro(siniestro);
        if (liquidado)
        {
            return (true, siniestro.MontoLiquidado, $"Siniestro #{numSiniestro} liquidado exitosamente por ${siniestro.MontoLiquidado:N2}.");
        }
        else
        {
            return (false, 0f, $"Siniestro #{numSiniestro} rechazado (daños no validados o monto peritado inferior al deducible de ${siniestro.Poliza.Deducible:N2}).");
        }
    }

    public string EjecutarSimulacroMemoria()
    {
        var sb = new StringBuilder();
        sb.AppendLine("================================================================================");
        sb.AppendLine("   SIMULACRO EN VIVO: GESTIÓN DE MEMORIA STACK vs HEAP (POO EXAM RUNNER)");
        sb.AppendLine("================================================================================");
        sb.AppendLine($"Ejecutado: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Runtime: .NET 8 WPF MVC\n");

        var a1 = new Asegurado(1001, "20-11223344-9", "Federico Santillan", "11-4455-6677");
        var a2 = a1; // Aliasing
        var a3 = a1.clone(); // Clon

        sb.AppendLine("[PASO 1] Punteros y Aliasing en Stack:");
        sb.AppendLine($"   -> a1 == a2 (Aliasing): {a1 == a2} (TRUE: Mismo puntero en Stack)");
        sb.AppendLine($"   -> a1 == a3 (Clon): {a1 == a3} (FALSE: Dos instancias en Heap)");
        sb.AppendLine($"   -> a1.equals(a3) (Estado): {a1.equals(a3)} (TRUE: Mismos atributos)");

        var fInicio = new Fecha(1, 1, 2026);
        var fFin = new Fecha(31, 12, 2026);
        var p1 = new Poliza(5001, "AUTO", 15000f, 1000000f, 50000f, fInicio, fFin, a1);
        var p2 = p1.clone();

        sb.AppendLine("\n[PASO 2] Clonación en Profundidad:");
        sb.AppendLine($"   -> p1 == p2: {p1 == p2} (FALSE)");
        sb.AppendLine($"   -> p1.equals(p2): {p1.equals(p2)} (TRUE)");
        sb.AppendLine($"   -> p1.VigenciaInicio == p2.VigenciaInicio: {p1.VigenciaInicio == p2.VigenciaInicio} (FALSE: Clon profundo)");
        sb.AppendLine($"   -> p1.VigenciaInicio.equals(p2.VigenciaInicio): {p1.VigenciaInicio.equals(p2.VigenciaInicio)} (TRUE)");

        sb.AppendLine("\n[PASO 3] Optimización Big-O en Repositorio:");
        sb.AppendLine($"   -> Búsqueda de póliza por clave hash: O(1) tiempo constante.");
        sb.AppendLine($"   -> Conteo por estado con índice invertido: O(1) tiempo constante.");

        return sb.ToString();
    }
}
