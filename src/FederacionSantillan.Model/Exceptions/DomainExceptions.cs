namespace FederacionSantillan.Model.Exceptions;

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
    public BusinessRuleException(string message, Exception inner) : base(message, inner) { }
}

public class DuplicateEntityException : BusinessRuleException
{
    public DuplicateEntityException(string message) : base(message) { }
    public DuplicateEntityException(string message, Exception inner) : base(message, inner) { }
}

public class PersistenceException : Exception
{
    public PersistenceException(string message) : base(message) { }
    public PersistenceException(string message, Exception inner) : base(message, inner) { }
}
