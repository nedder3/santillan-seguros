namespace FederacionSantillan.Core;

public class Siniestro
{
    private int numero;
    private string descripcion, estado;
    private float montoReclamado, montoLiquidado;
    private Fecha fechaOcurrencia;
    private Poliza poliza;
    private Peritaje peritaje;

    public Siniestro(int n, string desc, float monto, Fecha fecha, Poliza p)
    {
        numero = n;
        descripcion = desc;
        montoReclamado = monto;
        montoLiquidado = 0f;
        estado = "DENUNCIADO";
        fechaOcurrencia = fecha;
        poliza = p;
        peritaje = null;
    }

    // Propiedades
    public int Numero { get => numero; set => numero = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public string Estado { get => estado; set => estado = value; }
    public float MontoReclamado { get => montoReclamado; set => montoReclamado = value; }
    public float MontoLiquidado { get => montoLiquidado; set => montoLiquidado = value; }
    public Fecha FechaOcurrencia { get => fechaOcurrencia; set => fechaOcurrencia = value; }
    public Poliza Poliza { get => poliza; set => poliza = value; }
    public Peritaje Peritaje { get => peritaje; set => peritaje = value; }

    // Métodos explícitos
    public int getNumero() => numero;
    public void setNumero(int n) => numero = n;
    public string getDescripcion() => descripcion;
    public void setDescripcion(string d) => descripcion = d;
    public string getEstado() => estado;
    public void setEstado(string e) => estado = e;
    public float getMontoReclamado() => montoReclamado;
    public void setMontoReclamado(float m) => montoReclamado = m;
    public float getMontoLiquidado() => montoLiquidado;
    public void setMontoLiquidado(float m) => montoLiquidado = m;
    public Fecha getFechaOcurrencia() => fechaOcurrencia;
    public void setFechaOcurrencia(Fecha f) => fechaOcurrencia = f;
    public Poliza getPoliza() => poliza;
    public void setPoliza(Poliza p) => poliza = p;
    public Peritaje getPeritaje() => peritaje;
    public void setPeritaje(Peritaje p) => peritaje = p;

    // Tríada Sagrada EN PROFUNDIDAD
    public void copy(Siniestro s)
    {
        if (s == null) return;
        numero = s.numero;
        descripcion = s.descripcion;
        estado = s.estado;
        montoReclamado = s.montoReclamado;
        montoLiquidado = s.montoLiquidado;
        if (fechaOcurrencia != null && s.fechaOcurrencia != null) fechaOcurrencia.copy(s.fechaOcurrencia);
        if (poliza != null && s.poliza != null) poliza.copy(s.poliza);
        if (peritaje != null && s.peritaje != null)
            peritaje.copy(s.peritaje);
        else if (s.peritaje != null)
            peritaje = s.peritaje.clone();
    }

    public Siniestro clone()
    {
        var nuevo = new Siniestro(
            numero,
            descripcion,
            montoReclamado,
            fechaOcurrencia != null ? fechaOcurrencia.clone() : null,
            poliza != null ? poliza.clone() : null
        );
        nuevo.estado = estado;
        nuevo.montoLiquidado = montoLiquidado;
        if (peritaje != null) nuevo.peritaje = peritaje.clone();
        return nuevo;
    }

    public bool equals(Siniestro s) =>
        s != null && numero == s.numero && (poliza == null ? s.poliza == null : poliza.equals(s.poliza));

    // Métodos de negocio
    public bool registrarPeritaje(Peritaje p)
    {
        if (p == null) return false;
        peritaje = p;
        estado = "EN_PERITAJE";
        return true;
    }

    public bool liquidar()
    {
        if (poliza == null || peritaje == null || !peritaje.DanosVerificados)
        {
            estado = "RECHAZADO";
            montoLiquidado = 0f;
            return false;
        }

        montoLiquidado = poliza.liquidarIndemnizacion(peritaje.MontoPresupuestado);
        estado = montoLiquidado > 0 ? "LIQUIDADO" : "RECHAZADO";
        return montoLiquidado > 0;
    }

    public override string ToString() =>
        $"Siniestro #{numero} - Estado: {estado} - Reclamado: ${montoReclamado:N2} | Liquidado: ${montoLiquidado:N2}";
}
