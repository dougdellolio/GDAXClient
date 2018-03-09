﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GDAXSharp.Authentication;
using GDAXSharp.HttpClient;
using GDAXSharp.Services.Accounts.Models;
using GDAXSharp.Services.HttpRequest;

namespace GDAXSharp.Services.Accounts
{
    public class AccountsService : AbstractService
    {
        private readonly IHttpClient httpClient;

        private readonly IAuthenticator authenticator;

        public AccountsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService)
        {
            this.httpClient = httpClient;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, "/accounts");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var accountList = DeserializeObject<List<Account>>(contentBody);

            return accountList;
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            var httpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, authenticator, $"/accounts/{id}");
            var contentBody = await httpClient.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            var account = DeserializeObject<Account>(contentBody);

            return account;
        }

        public async Task<IList<IList<AccountHistory>>> GetAccountHistoryAsync(string id, int limit = 100, int numberOfPages = 0)
        {
            var httpResponseMessage = await SendHttpRequestMessagePagedAsync<AccountHistory>(HttpMethod.Get, authenticator, $"/accounts/{id}/ledger?limit={limit}", numberOfPages: numberOfPages);

            return httpResponseMessage;
        }

        public async Task<IList<IList<AccountHold>>> GetAccountHoldsAsync(string id, int limit = 100, int numberOfPages = 0)
        {
            var httpResponseMessage = await SendHttpRequestMessagePagedAsync<AccountHold>(HttpMethod.Get, authenticator, $"/accounts/{id}/holds?limit={limit}", numberOfPages: numberOfPages);

            return httpResponseMessage;
        }
    }
}
