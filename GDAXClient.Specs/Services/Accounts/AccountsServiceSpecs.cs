﻿using System;
using GDAXClient.Authentication;
using GDAXClient.HttpClient;
using GDAXClient.Services.Accounts;
using GDAXClient.Services.HttpRequest;
using GDAXClient.Specs.Fixtures;
using GDAXClient.Specs.JsonFixtures;
using Machine.Fakes;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GDAXClient.Specs.JsonFixtures.Accounts;

namespace GDAXClient.Specs.Services.Accounts
{
    [Subject("AccountsService")]
    public class AccountsServiceSpecs : WithSubject<AccountsService>
    {
        static Authenticator authenticator;

        Establish context = () =>
            authenticator = new Authenticator("apiKey", new string('2', 100), "passPhrase");

        class when_getting_all_accounts
        {
            static IEnumerable<Account> result;

            Establish context = () =>
            {
                The<IHttpRequestMessageService>().WhenToldTo(p => p.CreateHttpRequestMessage(Param.IsAny<HttpMethod>(), Param.IsAny<Authenticator>(), Param.IsAny<string>(), Param.IsAny<string>()))
                    .Return(new HttpRequestMessage());

                The<IHttpClient>().WhenToldTo(p => p.SendASync(Param.IsAny<HttpRequestMessage>()))
                    .Return(Task.FromResult(new HttpResponseMessage()));

                The<IHttpClient>().WhenToldTo(p => p.ReadAsStringAsync(Param.IsAny<HttpResponseMessage>()))
                    .Return(Task.FromResult(AllAccountsResponseFixture.Create()));
            };

            Because of = () =>
                result = Subject.GetAllAccountsAsync().Result;

            It should_have_correct_count = () =>
                result.Count().ShouldEqual(1);

            It should_have_correct_account_information = () =>
            {
                result.First().Id.ShouldEqual(new System.Guid("e316cb9a-0808-4fd7-8914-97829c1925de"));
                result.First().Currency.ShouldEqual("USD");
                result.First().Balance.ShouldEqual(80.2301373066930000M);
                result.First().Available.ShouldEqual(79.2266348066930000M);
                result.First().Hold.ShouldEqual(1.0035025000000000M);
                result.First().Margin_enabled.ShouldBeTrue();
            };
        }

        class when_getting_account_by_id
        {
            static Account result;

            Establish context = () =>
            {
                The<IHttpRequestMessageService>().WhenToldTo(p => p.CreateHttpRequestMessage(Param.IsAny<HttpMethod>(), Param.IsAny<Authenticator>(), Param.IsAny<string>(), Param.IsAny<string>()))
                    .Return(new HttpRequestMessage());

                The<IHttpClient>().WhenToldTo(p => p.SendASync(Param.IsAny<HttpRequestMessage>()))
                    .Return(Task.FromResult(new HttpResponseMessage()));

                The<IHttpClient>().WhenToldTo(p => p.ReadAsStringAsync(Param.IsAny<HttpResponseMessage>()))
                    .Return(Task.FromResult(AccountByIdResponseFixture.Create()));
            };

            Because of = () =>
                result = Subject.GetAccountByIdAsync("a1b2c3d4").Result;

            It should_have_correct_account_information = () =>
            {
                result.Id.ShouldEqual(new System.Guid("e316cb9a-0808-4fd7-8914-97829c1925de"));
                result.Currency.ShouldEqual("USD");
                result.Balance.ShouldEqual(1.100M);
                result.Available.ShouldEqual(1.00M);
            };
        }

        class when_getting_all_coinbase_accounts
        {
            static IEnumerable<CoinbaseAccount> result;

