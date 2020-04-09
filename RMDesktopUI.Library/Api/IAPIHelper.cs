using System.Net.Http;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticateUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);

        void LogOffUser();
    }
}