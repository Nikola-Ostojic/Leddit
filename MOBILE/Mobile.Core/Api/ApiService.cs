using Mobile.Core.Runtime;
using Refit;
using Splat;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Mobile.Core.Api
{
    public class ApiService<IRestApi> : IApiService<IRestApi>
    {
        private readonly IRuntimeContext _runtimeContext;
#if DEBUG
        private string _apiBaseAddress = "http://127.0.0.1:5000/api";

#else
        private string _apiBaseAddress = "https://leddit.azurewebsites.net/api";
#endif

        public ApiService(IRuntimeContext runtimeContext = null)
        {
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();
        }

        public IRestApi GetClient()
        {

            var client = new HttpClient(new AuthenticatedHttpClientHandler(_runtimeContext.AccessToken))
            {
                BaseAddress = new Uri(_apiBaseAddress)
            };

            return RestService.For<IRestApi>(client);
        }
    }

    class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly string _token;

        public AuthenticatedHttpClientHandler(string token)
        {
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, _token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
