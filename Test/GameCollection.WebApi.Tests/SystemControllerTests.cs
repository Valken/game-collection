using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GameCollection.WebApi.Tests
{
    public class SystemControllerTests : IClassFixture</*Custom*/WebApplicationFactory<Startup>>
    {
        private /*Custom*/WebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        
        public SystemControllerTests(/*Custom*/WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task Get_Systems()
        {
            var result = await _client.GetAsync("/system");
            result.EnsureSuccessStatusCode();
            
            var systems = await JsonSerializer
                .DeserializeAsync<IEnumerable<Database.Models.System>>(
                    await result.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}