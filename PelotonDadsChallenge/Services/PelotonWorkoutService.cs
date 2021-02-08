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

                foreach(var workout in pelotonWorkouts)
                {
                    try
                    {
                        var uri = $"{_pelotonOptions.BaseUri}/api/workout/{workout.Id}/performance_graph?every_n=100";
                        var result = await session.Request(uri).GetJsonAsync<PelotonWorkoutResponse>();

                        var outputSummary = result.Summaries.Where(x => x.Slug == "total_output").FirstOrDefault();

                        if(outputSummary != null)
                        {
                            var challengeResult = new PelotonDadChallengeResult()
                            {
                                Output = outputSummary.Value,
                                UserId = workout.UserId,
                                Username = workout.UserName
                            };

                            challengeResults.Add(challengeResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error retrieving data for Workout Id: {workout.Id}");
                    }
                }

                return challengeResults;
            }
        }
    }
}
