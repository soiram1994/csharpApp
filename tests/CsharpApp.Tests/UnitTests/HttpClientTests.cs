using System.Net;
using CSharpApp.Application.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace CsharpApp.Tests.UnitTests
{
    public class HttpClientWrapperTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly HttpClientWrapper _httpClientWrapper;

        public HttpClientWrapperTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
            _httpClientWrapper = new HttpClientWrapper(_httpClient);
        }

        [Fact]
        public async Task GetFromJsonAsync_ReturnsDeserializedObject_WhenResponseIsSuccessful()
        {
            // Arrange
            var expectedObject = new { Name = "Test" };
            var jsonResponse = JsonConvert.SerializeObject(expectedObject);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _httpClientWrapper.GetFromJsonAsync<object>("http://test.com");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetFromJsonAsync_ThrowsException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _httpClientWrapper.GetFromJsonAsync<object>("http://test.com"));
        }

        [Fact]
        public async Task PostAsJsonAsync_ReturnsResponse_WhenCalled()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var response = await _httpClientWrapper.PostAsJsonAsync("http://test.com", new { Name = "Test" });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutAsJsonAsync_ReturnsResponse_WhenCalled()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var response = await _httpClientWrapper.PutAsJsonAsync("http://test.com", new { Name = "Test" });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsResponse_WhenCalled()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var response = await _httpClientWrapper.DeleteAsync("http://test.com");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}