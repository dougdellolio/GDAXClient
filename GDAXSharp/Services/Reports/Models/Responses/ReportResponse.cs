﻿using System;
using GDAXSharp.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GDAXSharp.Services.Reports.Models.Responses
{
    public class ReportResponse
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType Type { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ReportStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public object CompletedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public object FileUrl { get; set; }

        public Params Params { get; set; }
    }

    public class Params
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
