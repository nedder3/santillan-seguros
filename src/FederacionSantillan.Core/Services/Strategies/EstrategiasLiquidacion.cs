using System;
using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core.Services.Strategies;

public class LiquidacionAutoStrategy : ILiquidadorStrategy
{
    public string Ramo => "AUTO";

    // O(1) tiempo, O(1) espacio
    public float CalcularIndemnizacion(float montoPeritado, float coberturaMax, float deducible)
    {
        if (montoPeritado <= deducible) return 0f;
        float neto = montoPeritado - deducible;
        return Math.Min(neto, coberturaMax);
    }
}

public class LiquidacionHogarStrategy : ILiquidadorStrategy
{
    public string Ramo => "HOGAR";

    // O(1) tiempo, O(1) espacio
    public float CalcularIndemnizacion(float montoPeritado, float coberturaMax, float deducible)
    {
        if (montoPeritado <= deducible) return 0f;
        float neto = montoPeritado - deducible;
        return Math.Min(neto, coberturaMax);
    }
}

public class LiquidacionVidaStrategy : ILiquidadorStrategy
{
    public string Ramo => "VIDA";

    // En vida no aplica deducible comercial
    // O(1) tiempo, O(1) espacio
    public float CalcularIndemnizacion(float montoPeritado, float coberturaMax, float deducible)
    {
        return Math.Min(montoPeritado, coberturaMax);
    }
}
