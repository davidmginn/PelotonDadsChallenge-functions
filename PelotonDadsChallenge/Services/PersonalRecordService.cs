using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public class PersonalRecordService : IPersonalRecordService
    {
        private IPelotonAuthenticationService _pelotonAuthenticationService;
        private PelotonOptions _pelotonOptions;
        private ILogger<PersonalRecordService> _logger;

        public PersonalRecordService(IPelotonAuthenticationService pelotonAuthenticationService,
            IOptions<PelotonOptions> pelotonOptions,
            ILogger<PersonalRecordService> logger)
        {
            _pelotonAuthenticationService = pelotonAuthenticationService;
            _pelotonOptions = pelotonOptions.Value;
            _logger = logger;
        }

        public async Task<PersonalRecord> GetPersonalRecords(string userId)
        {
            using (var session = new CookieSession(_pelotonOptions.BaseUri))
            {
                var authResult = await _pelotonAuthenticationService.Authenticate(session);

                var uri = $"{_pelotonOptions.BaseUri}/api/user/{userId}/overview?version=1";

                try
                {
                    var result = await session.Request(uri).WithHeader("Peloton-Platform", "web").GetJsonAsync<PelotonOverviewResponse>();

                    var cyclingPRs = result.PersonalRecords.Where(x => x.Slug == "cycling").FirstOrDefault();

                    return cyclingPRs;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

                return null;

            }
        }
    }
}
