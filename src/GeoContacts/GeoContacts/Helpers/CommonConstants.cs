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
        public const string FaceApiKey = "AC_FACE";
        #endregion

        #region AD
        public const bool USE_MSFT = true;

        public const string ADApplicationID = "8f999c10-2a7f-403c-b20b-cbe07b319cf3";
        public const string ADRedirectID = "msal" + ADApplicationID + "://auth";
        public static readonly string[] ADScopes = new string[] { "user.read" };
        public const string ADAuthority = "https://login.microsoftonline.com/organizations/";

        public const string B2CTenant = "geocontacts.onmicrosoft.com";
        public const string B2CClientID = "8aea67fd-38da-49a1-8f68-ca0c9e9e2501";
        public const string B2CPolicy = "B2C_1_SignInUp";

        public const string B2CAuthorityBase = "https://login.microsoftonline.com/tfp/" + B2CTenant + "/";
        public const string B2CAuthority = B2CAuthorityBase + B2CPolicy;

        public const string B2CRedirectUrl = "msal" + B2CClientID + "://auth";
        public static readonly string[] B2CScopes = new string[] { "https://geocontacts.onmicrosoft.com/geocontacts/login" };

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
