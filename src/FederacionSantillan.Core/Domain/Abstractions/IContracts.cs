namespace FederacionSantillan.Core.Domain.Abstractions;

/// <summary>
/// Contrato de mutación in-place de estado (Tríada Sagrada: copy).
/// </summary>
public interface ICopiable<in T>
{
    void copy(T origen);
}

/// <summary>
/// Contrato de instanciación independiente en Heap (Tríada Sagrada: clone).
/// </summary>
public interface IClonable<out T>
{
    T clone();
}

/// <summary>
/// Contrato de entidad con identificador unívoco para indexación O(1).
/// </summary>
public interface IIdentificable<out TId>
{
    TId Id { get; }
}
