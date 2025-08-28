namespace DreamBig.Faist.Application.Common.Exceptions;

public sealed class FatalException : FaistException
{
    public FatalException(string message) : base(message)
    {
    }

    public FatalException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
