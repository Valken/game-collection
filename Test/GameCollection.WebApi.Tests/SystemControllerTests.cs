using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GameCollection.Database;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using System = GameCollection.Database.Models.System;

namespace GameCollection.WebApi.Tests
{
    public class SystemControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        private ITestOutputHelper _outputHelper;

        public SystemControllerTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GamesContext>();
                if (db.Database.EnsureCreated())
                {
                    db.Systems.Add(new Database.Models.System { Name = "Something" });
                    db.SaveChanges();
                }
            }
            _client = _factory.CreateClient();
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task Get_Systems()
        {
            var result = await _client.GetAsync("/system");
            if (!result.IsSuccessStatusCode)
            {
                _outputHelper.WriteLine(await result.Content.ReadAsStringAsync());
            }
            
            var systems = await JsonSerializer
                .DeserializeAsync<IEnumerable<Database.Models.System>>(
                    await result.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            Assert.NotEmpty(systems);
        }
    }
}