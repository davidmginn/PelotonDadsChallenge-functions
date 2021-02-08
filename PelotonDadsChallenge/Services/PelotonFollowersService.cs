using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Models;
using Flurl.Http;
using System.Collections.Generic;

namespace PelotonDadsChallenge.Services
{
    public class PelotonFollowersService : IPelotonFollowersService
    {
        private IPelotonAuthenticationService _pelotonAuthenticationService;
        private PelotonOptions _pelotonOptions;

        public PelotonFollowersService(IPelotonAuthenticationService pelotonAuthenticationService,
            IOptions<PelotonOptions> pelotonOptions)
        {
            _pelotonAuthenticationService = pelotonAuthenticationService;
            _pelotonOptions = pelotonOptions.Value;
        }

        public async Task<List<PelotonFollower>> GetPelotonFollowers()
        {

            using (var session = new CookieSession(_pelotonOptions.BaseUri))
            {
                var authResult = await _pelotonAuthenticationService.Authenticate(session);

                var followers = new List<PelotonFollower>();

                var getFollowers = true;
                var page = 0;

                while (getFollowers)
                {
                    var uri = $"{_pelotonOptions.BaseUri}/api/user/{_pelotonOptions.FollowerAccountUserId}/followers?page={page}";

                    var result = await session.Request(uri).GetJsonAsync<PelotonFollowersResponse>();

                    followers.AddRange(result.Data);

                    if (result.ShowNext)
                        page++;
                    else
                        getFollowers = false;
                }

                return followers;
            }
        }
    }
}
