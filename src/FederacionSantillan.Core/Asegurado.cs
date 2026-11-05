using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

public class Asegurado : ICopiable<Asegurado>, IClonable<Asegurado>, IIdentificable<int>
{
    private int codigo;
    private string dni, nombre, telefono;

    public int Id => codigo;

    public Asegurado(int c)
    {
        codigo = c;
        dni = "SIN_DNI";
        nombre = "SIN_NOMBRE";
        telefono = "SIN_TELEFONO";
    }

    public Asegurado(int c, string d, string n, string t)
    {
        codigo = c;
        dni = d;
        nombre = n;
        telefono = t;
    }

    // Propiedades compactas con backing field
    public int Codigo { get => codigo; set => codigo = value; }
    public string Dni { get => dni; set => dni = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Telefono { get => telefono; set => telefono = value; }

    // Métodos explícitos clásicos
    public int getCodigo() => codigo;
    public void setCodigo(int c) => codigo = c;
    public string getDni() => dni;
    public void setDni(string d) => dni = d;
    public string getNombre() => nombre;
    public void setNombre(string n) => nombre = n;
    public string getTelefono() => telefono;
    public void setTelefono(string t) => telefono = t;

    // Tríada Sagrada
    public void copy(Asegurado a)
    {
        if (a == null) return;
        codigo = a.codigo;
        dni = a.dni;
        nombre = a.nombre;
        telefono = a.telefono;
    }

    public Asegurado clone() => new Asegurado(codigo, dni, nombre, telefono);

    public bool equals(Asegurado a) =>
        a != null && codigo == a.codigo && dni == a.dni;

    public override string ToString() => $"[{codigo}] {nombre} (DNI: {dni}, Tel: {telefono})";
}
