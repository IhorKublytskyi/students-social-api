namespace backend.Infrastructure.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string messsage) : base(messsage)
    {
        
    }
}