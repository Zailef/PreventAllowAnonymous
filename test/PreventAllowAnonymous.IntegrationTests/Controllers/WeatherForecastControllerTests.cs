using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PreventAllowAnonymous.IntegrationTests.Controllers
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task Valid_Get_Returns_200_Response()
        {
            // Act
            var response = await _client.GetAsync($"{_client.BaseAddress}WeatherForecast");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Unauthorized_Post_Returns_401_Response()
        {
            // Arrange
            var json = JsonSerializer.Serialize(new WeatherForecast());

            // Act
            var response = await _client.PostAsync($"{_client.BaseAddress}WeatherForecast", new StringContent(json));

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
