namespace FederacionSantillan.Core;

public class Poliza
{
    private int numero;
    private string tipo;
    private float prima, coberturaMax, deducible;
    private bool activa;
    private Fecha vigenciaInicio, vigenciaFin;
    private Asegurado titular;

    public Poliza(int n, string t, float p, float c, float d, Fecha vi, Fecha vf, Asegurado a)
    {
        numero = n;
        tipo = t;
        prima = p;
        coberturaMax = c;
        deducible = d;
        activa = true;
        vigenciaInicio = vi;
        vigenciaFin = vf;
        titular = a;
    }

    // Propiedades
    public int Numero { get => numero; set => numero = value; }
    public string Tipo { get => tipo; set => tipo = value; }
    public float Prima { get => prima; set => prima = value; }
    public float CoberturaMax { get => coberturaMax; set => coberturaMax = value; }
    public float Deducible { get => deducible; set => deducible = value; }
    public bool Activa { get => activa; set => activa = value; }
    public Fecha VigenciaInicio { get => vigenciaInicio; set => vigenciaInicio = value; }
    public Fecha VigenciaFin { get => vigenciaFin; set => vigenciaFin = value; }
    public Asegurado Titular { get => titular; set => titular = value; }

    // Métodos explícitos
    public int getNumero() => numero;
    public void setNumero(int n) => numero = n;
    public string getTipo() => tipo;
    public void setTipo(string t) => tipo = t;
    public float getPrima() => prima;
    public void setPrima(float p) => prima = p;
    public float getCoberturaMax() => coberturaMax;
    public void setCoberturaMax(float c) => coberturaMax = c;
    public float getDeducible() => deducible;
    public void setDeducible(float d) => deducible = d;
    public bool getActiva() => activa;
    public void setActiva(bool a) => activa = a;
    public Fecha getVigenciaInicio() => vigenciaInicio;
    public void setVigenciaInicio(Fecha f) => vigenciaInicio = f;
    public Fecha getVigenciaFin() => vigenciaFin;
    public void setVigenciaFin(Fecha f) => vigenciaFin = f;
    public Asegurado getTitular() => titular;
    public void setTitular(Asegurado a) => titular = a;

    // Tríada Sagrada EN PROFUNDIDAD
    public void copy(Poliza p)
    {
        if (p == null) return;
        numero = p.numero;
        tipo = p.tipo;
        prima = p.prima;
        coberturaMax = p.coberturaMax;
        deducible = p.deducible;
        activa = p.activa;
        if (vigenciaInicio != null && p.vigenciaInicio != null) vigenciaInicio.copy(p.vigenciaInicio);
        if (vigenciaFin != null && p.vigenciaFin != null) vigenciaFin.copy(p.vigenciaFin);
        if (titular != null && p.titular != null) titular.copy(p.titular);
    }

    public Poliza clone() =>
        new Poliza(
            numero,
            tipo,
            prima,
            coberturaMax,
            deducible,
            vigenciaInicio != null ? vigenciaInicio.clone() : null,
            vigenciaFin != null ? vigenciaFin.clone() : null,
            titular != null ? titular.clone() : null
        ) { activa = activa };

    public bool equals(Poliza p) =>
        p != null &&
        numero == p.numero &&
        tipo == p.tipo &&
        coberturaMax == p.coberturaMax &&
        (titular == null ? p.titular == null : titular.equals(p.titular));

    // Métodos de negocio
    public float liquidarIndemnizacion(float montoPeritado)
    {
        if (!activa || montoPeritado <= deducible) return 0f;
        float neto = montoPeritado - deducible;
        return neto > coberturaMax ? coberturaMax : neto;
    }

    public override string ToString() =>
        $"Póliza #{numero} ({tipo}) - Titular: {titular?.Nombre ?? "N/A"} - Cubre hasta: ${coberturaMax:N2} (Deducible: ${deducible:N2})";
}
