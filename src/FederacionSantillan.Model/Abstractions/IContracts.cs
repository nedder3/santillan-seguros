namespace FederacionSantillan.Model.Abstractions;

public interface ICopiable<T>
{
    void copy(T other);
}

public interface IClonable<T>
{
    T clone();
}

public interface IEquatableDomain<T>
{
    bool equals(T other);
}

public interface IIdentifiable<out TKey>
{
    TKey Id { get; }
}
