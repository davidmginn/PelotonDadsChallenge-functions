using System.Collections.Generic;
using Newtonsoft.Json;

namespace PelotonDadsChallenge.Models
{
    public class PelotonFollowersResponse
    {
        public List<PelotonFollower> Data { get; set; }
        public int Page { get; set; }
        [JsonProperty(PropertyName = "page_count")]
        public int PageCount { get; set; }
        [JsonProperty(PropertyName = "show_next")]
        public bool ShowNext { get; set; }
    }

    public class PelotonFollower
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }
}
