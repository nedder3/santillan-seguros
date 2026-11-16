using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

/// <summary>
/// Entidad Paramétrica: Ramo de Seguro (Clasificación SSN / Federación Patronal).
/// </summary>
public class Ramo : ICopiable<Ramo>, IClonable<Ramo>, IIdentificable<int>
{
    private int idRamo;
    private string codigo, nombre, descripcion;

    public int Id => idRamo;

    public Ramo(int id, string cod, string nom, string desc)
    {
        idRamo = id;
        codigo = cod;
        nombre = nom;
        descripcion = desc;
    }

    public int IdRamo { get => idRamo; set => idRamo = value; }
    public string Codigo { get => codigo; set => codigo = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }

    public int obtenerIdRamo() => idRamo;
    public void establecerIdRamo(int id) => idRamo = id;
    public string obtenerCodigo() => codigo;
    public void establecerCodigo(string c) => codigo = c;
    public string obtenerNombre() => nombre;
    public void establecerNombre(string n) => nombre = n;
    public string obtenerDescripcion() => descripcion;
    public void establecerDescripcion(string d) => descripcion = d;

    public void copy(Ramo r)
    {
        if (r == null) return;
        idRamo = r.idRamo;
        codigo = r.codigo;
        nombre = r.nombre;
        descripcion = r.descripcion;
    }

    public Ramo clone() => new Ramo(idRamo, codigo, nombre, descripcion);

    public bool equals(Ramo r) =>
        r != null && idRamo == r.idRamo && codigo == r.codigo && nombre == r.nombre;

    public override string ToString() => $"{nombre} ({codigo})";
}
