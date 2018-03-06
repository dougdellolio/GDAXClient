﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GDAXSharp.Authentication;
using GDAXSharp.HttpClient;
using GDAXSharp.Services.Fundings;
using GDAXSharp.Services.Fundings.Models;
using GDAXSharp.Services.HttpRequest;
using GDAXSharp.Specs.JsonFixtures.Fundings;
using GDAXSharp.Specs.JsonFixtures.HttpResponseMessage;
using Machine.Fakes;
using Machine.Specifications;

namespace GDAXSharp.Specs.Services.Fundings
{
    [Subject("FundingsService")]
    public class FundingsServiceSpecs : WithSubject<FundingsService>
    {
        static Authenticator authenticator;

        static IList<IList<Funding>> fundings_response;

        Establish context = () =>
            authenticator = new Authenticator("apiKey", new string('2', 100), "passPhrase");

        class when_requesting_all_fundings
        {
            Establish context = () =>
            {
                The<IHttpRequestMessageService>().WhenToldTo(p => p.CreateHttpRequestMessage(Param.IsAny<HttpMethod>(), Param.IsAny<Authenticator>(), Param.IsAny<string>(), Param.IsAny<string>()))
                    .Return(new HttpRequestMessage());

                The<IHttpClient>().WhenToldTo(p => p.SendASync(Param.IsAny<HttpRequestMessage>()))
                    .Return(Task.FromResult(HttpResponseMessageFixture.CreateWithEmptyValue()));

                The<IHttpClient>().WhenToldTo(p => p.ReadAsStringAsync(Param.IsAny<HttpResponseMessage>()))
                    .Return(Task.FromResult(FundingsResponseFixture.Create()));
            };

            Because of = () =>
                fundings_response = Subject.GetAllFundingsAsync().Result;

            It should_return_a_response = () =>
                fundings_response.ShouldNotBeNull();

            It should_return_a_correct_response = () =>
            {
                fundings_response.First().First().Id.ShouldEqual(new Guid("b93d26cd-7193-4c8d-bfcc-446b2fe18f71"));
                fundings_response.First().First().Order_id.ShouldEqual("b93d26cd-7193-4c8d-bfcc-446b2fe18f71");
                fundings_response.First().First().Profile_id.ShouldEqual("d881e5a6-58eb-47cd-b8e2-8d9f2e3ec6f6");
                fundings_response.First().First().Amount.ShouldEqual(1057.6519956381537500M);
                fundings_response.First().First().Status.ShouldEqual("settled");
                fundings_response.First().First().Currency.ShouldEqual("USD");
                fundings_response.First().First().Repaid_amount.ShouldEqual(1057.6519956381537500M);
                fundings_response.First().First().Default_amount.ShouldEqual(0);
                fundings_response.First().First().Repaid_default.ShouldBeFalse();

                fundings_response.First().Skip(1).First().Id.ShouldEqual(new Guid("280c0a56-f2fa-4d3b-a199-92df76fff5cd"));
                fundings_response.First().Skip(1).First().Order_id.ShouldEqual("280c0a56-f2fa-4d3b-a199-92df76fff5cd");
                fundings_response.First().Skip(1).First().Profile_id.ShouldEqual("d881e5a6-58eb-47cd-b8e2-8d9f2e3ec6f6");
                fundings_response.First().Skip(1).First().Amount.ShouldEqual(545.2400000000000000M);
                fundings_response.First().Skip(1).First().Status.ShouldEqual("outstanding");
                fundings_response.First().Skip(1).First().Currency.ShouldEqual("USD");
                fundings_response.First().Skip(1).First().Repaid_amount.ShouldEqual(532.7580047716682500M);
            };
        }
    }
}
