using System.Collections.Generic;
using System.Threading.Tasks;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface ISendGridService
    {
        Task EmailChallengeResults(IEnumerable<PelotonDadChallengeResult> results);
    }
}
