﻿using System.Net.Http;
using System.Threading.Tasks;
using GDAXSharp.Infrastructure.Authentication;
using GDAXSharp.Infrastructure.HttpClient;
using GDAXSharp.Infrastructure.HttpRequest;
using GDAXSharp.Services.Withdrawals.Models;
using GDAXSharp.Services.Withdrawals.Models.Responses;
using GDAXSharp.Shared.Types;

namespace GDAXSharp.Services.Withdrawals
{
    public class WithdrawalsService : AbstractService
    {
        public WithdrawalsService(
            IHttpClient httpClient,
            IHttpRequestMessageService httpRequestMessageService,
            IAuthenticator authenticator)
                : base(httpClient, httpRequestMessageService, authenticator)
        {
        }

        public async Task<WithdrawalResponse> WithdrawFundsAsync(
            string paymentMethodId,
            decimal amount,
            Currency currency)
        {
            var newWithdrawal = new Withdrawal
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodId = paymentMethodId
            };

            return await SendServiceCall<WithdrawalResponse>(HttpMethod.Post, "/withdrawals/payment-method", SerializeObject(newWithdrawal)).ConfigureAwait(false);
        }

        public async Task<CoinbaseResponse> WithdrawToCoinbaseAsync(
            string coinbaseAccountId,
            decimal amount,
            Currency currency)
        {
            var newCoinbaseWithdrawal = new Coinbase
            {
                Amount = amount,
                Currency = currency,
                CoinbaseAccountId = coinbaseAccountId
            };

            return await SendServiceCall<CoinbaseResponse>(HttpMethod.Post, "/withdrawals/coinbase-account", SerializeObject(newCoinbaseWithdrawal)).ConfigureAwait(false);
        }

        public async Task<CryptoResponse> WithdrawToCryptoAsync(
            string cryptoAddress,
            decimal amount,
            Currency currency)
        {
            var newCryptoWithdrawal = new Crypto
            {
                Amount = amount,
                Currency = currency,
                CryptoAddress = cryptoAddress
            };

            return await SendServiceCall<CryptoResponse>(HttpMethod.Post, "/withdrawals/crypto", SerializeObject(newCryptoWithdrawal)).ConfigureAwait(false);
        }
    }
}
