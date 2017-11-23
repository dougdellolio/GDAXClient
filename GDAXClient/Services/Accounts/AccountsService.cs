﻿using GDAXClient.Authentication;
using GDAXClient.HttpClient;
using GDAXClient.Services.HttpRequest;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GDAXClient.Services.Accounts
{
    public class AccountsService
    {
        private readonly IHttpRequestMessageService httpRequestMessageService;

        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public AccountsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
        {
            this.httpRequestMessageService = httpRequestMessageService;
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            var httpRequestMessage = httpRequestMessageService.CreateHttpRequestMessage(HttpMethod.Get, authenticator, "/accounts");

            var httpResponseMessage = await httpClient.SendASync(httpRequestMessage);
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);

            var accountList = JsonConvert.DeserializeObject<List<Account>>(contentBody);

            return accountList;
        }
    }
}