            private Establish context = () =>
            {
                The<IHttpRequestMessageService>().WhenToldTo(p => p.CreateHttpRequestMessage(Param.IsAny<HttpMethod>(), Param.IsAny<Authenticator>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(new HttpRequestMessage());

                The<IHttpClient>().WhenToldTo(p => p.SendASync(Param.IsAny<HttpRequestMessage>())).Return(Task.FromResult(new HttpResponseMessage()));

                The<IHttpClient>().WhenToldTo(p => p.ReadAsStringAsync(Param.IsAny<HttpResponseMessage>())).Return(Task.FromResult(AllCoinbaseResponseFixture.Create()));
            };

            private Because of = () =>
                result = Subject.GetCoinbaseAccountsAsync().Result;

            private It should_have_correct_count = () =>
                result.Count().ShouldEqual(4);

            private It should_have_correct_ETH_account_information = () =>
            {
                result.First().Id.ShouldEqual(new Guid("fc3a8a57-7142-542d-8436-95a3d82e1622"));
                result.First().Name.ShouldEqual("ETH Wallet");
                result.First().Currency.ShouldEqual("ETH");
                result.First().Type.ShouldEqual("wallet");
                result.First().Primary.ShouldEqual(false);
                result.First().Active.ShouldEqual(true);
            };

            private It should_have_correct_US_account_information = () =>
            {
                var usAccount = result.ElementAt(1);

                usAccount.Id.ShouldEqual(new Guid("2ae3354e-f1c3-5771-8a37-6228e9d239db"));
                usAccount.Name.ShouldEqual("USD Wallet");
                usAccount.Balance.ShouldEqual(0.00M);
                usAccount.Type.ShouldEqual("fiat");
                usAccount.Primary.ShouldEqual(false);
                usAccount.Active.ShouldEqual(true);
                usAccount.Wire_Deposit_Information.Account_Number.ShouldEqual("0199003122");
                usAccount.Wire_Deposit_Information.Routing_Number.ShouldEqual("026013356");
                usAccount.Wire_Deposit_Information.Bank_Name.ShouldEqual("Metropolitan Commercial Bank");
                usAccount.Wire_Deposit_Information.Bank_Address.ShouldEqual("99 Park Ave 4th Fl New York, NY 10016");
                usAccount.Wire_Deposit_Information.Bank_Country.Code.ShouldEqual("US");
                usAccount.Wire_Deposit_Information.Bank_Country.Name.ShouldEqual("United States");
                usAccount.Wire_Deposit_Information.Account_Name.ShouldEqual("Coinbase, Inc");
                usAccount.Wire_Deposit_Information.Account_Address.ShouldEqual(
                    "548 Market Street, #23008, San Francisco, CA 94104");
                usAccount.Wire_Deposit_Information.Reference.ShouldEqual("BAOCAEUX");
            };

            private It should_have_corret_BTC_account_information = () =>
            {
                var btcAccount = result.ElementAt(2);

                btcAccount.Id.ShouldEqual(new Guid("1bfad868-5223-5d3c-8a22-b5ed371e55cb"));
                btcAccount.Name.ShouldEqual("BTC Wallet");
                btcAccount.Currency.ShouldEqual("BTC");
                btcAccount.Type.ShouldEqual("wallet");
                btcAccount.Primary.ShouldEqual(true);
                btcAccount.Active.ShouldEqual(true);
            };

            private It should_have_correct_EU_account_information = () =>
            {
                var euAccount = result.ElementAt(3);

                euAccount.Id.ShouldEqual(new Guid("2a11354e-f133-5771-8a37-622be9b239db"));
                euAccount.Name.ShouldEqual("EUR Wallet");
                euAccount.Balance.ShouldEqual(0.00M);
                euAccount.Type.ShouldEqual("fiat");
                euAccount.Primary.ShouldEqual(false);
                euAccount.Active.ShouldEqual(true);
                euAccount.Sepa_Deposit_Information.Iban.ShouldEqual("EE957700771001355096");
                euAccount.Sepa_Deposit_Information.Swift.ShouldEqual("LHVBEE22");
                euAccount.Sepa_Deposit_Information.Bank_Name.ShouldEqual("AS LHV Pank");
                euAccount.Sepa_Deposit_Information.Bank_Address.ShouldEqual("Tartu mnt 2, 10145 Tallinn, Estonia");
                euAccount.Sepa_Deposit_Information.Bank_Country_Name.ShouldEqual("Estonia");
                euAccount.Sepa_Deposit_Information.Account_Name.ShouldEqual("Coinbase UK, Ltd.");
                euAccount.Sepa_Deposit_Information.Account_Address.ShouldEqual(
                    "9th Floor, 107 Cheapside, London, EC2V 6DN, United Kingdom");
                euAccount.Sepa_Deposit_Information.Reference.ShouldEqual("CBAEUXOVFXOXYX");
            };
        }
    }
}
