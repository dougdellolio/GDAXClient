﻿namespace GDAXSharp.Specs.JsonFixtures.Withdrawals
{
    public static class CryptoWithdrawalResponseFixture
    {
        public static string Create()
        {
            var json = @"
{
    ""id"":""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"":""10.00"",
    ""currency"": ""BTC"",
}";

            return json;
        }
    }
}
