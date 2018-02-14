using System;
using Microsoft.Identity.Client;
using AwesomeContacts.Helpers;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AwesomeContacts
{
    public static class AuthenticationService
    {
        public static UIParent UIParent = null;

        static PublicClientApplication authClient;

        static void Init()
        {
            if (authClient != null)
                return;

            authClient = new PublicClientApplication(CommonConstants.ADApplicationID, "https://login.microsoftonline.com/organizations/");
            authClient.ValidateAuthority = false;
            authClient.RedirectUri = CommonConstants.ADRedirectID;
        }

        public async static Task<string> Login()
        {
            string authToken = "";

            Init();

            try
            {
                var loginResult = await authClient.AcquireTokenAsync(CommonConstants.ADScopes, UIParent);

                Debug.WriteLine($"The id token: {loginResult.IdToken}");
            }
            catch (MsalServiceException ex)
            {
                Debug.WriteLine($"*** ERROR!: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** ERROR: {ex.Message}");
            }

            return authToken;
        }

        public static bool IsLoggedIn()
        {
            Init();
            return false;
        }

        public static void Logout()
        {
            Init();
        }
    }
}
