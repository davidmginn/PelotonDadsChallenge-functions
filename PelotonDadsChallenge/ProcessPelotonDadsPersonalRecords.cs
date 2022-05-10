using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Services;

namespace PelotonDadsChallenge
{
    public class ProcessPelotonDadsPersonalRecords
    {
        private IPelotonFollowersService _pelotonFollowersService;
        private IPersonalRecordService _personalRecordService;
        private PelotonOptions _pelotonOptions;

        public ProcessPelotonDadsPersonalRecords(
            IPelotonFollowersService pelotonFollowersService,
            IPersonalRecordService personalRecordService,
            IOptions<PelotonOptions> pelotonOptions)
        {
            _pelotonFollowersService = pelotonFollowersService;
            _personalRecordService = personalRecordService;
            _pelotonOptions = pelotonOptions.Value;
        }

        [FunctionName("ProcessPelotonDadsPersonalRecords")]
        public async Task Run([QueueTrigger("prs")] string myQueueItem,
            [Table("personalrecords")]CloudTable cloudTable,
            ILogger log)
        {
            var followers = await _pelotonFollowersService.GetPelotonFollowers();

            foreach(var follower in followers)
            {
                var prs = await _personalRecordService.GetPersonalRecords(follower.Id);

                if(prs != null)
                {
                    foreach (var record in prs.Records)
                    {
                        record.PartitionKey = follower.Id;
                        record.RowKey = record.Slug;

                        var insertOperation = TableOperation.InsertOrReplace(record);
                        await cloudTable.ExecuteAsync(insertOperation);
                    }
                }
            }

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
