﻿using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using GDAXSharp.Network.Authentication;
using GDAXSharp.Shared.Utilities.Clock;
using GDAXSharp.Shared.Utilities.Extensions;

namespace GDAXSharp.Network.HttpRequest
{
    public class HttpRequestMessageService : IHttpRequestMessageService
    {
        private const string ApiUri = "https://api.gdax.com";

        private const string SandBoxApiUri = "https://api-public.sandbox.gdax.com";

        private readonly IAuthenticator authenticator;

        private readonly IClock clock;

        private readonly bool sandBox;

        public HttpRequestMessageService(IAuthenticator authenticator, IClock clock, bool sandBox)
        {
            this.authenticator = authenticator;
            this.clock = clock;
            this.sandBox = sandBox;
        }

        public HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod, 
            string requestUri, 
            string contentBody = "")
        {
            var baseUri = sandBox
                ? SandBoxApiUri
                : ApiUri;

            var requestMessage = new HttpRequestMessage(httpMethod, new Uri(new Uri(baseUri), requestUri))
            {
                Content = contentBody == string.Empty
                    ? null
                    : new StringContent(contentBody, Encoding.UTF8, "application/json")
            };

            var timeStamp = clock.GetTime().ToTimeStamp();
            var signedSignature = ComputeSignature(httpMethod, authenticator.UnsignedSignature, timeStamp, requestUri, contentBody);

            AddHeaders(requestMessage, signedSignature, timeStamp);
            return requestMessage;
        }

        private static string ComputeSignature(HttpMethod httpMethod, string secret, double timestamp, string requestUri, string contentBody = "")
        {
            var convertedString = Convert.FromBase64String(secret);
            var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() + requestUri + contentBody;
            return HashString(prehash, convertedString);
        }

        private static string HashString(string str, byte[] secret)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var hmaccsha = new HMACSHA256(secret))
            {
                return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
            }
        }

        private void AddHeaders(
            HttpRequestMessage httpRequestMessage,
            string signedSignature,
            double timeStamp)
        {
            httpRequestMessage.Headers.Add("User-Agent", "GDAXClient");
            httpRequestMessage.Headers.Add("CB-ACCESS-KEY", authenticator.ApiKey);
            httpRequestMessage.Headers.Add("CB-ACCESS-TIMESTAMP", timeStamp.ToString("F0", CultureInfo.InvariantCulture));
            httpRequestMessage.Headers.Add("CB-ACCESS-SIGN", signedSignature);
            httpRequestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", authenticator.Passphrase);
        }
    }
}
