namespace DreamBig.Faist.Application.Common.Exceptions;

public class FaistException : Exception
{
    public FaistException(string message) : base(message)
    {
    }

    public FaistException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
