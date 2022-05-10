using System;
using System.Threading.Tasks;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface IPersonalRecordService
    {
        Task<PersonalRecord> GetPersonalRecords(string userId);
    }
}
