namespace Common.Api.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string error) : base(error) 
    {
        
    }
}
