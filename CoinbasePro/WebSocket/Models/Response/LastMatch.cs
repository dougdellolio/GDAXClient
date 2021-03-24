﻿using System;
using CoinbasePro.Services.Orders.Types;

namespace CoinbasePro.WebSocket.Models.Response
{
    public class LastMatch : BaseMessage
    {
        public long TradeId { get; set; }

        public Guid MakerOrderId { get; set; }

        public Guid TakerOrderId { get; set; }

        public OrderSide Side { get; set; }

        public decimal Size { get; set; }

        public decimal Price { get; set; }

        public string ProductId { get; set; }

        public long Sequence { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}
