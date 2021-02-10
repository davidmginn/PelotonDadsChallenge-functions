using FileHelpers;

namespace PelotonDadsChallenge.Models
{
    [DelimitedRecord(",")]
    public class PelotonDadChallengeResult
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public decimal Output { get; set; }
        public decimal Distance { get; set; }
        public decimal Calories { get; set; }
        public decimal MaxOutput { get; set; }
        public decimal AverageOutput { get; set; }
        public decimal MaxCadence { get; set; }
        public decimal AverageCadence { get; set; }
        public decimal MaxResistance { get; set; }
        public decimal AverageResistance { get; set; }
        public decimal MaxSpeed { get; set; }
        public decimal AverageSpeed { get; set; }
        public decimal MaxHeartRate { get; set; }
        public decimal AverageHeartRate { get; set; }
    }
}
