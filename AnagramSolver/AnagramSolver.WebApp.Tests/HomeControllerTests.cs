using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace AnagramSolver.WebApp.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HomeController _sut;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            client.BaseAddress = new Uri("http://localhost");

            _mockHttpClientFactory.Setup(x => x.CreateClient("AnagramApi"))
                .Returns(client);

            _sut = new HomeController(_mockLogger.Object, _mockHttpClientFactory.Object);
            
            // Setup ControllerContext for Cookies
            var httpContext = new DefaultHttpContext();
            var sessionMock = new Mock<ISession>();
            
            // Mock session behavior
            byte[] value = null;
            sessionMock.Setup(x => x.TryGetValue(It.IsAny<string>(), out value))
                .Returns(false);
            
            httpContext.Session = sessionMock.Object;
            
            _sut.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithEmptyModel_WhenNoIdProvided()
        {
            // Act
            var result = await _sut.Index(null, CancellationToken.None);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<AnagramViewModel>().Subject;
            model.Anagrams.Should().BeEmpty();
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithAnagrams_WhenIdProvidedAndApiReturnsSuccess()
        {
            // Arrange
            var id = "test";
            var expectedAnagrams = new List<string> { "sett", "tset" };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().Contains($"anagrams/{id}")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedAnagrams)
                });

            // Act
            var result = await _sut.Index(id, CancellationToken.None);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<AnagramViewModel>().Subject;
            model.Anagrams.Should().BeEquivalentTo(expectedAnagrams);
        }
    }
}
