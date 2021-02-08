using FileHelpers;

namespace PelotonDadsChallenge.Models
{
    [DelimitedRecord(",")]
    public class PelotonDadChallengeResult
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public decimal Output { get; set; }
    }
}
