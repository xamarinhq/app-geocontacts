using System;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
namespace AwesomeContacts
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Login();
        Task<AuthenticationResult> SilentLogin();
        Task<bool> IsLoggedIn();
        void Logout();
    }
}
