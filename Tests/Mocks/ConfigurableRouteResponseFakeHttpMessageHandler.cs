using Newtonsoft.Json.Linq;

namespace PitneyBowes.NexShipService.UnitTests.Mocks
{
    class ConfigurableRouteResponseFakeHttpMessageHandler : DelegatingHandler
    {
        private readonly IEnumerable<RouteResponse> _definitions;
        private readonly HttpResponseMessage _defaultResponse;

        public ConfigurableRouteResponseFakeHttpMessageHandler(
            IEnumerable<RouteResponse> routeResponseDefinitions,
            HttpResponseMessage defaultResponse
            )
        {
            _definitions = routeResponseDefinitions;
            _defaultResponse = defaultResponse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var route = _definitions.FirstOrDefault(def =>
                request.Method == def.Method &&
                request.RequestUri.PathAndQuery.StartsWith(StandarizeRoute(def.Route), System.StringComparison.InvariantCultureIgnoreCase));

            if (route != null)
            {
                if (route is RouteResponseWithJsonBody routeResponseWithBody)
                {
                    var requestContent = await request.Content.ReadAsStringAsync();

                    var requestJson = JToken.Parse(requestContent);
                    var bodyJson = JToken.Parse(routeResponseWithBody.Body);

                    if (!JToken.DeepEquals(requestJson, bodyJson))
                    {
                        return routeResponseWithBody.FailedResponse;
                    }
                } 
                return await Task.FromResult(route.Response);
            }
            else
            {
                return await Task.FromResult(_defaultResponse);
            }
        }

        private string StandarizeRoute(string url)
        {
            // Add leading '/' if not present
            if (url.StartsWith('/'))
                return url;

            return $"/{url}";
        }
    }

    class RouteResponse
    {
        public HttpMethod Method { get; set; }
        public string Route { get; set; }
        public HttpResponseMessage Response { get; set; }
    }

    class RouteResponseWithJsonBody : RouteResponse
    {
        public string Body { get; set; }
        public HttpResponseMessage FailedResponse { get; set; }
    }
}
