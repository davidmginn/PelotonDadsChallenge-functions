namespace PelotonDadsChallenge.Configuration
{
    public class PelotonOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FollowerAccountUserId { get; set; }
        public string BaseUri { get; set; }
        public string ChallengeClassId { get; set; }
        public int ChallengeRideLength { get; set; }
        public string SendGridApiKey { get; set; }
    }
}
