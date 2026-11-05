using System;
using System.Collections.Generic;
using FederacionSantillan.Core.Domain.Abstractions;

namespace FederacionSantillan.Core.Repositories;

/// <summary>
/// Implementación de repositorio en memoria con indexación hash O(1).
/// Garantiza búsquedas en tiempo constante eliminando escaneos lineales O(N).
/// </summary>
public class IndexedRepository<T, TId> : IRepository<T, TId> where T : IIdentificable<TId>
{
    // Índice primario por Clave: Búsqueda y Adición en O(1)
    private readonly Dictionary<TId, T> _primaryIndex;
    
    // Índice secundario invertido por Categoría/Estado: O(1) acceso a conjuntos
    private readonly Dictionary<string, HashSet<TId>> _secondaryIndex;

    public IndexedRepository()
    {
        _primaryIndex = new Dictionary<TId, T>();
        _secondaryIndex = new Dictionary<string, HashSet<TId>>(StringComparer.OrdinalIgnoreCase);
    }

    public int Cantidad => _primaryIndex.Count;

    // Complejidad: O(1) amortizado
    public bool Agregar(T entidad)
    {
        if (entidad == null || _primaryIndex.ContainsKey(entidad.Id))
            return false;

        _primaryIndex[entidad.Id] = entidad;
        return true;
    }

    // Complejidad: O(1) con indexación secundaria
    public bool AgregarConIndiceSecundario(T entidad, string categoria)
    {
        if (!Agregar(entidad)) return false;

        if (!string.IsNullOrEmpty(categoria))
        {
            if (!_secondaryIndex.TryGetValue(categoria, out var set))
            {
                set = new HashSet<TId>();
                _secondaryIndex[categoria] = set;
            }
            set.Add(entidad.Id);
        }
        return true;
    }

    // Complejidad: O(1) búsqueda directa en tabla hash
    public T ObtenerPorId(TId id)
    {
        _primaryIndex.TryGetValue(id, out var entidad);
        return entidad;
    }

    // Complejidad: O(1)
    public bool Existe(TId id) => _primaryIndex.ContainsKey(id);

    // Complejidad: O(N) solo para renderizado de colección completa
    public IReadOnlyCollection<T> ObtenerTodos() => _primaryIndex.Values;

    // Complejidad: O(1) conteo por estado gracias al índice invertido
    public int ContarPorCategoria(string categoria)
    {
        if (string.IsNullOrEmpty(categoria)) return 0;
        return _secondaryIndex.TryGetValue(categoria, out var set) ? set.Count : 0;
    }

    // Complejidad: O(K) donde K = cantidad de elementos en esa categoría (mucho menor que O(N))
    public IEnumerable<T> ObtenerPorCategoria(string categoria)
    {
        if (string.IsNullOrEmpty(categoria) || !_secondaryIndex.TryGetValue(categoria, out var set))
            yield break;

        foreach (var id in set)
        {
            if (_primaryIndex.TryGetValue(id, out var item))
                yield return item;
        }
    }
}
