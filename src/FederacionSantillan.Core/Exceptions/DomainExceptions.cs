namespace FederacionSantillan.Core.Exceptions;

public class NegocioException : Exception
{
    public NegocioException(string mensaje) : base(mensaje) { }
    public NegocioException(string mensaje, Exception inner) : base(mensaje, inner) { }
}

public class EntidadDuplicadaException : NegocioException
{
    public EntidadDuplicadaException(string mensaje) : base(mensaje) { }
    public EntidadDuplicadaException(string mensaje, Exception inner) : base(mensaje, inner) { }
}

public class PersistenciaException : Exception
{
    public PersistenciaException(string mensaje) : base(mensaje) { }
    public PersistenciaException(string mensaje, Exception inner) : base(mensaje, inner) { }
}
