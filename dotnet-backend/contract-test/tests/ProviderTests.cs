using contract_test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using PactNet;
using System.Net.Http;

namespace tests
{
    public class ProviderTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>();
                });

            var host = hostBuilder.Start();

            _client = host.GetTestClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json, text/json, text/x-json, text/javascript, application/xml, text/xml");
        }

        [Test]
        public void Test()
        {
            //Arrange
            var pactVerifierConfig = new PactVerifierConfig()
            {
                PublishVerificationResults = true,
                ProviderVersion = "1.0.0"
            };
            var pactVerifier = new PactVerifier(pactVerifierConfig);

            pactVerifier.ProviderState("http://localhost:9292");

            //Act & Assert
            pactVerifier.ServiceProvider("WorkList.Api", _client.BaseAddress.ToString())
                .HonoursPactWith("WorkList.Service")
                .PactUri("http://localhost:9292/pacts/provider/WorkList.Api/consumer/WorkList.Service/latest")
                .Verify();
        }
    }
}