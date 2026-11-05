using System.Collections.Generic;

namespace FederacionSantillan.Core.Repositories;

/// <summary>
/// Contrato de repositorio desacoplado (DIP - Dependency Inversion Principle).
/// </summary>
public interface IRepository<T, in TId>
{
    /// <summary>Complejidad temporal O(1) amortizado.</summary>
    bool Agregar(T entidad);

    /// <summary>Complejidad temporal O(1) constante mediante tabla hash.</summary>
    T ObtenerPorId(TId id);

    /// <summary>Complejidad temporal O(1) existencia.</summary>
    bool Existe(TId id);

    /// <summary>Complejidad temporal O(N) enumeración de elementos.</summary>
    IReadOnlyCollection<T> ObtenerTodos();

    /// <summary>Complejidad temporal O(1) conteo total.</summary>
    int Cantidad { get; }
}
