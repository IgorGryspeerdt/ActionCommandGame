using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ActionCommandGame.Sdk.Handlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly Func<string?> _getToken;

        public AuthorizationHandler(Func<string?> getToken)
        {
            _getToken = getToken;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _getToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
