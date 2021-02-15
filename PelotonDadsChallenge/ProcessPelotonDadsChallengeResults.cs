using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Services;

namespace PelotonDadsChallenge
{
    public class ProcessPelotonDadsChallengeResults
    {
        private IPelotonFollowersService _pelotonFollowersService;
        private IPelotonWorkoutsService _pelotonWorkoutsService;
        private IPelotonWorkoutService _pelotonWorkoutService;
        private ISendGridService _sendGridService;
        private PelotonOptions _pelotonOptions;

        public ProcessPelotonDadsChallengeResults(IPelotonFollowersService pelotonFollowersService,
            IPelotonWorkoutsService pelotonWorkoutsService,
            IPelotonWorkoutService pelotonWorkoutService,
            ISendGridService sendGridService,
            IOptions<PelotonOptions> pelotonOptions)
        {
            _pelotonFollowersService = pelotonFollowersService;
            _pelotonWorkoutsService = pelotonWorkoutsService;
            _pelotonWorkoutService = pelotonWorkoutService;
            _sendGridService = sendGridService;
            _pelotonOptions = pelotonOptions.Value;
        }

        [FunctionName("ProcessPelotonDadsChallengeResults")]
        public async Task Run([QueueTrigger("results")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function executed at: {DateTime.Now}");

            var followers = await _pelotonFollowersService.GetPelotonFollowers();

            var workouts = await _pelotonWorkoutsService.GetWorkouts(followers, _pelotonOptions.ChallengeClassId);

            var challengeResults = await _pelotonWorkoutService.GetPelotonDadChallengeResults(workouts);

            await _sendGridService.EmailChallengeResults(challengeResults);

            log.LogInformation($"C# Queue trigger function completed at: {DateTime.Now}");
        }
    }
}
