using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Something.Something.Tests
{
    public interface ISomething
    {
        void DoThings();
    }

    public class SomethingProvider
    {
        private static readonly AsyncLocal<string> SomethingImportant = new AsyncLocal<string>();
        
        public string Important
        {
            get => SomethingImportant.Value;
            set => SomethingImportant.Value = value;
        }
    }
    
    public class FirstMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly SomethingProvider _somethingProvider;
        
        public FirstMiddleWare(RequestDelegate next, SomethingProvider somethingProvider)
        {
            _next = next;
            _somethingProvider = somethingProvider;
        }
        
        public Task Invoke(HttpContext httpContext, IServiceProvider services)
        {
            var something = services.GetRequiredService<ISomething>();
            if (_somethingProvider.Important == "Yes?") something.DoThings();
            return _next(httpContext);
        }
    }
    
    public class SecondMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly SomethingProvider _somethingProvider;

        public SecondMiddleWare(RequestDelegate next, SomethingProvider somethingProvider)
        {
            _next = next;
            _somethingProvider = somethingProvider;
        }
        
        public Task Invoke(HttpContext httpContext)
        {
            _somethingProvider.Important = "Oh hi!!!";
            return _next(httpContext);
        }
    }
    
    public class StuffTests
    {
        [Fact]
        public async Task MiddlewareThings()
        {
            var somethingProvider = new SomethingProvider { Important = "Yes?" };
            var second = new SecondMiddleWare(context => Task.CompletedTask, somethingProvider);
            var first = new FirstMiddleWare(second.Invoke, somethingProvider);
            var something = new Mock<ISomething>();
            var services = new ServiceCollection()
                .AddScoped(p => something.Object)
                .BuildServiceProvider();
            
            var httpContext = new DefaultHttpContext();
            await first.Invoke(httpContext, services);
            
            something.Verify(s => s.DoThings());
            Assert.Equal("Oh hi!!!", somethingProvider.Important);
        }
    }
}