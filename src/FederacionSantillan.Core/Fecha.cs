namespace FederacionSantillan.Core;

public class Fecha
{
    private int dia, mes, ano;

    public Fecha(int d, int m, int a)
    {
        dia = d;
        mes = m;
        ano = a;
    }

    // Propiedades compactas con backing field
    public int Dia { get => dia; set => dia = value; }
    public int Mes { get => mes; set => mes = value; }
    public int Ano { get => ano; set => ano = value; }

    // Métodos explícitos clásicos
    public int getDia() => dia;
    public void setDia(int d) => dia = d;
    public int getMes() => mes;
    public void setMes(int m) => mes = m;
    public int getAno() => ano;
    public void setAno(int a) => ano = a;

    // Tríada Sagrada
    public void copy(Fecha f)
    {
        if (f == null) return;
        dia = f.dia;
        mes = f.mes;
        ano = f.ano;
    }

    public Fecha clone() => new Fecha(dia, mes, ano);

    public bool equals(Fecha f) =>
        f != null && dia == f.dia && mes == f.mes && ano == f.ano;

    // Métodos de negocio
    public bool esValida() =>
        ano > 1900 && mes >= 1 && mes <= 12 && dia >= 1 && dia <= 31;

    public override string ToString() => $"{dia:D2}/{mes:D2}/{ano}";
}
