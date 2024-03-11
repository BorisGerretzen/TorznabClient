namespace TorznabClient.Models.Exceptions;

public class TorznabException(int code, string message) : Exception(message)
{
    public int Code { get; } = code;
}