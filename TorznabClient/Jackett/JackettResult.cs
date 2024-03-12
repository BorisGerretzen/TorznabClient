namespace TorznabClient.Jackett;

public record JackettResult<T>(T? Result, Exception? Error) where T : class
{
    public bool IsError => Error != null;
    public bool IsSuccess => Result != null;

    public static implicit operator JackettResult<T>(T result)
    {
        return new JackettResult<T>(result, null);
    }

    public static implicit operator JackettResult<T>(Exception error)
    {
        return new JackettResult<T>(null, error);
    }
}