using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PelotonDadsChallenge.Models
{
    public class PelotonWorkoutsResponse
    {

        public List<PelotonWorkout> Data { get; set; }
        public int Page { get; set; }
        [JsonProperty(PropertyName = "page_count")]
        public int PageCount { get; set; }
        [JsonProperty(PropertyName = "show_next")]
        public bool ShowNext { get; set; }
    }

    public class PelotonWorkout
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public PelotonRide Ride { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
        [JsonProperty(PropertyName = "start_time")]
        public long? StartTime { get; set; }
        [JsonProperty(PropertyName = "end_time")]
        public long? EndTime { get; set; }

        public DateTimeOffset StartTimeUtc
        {
            get
            {
                if (StartTime.HasValue)
                    return DateTimeOffset.FromUnixTimeSeconds(StartTime.Value).ToUniversalTime();

                return DateTimeOffset.UtcNow;
            }
        }

        public DateTimeOffset EndTimeUtc
        {
            get
            {
                if (EndTime.HasValue)
                    return DateTimeOffset.FromUnixTimeSeconds(EndTime.Value).ToUniversalTime();

                return DateTimeOffset.UtcNow;
            }
        }
    }

    public class PelotonRide
    {
        public string Id { get; set; }
    }
}
