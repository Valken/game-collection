using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GameCollection.Database;
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
            _client = _factory.CreateClient();
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task Get_Systems()
        {
            
            var context = (GamesContext)_factory.Services.GetService(typeof(GamesContext));
            context.Systems.Add(new Database.Models.System {Name = "Nintendo Entertainment System"});
            context.SaveChanges();
            
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