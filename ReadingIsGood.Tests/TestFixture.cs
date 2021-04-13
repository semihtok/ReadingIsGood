using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace ReadingIsGood.Tests
{
    public class TestFixture : IDisposable  
    {
        private readonly TestServer _server;

        public TestFixture()
        {
            try
            {
                var builder = new WebHostBuilder()
                    .UseStartup<API.Startup>()
                    .ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.SetBasePath(Path.Combine(
                            Directory.GetCurrentDirectory(), "..\\..\\..\\..\\ReadingIsGood.API"));

                        configBuilder.AddJsonFile("appsettings.json");
                    
                    });
                _server = new TestServer(builder);

                Client = _server.CreateClient();
                Client.BaseAddress = new Uri("http://localhost:5000");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}