using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PelotonDadsChallenge.Models
{
    public class PelotonWorkoutResponse
    {
        public List<PelotonWorkoutResponseSummary> Summaries { get; set; }
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
}
