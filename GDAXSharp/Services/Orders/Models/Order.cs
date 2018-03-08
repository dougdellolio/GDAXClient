﻿using GDAXSharp.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GDAXSharp.Services.Orders.Models
{
    public class Order
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderSide Side { get; set; }

        public decimal Size { get; set; }

        public decimal Price { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TimeInForce TimeInForce { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GoodTillTime CancelAfter { get; set; }

        public bool PostOnly { get; set; }
    }
}
