using Xunit;
using Xunit.Abstractions;

namespace GameCollection.WebApi.Tests;

public class GameControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    public GameControllerTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
        
    }
}