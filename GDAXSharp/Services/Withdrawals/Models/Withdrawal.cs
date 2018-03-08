﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GDAXSharp.Services.Withdrawals.Models
{
    public class Withdrawal
    {
        public decimal Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Shared.Currency Currency { get; set; }

        public string PaymentMethodId { get; set; }
    }
}
