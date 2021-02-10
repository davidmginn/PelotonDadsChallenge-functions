using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PelotonDadsChallenge.Models
{
    public class PelotonWorkoutResponse
    {
        public List<PelotonWorkoutResponseSummary> Summaries { get; set; }
        public List<PelotonWorkoutResponseMetric> Metrics { get; set; }
    }

    public class PelotonWorkoutResponseSummary
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "display_unit")]
        public string DisplayUnit { get; set; }
        public decimal Value { get; set; }
        public string Slug { get; set; }
    }

    public class PelotonWorkoutResponseMetric
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "display_unit")]
        public string DisplayUnit { get; set; }
        [JsonProperty(PropertyName = "max_value")]
        public decimal MaxValue { get; set; }
        [JsonProperty(PropertyName = "average_value")]
        public decimal AverageValue { get; set; }
        public string Slug { get; set; }
    }
}
