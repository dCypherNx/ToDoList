using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class LoggerMockExtensions
{
    public static void VerifyLogging<T>(this Mock<ILogger<T>> loggerMock, string message, LogLevel level, Times times)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            times);
    }
}