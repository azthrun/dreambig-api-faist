namespace DreamBig.Faist.Application.Common.Exceptions;

public sealed class RetriableException : FaistException
{
    public RetriableException(string message) : base(message)
    {
    }

    public RetriableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
