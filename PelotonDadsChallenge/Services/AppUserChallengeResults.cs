using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public class AppUserChallengeResults : IAppUserChallengeResults
    {
        private ValuesOptions _valuesOptions;

        public AppUserChallengeResults(IOptions<ValuesOptions> valuesOptions)
        {
            _valuesOptions = valuesOptions.Value;
        }

        public async Task<List<PelotonDadChallengeResult>> GetAppUserChallengeResults(string classId)
        {
            var table = await CreateTableAsync("AppUserResults");
            var results = RetrieveEntityUsingPointQueryAsync(table, classId);

            var pelotonDadsChallengeResult = new List<PelotonDadChallengeResult>();

            foreach(var result in results)
            {
                pelotonDadsChallengeResult.Add(new PelotonDadChallengeResult()
                {
                    Username = result.Username,
                    Output = result.Output,
                    AverageCadence = result.AverageCadence
                });
            }

            return pelotonDadsChallengeResult;
        }

        public List<AppUserChallengeResult> RetrieveEntityUsingPointQueryAsync(CloudTable table, string classId)
        {
            try
            {
                var query = new TableQuery<AppUserChallengeResult>().Where(TableQuery.GenerateFilterCondition("ChallengeRideId", QueryComparisons.Equal, classId));
                
                var result = table.ExecuteQuery(query);

                return new List<AppUserChallengeResult>(result);
            }
            catch (StorageException e)
            {
                throw e;
            }
        }

        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        private async Task<CloudTable> CreateTableAsync(string tableName)
        {
            string storageConnectionString = _valuesOptions.AzureWebJobsStorage;

            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
