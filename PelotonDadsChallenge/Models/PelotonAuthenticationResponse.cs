using Newtonsoft.Json;

namespace PelotonDadsChallenge.Models
{
    public class PelotonAuthenticationResponse
    {
        [JsonProperty(PropertyName ="id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName ="username")]
        public string Username { get; set; }
    }
}
