﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GDAXSharp.Infrastructure.Authentication;
using GDAXSharp.Infrastructure.HttpClient;
using GDAXSharp.Infrastructure.HttpRequest;

namespace GDAXSharp.Services.Currencies
{
    public class CurrenciesService : AbstractService
    {
        public CurrenciesService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService, authenticator)
        {
        }

        public async Task<IEnumerable<Models.Currency>> GetAllCurrenciesAsync()
        {
            return await SendServiceCall<List<Models.Currency>>(HttpMethod.Get, "/currencies").ConfigureAwait(false);
        }
    }
}
