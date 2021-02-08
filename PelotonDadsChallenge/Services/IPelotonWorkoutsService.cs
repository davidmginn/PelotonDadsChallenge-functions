using System.Collections.Generic;
using System.Threading.Tasks;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface IPelotonWorkoutsService
    {
        Task<List<PelotonWorkout>> GetWorkouts(IEnumerable<PelotonFollower> users, string classId);
    }
}
