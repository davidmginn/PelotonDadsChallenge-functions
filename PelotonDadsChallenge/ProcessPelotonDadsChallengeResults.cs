using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Models;
using PelotonDadsChallenge.Services;

namespace PelotonDadsChallenge
{
    public class ProcessPelotonDadsChallengeResults
    {
        private IPelotonFollowersService _pelotonFollowersService;
        private IPelotonWorkoutsService _pelotonWorkoutsService;
        private IPelotonWorkoutService _pelotonWorkoutService;
        private ISendGridService _sendGridService;
        private IAppUserChallengeResults _appUserChallengeResults;
        private PelotonOptions _pelotonOptions;

        public ProcessPelotonDadsChallengeResults(IPelotonFollowersService pelotonFollowersService,
            IPelotonWorkoutsService pelotonWorkoutsService,
            IPelotonWorkoutService pelotonWorkoutService,
            ISendGridService sendGridService,
            IAppUserChallengeResults appUserChallengeResults,
            IOptions<PelotonOptions> pelotonOptions)
        {
            _pelotonFollowersService = pelotonFollowersService;
            _pelotonWorkoutsService = pelotonWorkoutsService;
            _pelotonWorkoutService = pelotonWorkoutService;
            _sendGridService = sendGridService;
            _appUserChallengeResults = appUserChallengeResults;
            _pelotonOptions = pelotonOptions.Value;
        }

        [FunctionName("ProcessPelotonDadsChallengeResults")]
        public async Task Run([QueueTrigger("results")] string myQueueItem,
            [Table("personalrecords")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function executed at: {DateTime.Now}");

            var appUserChallengeResults = await _appUserChallengeResults.GetAppUserChallengeResults(_pelotonOptions.ChallengeClassId);

            var followers = await _pelotonFollowersService.GetPelotonFollowers();

            var workouts = await _pelotonWorkoutsService.GetWorkouts(followers, _pelotonOptions.ChallengeClassId);

            var challengeResults = await _pelotonWorkoutService.GetPelotonDadChallengeResults(workouts);

            foreach(var result in challengeResults)
            {
                var partionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, result.UserId);
                var rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, (_pelotonOptions.ChallengeRideLength * 60).ToString());

                var combine = TableQuery.CombineFilters(partionFilter, TableOperators.And, rowFilter);

                var query = new TableQuery<Record>().Where(combine);

                var personalRecordResult = cloudTable.ExecuteQuery(query);

                var personalRecordResultList = new List<Record>(personalRecordResult);

                var pr = personalRecordResultList.FirstOrDefault();

                if(pr != null)
                {
                    result.PersonalRecord = pr.Value;
                }
            }

            challengeResults.AddRange(appUserChallengeResults);

            await _sendGridService.EmailChallengeResults(challengeResults);

            log.LogInformation($"C# Queue trigger function completed at: {DateTime.Now}");
        }
    }
}
