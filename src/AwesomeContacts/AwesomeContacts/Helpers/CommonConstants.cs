using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeContacts.Helpers
{
    public class CommonConstants
    {
        #region AppCenter
        public const string AppCenteriOS = "AC_IOS";
        public const string AppCenterAndroid = "AC_ANDROID";
        public const string AppCenterUWP = "AC_UWP";
        #endregion

        #region AD
        public const string ADApplicationID = "9f43032f-f539-4c7e-9639-3114d38d43ed";
        public const string ADRedirectID = "msal" + ADApplicationID + "://auth";
        public static readonly string[] ADScopes = new string[] { "User.Read" };
        public const string ADAuthority = "https://login.microsoftonline.com/organizations/";
        #endregion

    }
}
