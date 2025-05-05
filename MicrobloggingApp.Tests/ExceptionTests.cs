using MicrobloggingApp.API.Extensions;
using MicrobloggingApp.Infrastructure.Interfacses;
using MicrobloggingApp.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace MicrobloggingApp.Tests
{
    public class ExceptionTests
    {
        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_LogsAndReturns500()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var mockLogger = new Mock<IExceptionLogger>();

            RequestDelegate next = (HttpContext ctx) =>
            {
                throw new InvalidOperationException("Boom!");
            };

            var middleware = new ExceptionLoggingMiddleware(next, mockLogger.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(500, context.Response.StatusCode);
            mockLogger.Verify(l => l.Log(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Log_WritesException_AndDoesNotThrow()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<EventViewerLogger>>();
            var logger = new EventViewerLogger(mockLogger.Object);

            var exception = new InvalidOperationException("Test exception");

            // Act & Assert
            var ex = Record.Exception(() => logger.Log(exception, "TestContext"));
            Assert.Null(ex); // ensure it doesn't throw
        }
    }
}
