﻿using System.Runtime.Serialization;

namespace GDAXSharp.Services.Orders.Models
{
    public enum OrderStatus
    {
        [EnumMember(Value = "all")]
        All,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "open")]
        Open,
        [EnumMember(Value = "done")]
        Done,
        [EnumMember(Value = "settled")]
        Settled
    }
}
