using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

public class Asegurado : ICopiable<Asegurado>, IClonable<Asegurado>, IIdentificable<int>
{
    private int codigo;
    private string dni, nombre, telefono, email, domicilio;
    private bool activo;

    public int Id => codigo;

    public Asegurado(int c)
    {
        codigo = c;
        dni = "SIN_DNI";
        nombre = "SIN_NOMBRE";
        telefono = "SIN_TELEFONO";
        email = string.Empty;
        domicilio = string.Empty;
        activo = true;
    }

    public Asegurado(int c, string d, string n, string t)
        : this(c, d, n, t, string.Empty, string.Empty) { }

    public Asegurado(int c, string d, string n, string t, string em, string dom)
    {
        codigo = c;
        dni = d;
        nombre = n;
        telefono = t;
        email = em ?? string.Empty;
        domicilio = dom ?? string.Empty;
        activo = true;
    }

    // Propiedades compactas con backing field
    public int Codigo { get => codigo; set => codigo = value; }
    public string Dni { get => dni; set => dni = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Telefono { get => telefono; set => telefono = value; }
    public string Email { get => email; set => email = value; }
    public string Domicilio { get => domicilio; set => domicilio = value; }
    public bool Activo { get => activo; set => activo = value; }

    // Métodos explícitos clásicos (Expression bodies)
    public int getCodigo() => codigo;
    public void setCodigo(int c) => codigo = c;
    public string getDni() => dni;
    public void setDni(string d) => dni = d;
    public string getNombre() => nombre;
    public void setNombre(string n) => nombre = n;
    public string getTelefono() => telefono;
    public void setTelefono(string t) => telefono = t;
    public string getEmail() => email;
    public void setEmail(string e) => email = e;
    public string getDomicilio() => domicilio;
    public void setDomicilio(string d) => domicilio = d;

    // Métodos copy, clone y equals
    public void copy(Asegurado a)
    {
        if (a == null) return;
        codigo = a.codigo;
        dni = a.dni;
        nombre = a.nombre;
        telefono = a.telefono;
        email = a.email;
        domicilio = a.domicilio;
        activo = a.activo;
    }

    public Asegurado clone() => new Asegurado(codigo, dni, nombre, telefono, email, domicilio) { activo = activo };

    public bool equals(Asegurado a) =>
        a != null && codigo == a.codigo && dni == a.dni;

    public override string ToString() => $"[{codigo}] {nombre} (DNI: {dni}, Tel: {telefono})";
}
