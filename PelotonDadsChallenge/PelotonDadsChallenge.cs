using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Services;

namespace PelotonDadsChallenge
{
    public class PelotonDadsChallenge
    {
        private IPelotonFollowersService _pelotonFollowersService;
        private IPelotonWorkoutsService _pelotonWorkoutsService;
        private IPelotonWorkoutService _pelotonWorkoutService;
        private ISendGridService _sendGridService;
        private PelotonOptions _pelotonOptions;

        public PelotonDadsChallenge(IPelotonFollowersService pelotonFollowersService,
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


        [FunctionName("PelotonDadsChallenge")]
        public async Task Run([TimerTrigger("0 */15 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var followers = await _pelotonFollowersService.GetPelotonFollowers();

            var workouts = await _pelotonWorkoutsService.GetWorkouts(followers, _pelotonOptions.ChallengeClassId);

            var challengeResults = await _pelotonWorkoutService.GetPelotonDadChallengeResults(workouts);

            await _sendGridService.EmailChallengeResults(challengeResults);

            log.LogInformation($"C# Timer trigger function completed at: {DateTime.Now}");
        }
    }
}
