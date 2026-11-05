using System;
using System.Collections.Generic;
using FederacionSantillan.Core.Domain.Abstractions;
using FederacionSantillan.Core.Services.Strategies;

namespace FederacionSantillan.Core.Services;

public interface ILiquidacionService
{
    bool LiquidarSiniestro(Siniestro siniestro);
}

public class LiquidacionService : ILiquidacionService
{
    private readonly Dictionary<string, ILiquidadorStrategy> _estrategias;
    private readonly ILiquidadorStrategy _estrategiaDefault;

    public LiquidacionService(IEnumerable<ILiquidadorStrategy> estrategias = null)
    {
        _estrategias = new Dictionary<string, ILiquidadorStrategy>(StringComparer.OrdinalIgnoreCase);
        _estrategiaDefault = new LiquidacionAutoStrategy();

        if (estrategias != null)
        {
            foreach (var est in estrategias)
            {
                _estrategias[est.Ramo] = est;
            }
        }
        else
        {
            // Estrategias predeterminadas
            _estrategias["AUTO"] = new LiquidacionAutoStrategy();
            _estrategias["HOGAR"] = new LiquidacionHogarStrategy();
            _estrategias["VIDA"] = new LiquidacionVidaStrategy();
        }
    }

    // Complejidad O(1) tiempo constante
    public bool LiquidarSiniestro(Siniestro siniestro)
    {
        if (siniestro == null || siniestro.Poliza == null || siniestro.Peritaje == null)
            return false;

        if (!siniestro.Poliza.Activa || !siniestro.Peritaje.DanosVerificados)
        {
            siniestro.Estado = "RECHAZADO";
            siniestro.MontoLiquidado = 0f;
            return false;
        }

        // Búsqueda O(1) de la estrategia adecuada según tipo de póliza
        string tipo = siniestro.Poliza.Tipo ?? "AUTO";
        if (!_estrategias.TryGetValue(tipo, out var estrategia))
        {
            estrategia = _estrategiaDefault;
        }

        float neto = estrategia.CalcularIndemnizacion(
            siniestro.Peritaje.MontoPresupuestado,
            siniestro.Poliza.CoberturaMax,
            siniestro.Poliza.Deducible
        );

        siniestro.MontoLiquidado = neto;
        siniestro.Estado = neto > 0 ? "LIQUIDADO" : "RECHAZADO";
        return neto > 0;
    }
}
