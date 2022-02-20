using RestSharp;

namespace contract_test.consumers
{
    public class WorkListApiConsumer
    {
        private readonly RestClient _restClient;

        public WorkListApiConsumer(string baseUri)
        {
            _restClient = new RestClient(baseUri);
        }

        public object Get()
        {
            var request = new RestRequest("/api/v1.0/orders", Method.GET)
            { 
                RequestFormat = DataFormat.Json
            };

            var result = _restClient.Execute<object>(request);

            return result.Data;
        }
    }
}