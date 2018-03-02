using System;
using System.Collections.Generic;
using System.Text;

namespace GeoContacts.Helpers
{
    public class CommonConstants
    {
        #region AppCenter
        public const string AppCenteriOS = "AC_IOS";
        public const string AppCenterAndroid = "AC_ANDROID";
        public const string AppCenterUWP = "AC_UWP";
        public const string ShowLogin = "AC_SHOWLOGIN";
        #endregion

        #region AD
        public const string ADApplicationID = "8f999c10-2a7f-403c-b20b-cbe07b319cf3";
        public const string ADRedirectID = "msal" + ADApplicationID + "://auth";
        public static readonly string[] ADScopes = new string[] { "user.read" };
        public const string ADAuthority = "https://login.microsoftonline.com/organizations/";
        #endregion

        #region LocationFunction
        public const string FunctionUrl = "https://awesomecontactz.azurewebsites.net/api/UpdateGeolocation?code=dbn/fuOeJ460IEGCuA7M5AZoKeWQsc81haxZiHokFGazXHku/ZY3Zw==&clientId=_master";
        #endregion

        #region DodcumentDb

        public const string CDADatabaseId = "CDALocations";
        public const string AllCDACollectionId = "CDAInfo";
        public const string CDALocationCollectionId = "Location";

        public const string CosmosDbUrl = "https://awesomecontactz.documents.azure.com:443/";
        //This is a public read only key!
        public const string CosmosAuthKey = "tT95SR4OLebuny9cx9GxeghbBQhNoAEyWBpaSlOcQ5DdIdS4dN2e431r6GSwG6WM3lTKdz4djn7ldDSiUwytiQ==";
        #endregion
    }
}
