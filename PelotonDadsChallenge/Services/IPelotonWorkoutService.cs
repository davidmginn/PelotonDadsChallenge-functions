using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface IPelotonWorkoutService
    {
        Task<List<PelotonDadChallengeResult>> GetPelotonDadChallengeResults(IEnumerable<PelotonWorkout> pelotonWorkouts);
    }
}
