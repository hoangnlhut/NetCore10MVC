using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MySessionIntegrationTest
{
    public class SessionTest : IClassFixture<WebApplicationFactory<SessionFeature.Program>>
    {
        private readonly WebApplicationFactory<SessionFeature.Program> _factory;

        public SessionTest(WebApplicationFactory<SessionFeature.Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Test/TestGetSession")]
        public async Task Call_AsyncTestGetSession_Return_Ok(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [InlineData("/Test/GetSessionValue?key=Hoang")]
        public async Task Call_Async_Set_GetSessionValue_Return_Ok()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act: Set Session Value first
            var response1 = await client.GetAsync("/Test/SetSessionValue?key=Trang&value=Vietdohoi");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);


            // Act: Set Session Value first
            var response2 = await client.GetAsync("/Test/GetSessionValue?key=Trang");
            var responseContent = await response2.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.Equal("Vietdohoi", responseContent);
        }
    }
}
