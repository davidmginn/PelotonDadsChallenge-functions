using System;
using Microsoft.Azure.Cosmos.Table;

namespace PelotonDadsChallenge.Models
{
    public class AppUserChallengeResult : TableEntity
    {
        public string Username { get; set; }
        public int Output { get; set; }
        public int AverageCadence { get; set; }
        public string ChallengeRideId { get; set; }
    }
}
