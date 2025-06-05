namespace backend.Infrastructure.Exceptions;

public class SubscriptionNotExistException : Exception
{
    public SubscriptionNotExistException(string message) : base(message)
    {
        
    }
}