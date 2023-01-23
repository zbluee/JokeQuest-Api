using System.Net;
namespace JokeApi.Errors;


public class CustomAPIException : Exception
{
    
    public CustomAPIException(string message, int statusCode) : base(message) => this.HResult = statusCode;

}

class BadRequestException : CustomAPIException
{

    public BadRequestException(string message) : base(message, (int)HttpStatusCode.BadRequest) {}

}

class UnauthenticatedException : CustomAPIException
{
    public UnauthenticatedException(string message) : base(message, (int)HttpStatusCode.Unauthorized) {}
}

class NotFoundException : CustomAPIException
{
    public NotFoundException(string message) : base(message, (int)HttpStatusCode.NotFound) {}
}

