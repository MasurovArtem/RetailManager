using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        Task<AuthenticateUser> Authenticate(string username, string password);
        Task GetLoginUserInfo(string token);
    }
}