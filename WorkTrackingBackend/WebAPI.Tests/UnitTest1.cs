using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace WebAPI.Tests;


public class UnitTest1
{
    private HttpClient _httpClient;

    public UnitTest1()
    {
        var builder = new WebHostBuilder()
            .UseEnvironment("Development");

        var server = new TestServer(builder);
            
        _httpClient = server.CreateClient();
    }

    [Fact]
    public void Test1()
    {

    }
}