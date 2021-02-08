using System.Collections.Generic;
using System.Threading.Tasks;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface IPelotonFollowersService
    {
        Task<List<PelotonFollower>> GetPelotonFollowers();
    }
}
