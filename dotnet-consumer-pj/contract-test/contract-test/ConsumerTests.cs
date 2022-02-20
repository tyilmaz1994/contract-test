using contract_test.consumers;
using NUnit.Framework;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System.Collections.Generic;

namespace contract_test
{
    public class ConsumerTests
    {
        private PactBuilder _pactBuilder;
        private IMockProviderService _mockProviderService;
        private int _mockServerPort = 5154;
        private WorkListApiConsumer _sut;
        private string _mockProviderServiceBaseUri 
        { 
            get { return $"http://localhost:{_mockServerPort}"; } 
        }

        [SetUp]
        public void Setup()
        {
            var pactConfig = new PactConfig
            {
                
            };

            _pactBuilder = new PactBuilder();

            _pactBuilder
                .ServiceConsumer("WorkList.Service")
                .HasPactWith("WorkList.Api");

            _mockProviderService = _pactBuilder.MockService(_mockServerPort);
            _mockProviderService.ClearInteractions();

            _sut = new WorkListApiConsumer(_mockProviderServiceBaseUri);
        }

        [Test]
        public void Test()
        {
            //Arrange
            _mockProviderService
                .UponReceiving("get request to orders able to create worklist")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/api/v1.0/orders",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json, text/json, text/x-json, text/javascript, application/xml, text/xml" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type",  "application/json; charset=utf-8" }
                    },
                    Body = new[]
                    {
                        new { Code = "123", Number = 123 },
                        new { Code = "124", Number = 124 }
                    }
                });

            //Act
            var result = _sut.Get();

            //Assert
            _mockProviderService.VerifyInteractions();
            _pactBuilder.Build();

            //publish
            var pactPublisher = new PactPublisher("http://localhost:9292");
            pactPublisher.PublishToBroker("..\\..\\..\\pacts\\worklist.service-worklist.api.json", "1.0.0");
        }
    }
}