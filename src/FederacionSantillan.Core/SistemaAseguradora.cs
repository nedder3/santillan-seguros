using System.Collections.Generic;
using System.Linq;

namespace FederacionSantillan.Core;

public class SistemaAseguradora
{
    private string nombre;
    private List<Asegurado> asegurados;
    private List<Poliza> polizas;
    private List<Siniestro> siniestros;

    public SistemaAseguradora(string n)
    {
        nombre = n;
        asegurados = new List<Asegurado>();
        polizas = new List<Poliza>();
        siniestros = new List<Siniestro>();
    }

    public string Nombre { get => nombre; set => nombre = value; }
    public List<Asegurado> Asegurados => asegurados;
    public List<Poliza> Polizas => polizas;
    public List<Siniestro> Siniestros => siniestros;

    public bool agregarAsegurado(Asegurado a)
    {
        if (a == null || asegurados.Any(x => x.Codigo == a.Codigo || x.Dni == a.Dni))
            return false;
        asegurados.Add(a);
        return true;
    }

    public bool emitirPoliza(Poliza p)
    {
        if (p == null || polizas.Any(x => x.Numero == p.Numero))
            return false;
        polizas.Add(p);
        return true;
    }

    public bool registrarSiniestro(Siniestro s)
    {
        if (s == null || siniestros.Any(x => x.Numero == s.Numero))
            return false;
        siniestros.Add(s);
        return true;
    }

    public Poliza buscarPoliza(int num) => polizas.FirstOrDefault(p => p.Numero == num);

    public Asegurado buscarAsegurado(int cod) => asegurados.FirstOrDefault(a => a.Codigo == cod);

    public float calcularTotalLiquidaciones() => siniestros.Sum(s => s.MontoLiquidado);

    public int contarPorEstado(string estado) =>
        siniestros.Count(s => s.Estado.Equals(estado, System.StringComparison.OrdinalIgnoreCase));
}
