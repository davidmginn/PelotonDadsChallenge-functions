using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public class PelotonWorkoutService : IPelotonWorkoutService
    {
        private IPelotonAuthenticationService _pelotonAuthenticationService;
        private ILogger<PelotonWorkoutService> _logger;
        private PelotonOptions _pelotonOptions;

        public PelotonWorkoutService(IPelotonAuthenticationService pelotonAuthenticationService,
            ILogger<PelotonWorkoutService> logger,
            IOptions<PelotonOptions> pelotonOptions)
        {
            _pelotonAuthenticationService = pelotonAuthenticationService;
            _logger = logger;
            _pelotonOptions = pelotonOptions.Value;
        }

        public async Task<List<PelotonDadChallengeResult>> GetPelotonDadChallengeResults(IEnumerable<PelotonWorkout> pelotonWorkouts)
        {
            using (var session = new CookieSession(_pelotonOptions.BaseUri))
            {
                var authResult = await _pelotonAuthenticationService.Authenticate(session);

                var challengeResults = new List<PelotonDadChallengeResult>();

                foreach (var workout in pelotonWorkouts)
                {
                    try
                    {
                        var uri = $"{_pelotonOptions.BaseUri}/api/workout/{workout.Id}/performance_graph?every_n=100";
                        var result = await session.Request(uri).GetJsonAsync<PelotonWorkoutResponse>();

                        var challengeResult = new PelotonDadChallengeResult()
                        {
                            Output = GetSafeWorkoutSummary(result.Summaries, "total_output").Value,
                            Distance = GetSafeWorkoutSummary(result.Summaries, "distance").Value,
                            Calories = GetSafeWorkoutSummary(result.Summaries, "calories").Value,
                            MaxOutput = GetSafeWorkoutMetric(result.Metrics, "output").MaxValue,
                            AverageOutput = GetSafeWorkoutMetric(result.Metrics, "output").AverageValue,
                            MaxCadence = GetSafeWorkoutMetric(result.Metrics, "cadence").MaxValue,
                            AverageCadence = GetSafeWorkoutMetric(result.Metrics, "cadence").AverageValue,
                            MaxResistance = GetSafeWorkoutMetric(result.Metrics, "resistance").MaxValue,
                            AverageResistance = GetSafeWorkoutMetric(result.Metrics, "resistance").AverageValue,
                            MaxSpeed = GetSafeWorkoutMetric(result.Metrics, "speed").MaxValue,
                            AverageSpeed = GetSafeWorkoutMetric(result.Metrics, "speed").AverageValue,
                            MaxHeartRate = GetSafeWorkoutMetric(result.Metrics, "heart_rate").MaxValue,
                            AverageHeartRate = GetSafeWorkoutMetric(result.Metrics, "heart_rate").AverageValue,
                            UserId = workout.UserId,
                            Username = workout.UserName
                        };

                        challengeResults.Add(challengeResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error retrieving data for Workout Id: {workout.Id}");
                    }
                }

                return challengeResults;
            }

            PelotonWorkoutResponseSummary GetSafeWorkoutSummary(IEnumerable<PelotonWorkoutResponseSummary> summaries, string slug)
            {
                var summary = summaries.Where(x => x.Slug == slug).FirstOrDefault();

                if (summary == null)
                    return new PelotonWorkoutResponseSummary();

                return summary;
            }

            PelotonWorkoutResponseMetric GetSafeWorkoutMetric(IEnumerable<PelotonWorkoutResponseMetric> metrics, string slug)
            {
                var metric = metrics.Where(x => x.Slug == slug).FirstOrDefault();

                if (metric == null)
                    return new PelotonWorkoutResponseMetric();

                return metric;
            }
        }
    }
}
