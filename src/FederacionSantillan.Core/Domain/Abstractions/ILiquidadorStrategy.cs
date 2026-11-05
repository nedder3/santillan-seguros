namespace FederacionSantillan.Core.Domain.Abstractions;

/// <summary>
/// Estrategia abierta a extensión (Open/Closed) para liquidación de indemnizaciones según ramo asegurador.
/// </summary>
public interface ILiquidadorStrategy
{
    string Ramo { get; }
    
    /// <summary>
    /// Calcula el importe neto indemnizable respetando límites y deducibles.
    /// Complejidad temporal: O(1).
    /// </summary>
    float CalcularIndemnizacion(float montoPeritado, float coberturaMax, float deducible);
}
