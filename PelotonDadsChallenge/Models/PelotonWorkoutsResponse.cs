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
        public string Id { get; set; }
        public PelotonRide Ride { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class PelotonRide
    {
        public string Id { get; set; }
    }
}
