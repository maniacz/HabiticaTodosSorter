using Moq;
using System.Net;
using HabiticaTodosSorter.Clients;
using Microsoft.Extensions.Configuration;
using Tests.Clients;

namespace PitneyBowes.NexShipService.UnitTests.Mocks
{
    static class HttpClientFactoryMockProvider
    {
        public static IConfiguration Configuration { get; set; }
        
        public static Mock<IHttpClientFactory> GetIHttpClientFactoryMock(params RouteResponse[] routeResponseDefinitions)
        {
            var defaultResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            return GetIHttpClientFactoryMock("https://habitica.com", routeResponseDefinitions, defaultResponse);
        }

        public static Mock<IHttpClientFactory> GetIHttpClientFactoryMock(IEnumerable<RouteResponse> routeResponseDefinitions)
        {
            var defaultResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            return GetIHttpClientFactoryMock("https://habitica.com/", routeResponseDefinitions, defaultResponse);
        }

        public static Mock<IHttpClientFactory> GetIHttpClientFactoryMock(string baseAddress, params RouteResponse[] routeResponseDefinitions)
        {
            var defaultResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            return GetIHttpClientFactoryMock(baseAddress, routeResponseDefinitions, defaultResponse);
        }

        public static Mock<IHttpClientFactory> GetIHttpClientFactoryMock(string baseAddress, IEnumerable<RouteResponse> routeResponseDefinitions)
        {
            var defaultResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            return GetIHttpClientFactoryMock(baseAddress, routeResponseDefinitions, defaultResponse);
        }

        // public static Mock<IHttpClientFactory> GetIHttpClientFactoryMock(string baseAddress, IEnumerable<RouteResponse> routeResponseDefinitions, HttpResponseMessage defaultResponse)
        // {
        //     var fakeHttpMessageHandler = new ConfigurableRouteResponseFakeHttpMessageHandler(routeResponseDefinitions, defaultResponse);
        //     var client = new HttpClient(fakeHttpMessageHandler);
        //     client.BaseAddress = string.IsNullOrEmpty(baseAddress) ? null : new Uri(baseAddress);
        //
        //     var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        //     httpClientFactoryMock
        //         .Setup(x => x.CreateClient(It.IsAny<string>()))
        //         .Returns(client);
        //
        //     return httpClientFactoryMock;
        // }
        
        public static Mock<IHttpClientFactory> GetIHttpClientFactoryMock(string baseAddress, IEnumerable<RouteResponse> routeResponseDefinitions, HttpResponseMessage defaultResponse)
        {
            var fakeHttpMessageHandler = new ConfigurableRouteResponseFakeHttpMessageHandler(routeResponseDefinitions, defaultResponse);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(fakeHttpMessageHandler) { BaseAddress = string.IsNullOrEmpty(baseAddress) ? null : new Uri(baseAddress) });

            return httpClientFactoryMock;
        }
    }
}
