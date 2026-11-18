namespace FederacionSantillan.Model.Interfaces;

public interface ICopiable<in T>
{
    void copy(T other);
}

public interface IClonable<out T>
{
    T clone();
}

public interface IEquatableDomain<in T>
{
    bool equals(T other);
}

public interface IIdentifiable<out TKey>
{
    TKey Id { get; }
}
