using System.Threading.Tasks;
using Flurl.Http;
using PelotonDadsChallenge.Models;

namespace PelotonDadsChallenge.Services
{
    public interface IPelotonAuthenticationService
    {
        Task<PelotonAuthenticationResponse> Authenticate(CookieSession session);
    }
}
