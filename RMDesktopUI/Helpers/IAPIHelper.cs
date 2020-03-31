using System.Threading.Tasks;
using RMDesktopUI.Model;

namespace RMDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticateUser> Authenticate(string username, string password);
    }
}