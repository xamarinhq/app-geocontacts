using System;
using Microsoft.Identity.Client;
using GeoContacts.Helpers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace GeoContacts
{
    public class AuthenticationService : IAuthenticationService
    {
        public static UIParent UIParent = null;

        PublicClientApplication authClient;

        void Init()
        {
            if (authClient != null)
                return;

            authClient = new PublicClientApplication(CommonConstants.ADApplicationID, CommonConstants.ADAuthority);
            authClient.ValidateAuthority = false;
            authClient.RedirectUri = CommonConstants.ADRedirectID;
        }

        public async Task<AuthenticationResult> Login()
        {
            AuthenticationResult result = null;

            Init();

            try
            {
                result = await SilentLogin();

                if (result != null)
                    return result;

                result = await authClient.AcquireTokenAsync(CommonConstants.ADScopes, UIParent);
                var scope = result.Scopes.FirstOrDefault();
            }
            catch (MsalServiceException ex)
            {
                Debug.WriteLine($"Error occurred during the webview displayed - most likely a cancel. {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** UNIDENTIFIED ERROR: {ex.Message}");
            }

            return result;
        }

        public async Task<AuthenticationResult> SilentLogin()
        {
            Init();

            AuthenticationResult result = null;

            try
            {
                result = await authClient.AcquireTokenSilentAsync(CommonConstants.ADScopes, authClient.Users.FirstOrDefault());
            }
            catch (Exception ex)
            {
                // Most likely means the user hasn't been found
                Debug.WriteLine($"*** Error: {ex?.Message}");
            }

            return result;
        }

        public async Task<bool> IsLoggedIn()
        {
            Init();

            var result = await SilentLogin();

            return result != null;
        }

        public void Logout()
        {
            Init();

            foreach (var user in authClient.Users)
            {
                authClient.Remove(user);
            }
        }
    }
}
