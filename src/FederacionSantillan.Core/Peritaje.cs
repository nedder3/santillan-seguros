using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core;

public class Peritaje : ICopiable<Peritaje>, IClonable<Peritaje>, IIdentificable<int>
{
    private int codigo;
    private string perito, observaciones;
    private float montoPresupuestado;
    private bool danosVerificados;

    public int Id => codigo;

    public Peritaje(int c)
    {
        codigo = c;
        perito = "PERITO_ASIGNADO";
        observaciones = "";
        montoPresupuestado = 0f;
        danosVerificados = false;
    }

    public Peritaje(int c, string p, float m, bool d, string o)
    {
        codigo = c;
        perito = p;
        montoPresupuestado = m;
        danosVerificados = d;
        observaciones = o;
    }

    // Propiedades
    public int Codigo { get => codigo; set => codigo = value; }
    public string Perito { get => perito; set => perito = value; }
    public string Observaciones { get => observaciones; set => observaciones = value; }
    public float MontoPresupuestado { get => montoPresupuestado; set => montoPresupuestado = value; }
    public bool DanosVerificados { get => danosVerificados; set => danosVerificados = value; }

    // Métodos explícitos
    public int getCodigo() => codigo;
    public void setCodigo(int c) => codigo = c;
    public string getPerito() => perito;
    public void setPerito(string p) => perito = p;
    public float getMontoPresupuestado() => montoPresupuestado;
    public void setMontoPresupuestado(float m) => montoPresupuestado = m;
    public bool getDanosVerificados() => danosVerificados;
    public void setDanosVerificados(bool d) => danosVerificados = d;
    public string getObservaciones() => observaciones;
    public void setObservaciones(string o) => observaciones = o;

    // Tríada Sagrada
    public void copy(Peritaje p)
    {
        if (p == null) return;
        codigo = p.codigo;
        perito = p.perito;
        montoPresupuestado = p.montoPresupuestado;
        danosVerificados = p.danosVerificados;
        observaciones = p.observaciones;
    }

    public Peritaje clone() =>
        new Peritaje(codigo, perito, montoPresupuestado, danosVerificados, observaciones);

    public bool equals(Peritaje p) =>
        p != null && codigo == p.codigo && montoPresupuestado == p.montoPresupuestado && danosVerificados == p.danosVerificados;

    public override string ToString() =>
        $"Peritaje #{codigo} por {perito} - Daños: {(danosVerificados ? "VERIFICADOS" : "PENDIENTE")} (${montoPresupuestado:N2})";
}
