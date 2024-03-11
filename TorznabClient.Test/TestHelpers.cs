using System.Net;
using Moq;
using Moq.Protected;

namespace TorznabClient.Test;

public static class TestHelpers
{
    /// <summary>
    ///     Creates a mocked HTTP client.
    /// </summary>
    /// <param name="responseContent">Response to return.</param>
    /// <param name="baseAddress">Base address of the http client.</param>
    /// <param name="statusCode">Status code to return.</param>
    /// <param name="expectedUrl">Expected url.</param>
    public static HttpClient GetMockedClient(
        string responseContent,
        string? baseAddress = null,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string? expectedUrl = null
    )
    {
        var mockHandler = new Mock<HttpMessageHandler>();

        var responseMessageCondition = new Func<HttpRequestMessage, bool>(req =>
        {
            if (expectedUrl != null)
            {
                var isCorrectUrl = req.RequestUri?.ToString() == expectedUrl;

                if (!isCorrectUrl) Assert.That(req.RequestUri?.ToString(), Is.EqualTo(expectedUrl));
            }

            return true;
        });

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => responseMessageCondition(req)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseContent)
            })
            .Verifiable();

        return new HttpClient(mockHandler.Object)
        {
            BaseAddress = baseAddress == null ? null : new Uri(baseAddress)
        };
    }
}