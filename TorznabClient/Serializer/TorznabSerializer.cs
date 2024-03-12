using System.Text;
using System.Xml;
using TorznabClient.Exceptions;
using TorznabClient.Models;

namespace TorznabClient.Serializer;

public class TorznabSerializer<TSerializer> : ITorznabSerializer<TSerializer>
{
    private readonly XmlSerializer _errorSerializer = new(typeof(TorznabError));
    private readonly XmlSerializer _serializer = new(typeof(TSerializer));

    public TSerializer? Deserialize(string xml)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        return Deserialize(stream);
    }

    public TSerializer? Deserialize(Stream stream)
    {
        if (IsError(stream))
        {
            var error = (TorznabError?)_errorSerializer.Deserialize(stream);
            ThrowError(error);
        }

        return (TSerializer?)_serializer.Deserialize(stream);
    }

    private bool IsError(Stream stream)
    {
        using var reader = XmlReader.Create(stream);
        var isError = _errorSerializer.CanDeserialize(reader);
        stream.Seek(0, SeekOrigin.Begin);
        return isError;
    }

    private static void ThrowError(TorznabError? error)
    {
        if (error is null) throw new TorznabException(0, "Unknown error.");

        throw new TorznabException(error.Code, error.Description ?? "Unknown error.");
    }
}