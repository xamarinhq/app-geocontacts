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
        //public const string ADApplicationID = "9f43032f-f539-4c7e-9639-3114d38d43ed";
        //public const string ADApplicationID = "cf6bc8d5-fd84-423b-99e0-8cd7924eeeb6";
        public const string ADApplicationID = "8f999c10-2a7f-403c-b20b-cbe07b319cf3";
        //public const string ADApplicationID = "07c72f3b-6b04-4688-8e37-c870a5b4b959";
        public const string ADRedirectID = "msal" + ADApplicationID + "://auth";
        public static readonly string[] ADScopes = new string[] { "user.read" };//api://8f999c10-2a7f-403c-b20b-cbe07b319cf3/access_as_user" };//"https://awesomecontactz.azurewebsites.net/user_impersonation" };
        public const string ADAuthority = "https://login.microsoftonline.com/organizations/";
        #endregion

    }
}
