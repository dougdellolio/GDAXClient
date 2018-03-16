﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GDAXSharp.Authentication;
using GDAXSharp.HttpClient;
using GDAXSharp.Services.HttpRequest;

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
            return await MakeServiceCall<List<Models.Currency>>(HttpMethod.Get, "/currencies").ConfigureAwait(false);
        }
    }
}
