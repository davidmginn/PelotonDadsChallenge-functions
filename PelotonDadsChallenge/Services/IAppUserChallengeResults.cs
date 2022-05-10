using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface IAppUserChallengeResults
    {
        Task<List<PelotonDadChallengeResult>> GetAppUserChallengeResults(string classId);
    }
}
