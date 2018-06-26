using System;
using Microsoft.Identity.Client;
using GeoContacts.Helpers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;
using System.Text;

namespace GeoContacts
{
    public class AuthenticationService : IAuthenticationService
    {
        public static UIParent UIParent = null;

        PublicClientApplication authClient;
        string[] scopes;

        void Init()
        {
            if (authClient != null)
                return;

            if (CommonConstants.USE_MSFT)
            {
                authClient = new PublicClientApplication(CommonConstants.ADApplicationID,
                    CommonConstants.ADAuthority);
                authClient.ValidateAuthority = false;
                authClient.RedirectUri = CommonConstants.ADRedirectID;

                scopes = CommonConstants.ADScopes;
            }
            else
            {
                authClient = new PublicClientApplication(CommonConstants.B2CClientID);

                authClient.ValidateAuthority = false;
                authClient.RedirectUri = CommonConstants.B2CRedirectUrl;

                scopes = CommonConstants.B2CScopes;
            }
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

                if (CommonConstants.USE_MSFT)
                {
                    result = await authClient.AcquireTokenAsync(scopes, UIParent);
                }
                else
                {
                    result = await authClient.AcquireTokenAsync(scopes,
                                                                GetUserByPolicy(authClient.Users,
                                                                                CommonConstants.B2CPolicy),
                                                                UIBehavior.ForceLogin,
                                                                null,
                                                                null,
                                                                CommonConstants.B2CAuthority,
                                                                UIParent);

                }
            }
            catch (MsalServiceException ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine($"Error occurred during the webview displayed - most likely a cancel. {ex.Message}");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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
                if (CommonConstants.USE_MSFT)
                {
                    result = await authClient.AcquireTokenSilentAsync(scopes, authClient.Users.FirstOrDefault());
                }
                else
                {
                    result = await authClient.AcquireTokenSilentAsync(scopes,
                                                                      GetUserByPolicy(authClient.Users, CommonConstants.B2CPolicy),
                                                                        CommonConstants.B2CAuthority,
                                                                      false);
                }
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

        IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);

                if (userIdentifier.EndsWith(policy.ToLower(), StringComparison.OrdinalIgnoreCase)) return user;
            }

            return null;
        }

        string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

    }
}
