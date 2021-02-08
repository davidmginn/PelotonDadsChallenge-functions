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
    public class PelotonWorkoutsService : IPelotonWorkoutsService
    {
        private IPelotonAuthenticationService _pelotonAuthenticationService;
        private PelotonOptions _pelotonOptions;
        private ILogger<PelotonWorkoutService> _logger;

        public PelotonWorkoutsService(IPelotonAuthenticationService pelotonAuthenticationService,
            IOptions<PelotonOptions> pelotonOptions,
            ILogger<PelotonWorkoutService> logger)
        {
            _pelotonAuthenticationService = pelotonAuthenticationService;
            _pelotonOptions = pelotonOptions.Value;
            _logger = logger;
        }

        public async Task<List<PelotonWorkout>> GetWorkouts(IEnumerable<PelotonFollower> users, string classId)
        {
            using (var session = new CookieSession(_pelotonOptions.BaseUri))
            {
                var authResult = await _pelotonAuthenticationService.Authenticate(session);

                var workouts = new List<PelotonWorkout>();

                foreach(var user in users)
                {
                    try
                    {
                        var uri = $"{_pelotonOptions.BaseUri}/api/user/{user.Id}/workouts?joins=ride&limit=10&page=0";
                        var result = await session.Request(uri).GetJsonAsync<PelotonWorkoutsResponse>();

                        var workout = result.Data.Where(x => x.Ride.Id == classId).FirstOrDefault();

                        if (workout != null)
                        {
                            workout.UserName = user.Username;
                            workouts.Add(workout);
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, $"Error retrieving data for User Id: {user.Id}");
                    }
                }

                return workouts;
            }
        }
    }
}
