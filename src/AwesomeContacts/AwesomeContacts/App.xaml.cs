using AwesomeContacts.Helpers;
using AwesomeContacts.Resources;
using AwesomeContacts.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Device = Xamarin.Forms.Device;
using AwesomeContacts.View;

namespace AwesomeContacts
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var culture = CrossMultilingual.Current.DeviceCultureInfo;
            AppResources.Culture = culture;

            //DependencyService.Register<IDataService, MockDataService>();
            DependencyService.Register<IDataService, AzureDataService>();
            DependencyService.Register<IDialogs, Dialogs>();
            DependencyService.Register<IAuthenticationService, AuthenticationService>();

            MonkeyCache.FileStore.Barrel.ApplicationId = "AwesomeContacts";

            if (Settings.Current.LoggedInMSFT)
            {
                GoHome();
            }
            else
                MainPage = new NavigationPage(new LoginPage());
        }

        public static void GoHome()
        {
            if(Device.RuntimePlatform == Device.iOS)
            {
                Current.MainPage = new HomePageiOS();
            }
            else
            {
                Current.MainPage = new NavigationPage(new HomePage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if ((Device.RuntimePlatform == Device.Android && CommonConstants.AppCenterAndroid != "AC_ANDROID") ||
                (Device.RuntimePlatform == Device.iOS && CommonConstants.AppCenteriOS != "AC_IOS") ||
                (Device.RuntimePlatform == Device.UWP && CommonConstants.AppCenterUWP != "AC_UWP"))
            {
                AppCenter.Start($"android={CommonConstants.AppCenterAndroid};" +
                   $"uwp={CommonConstants.AppCenterUWP};" +
                   $"ios={CommonConstants.AppCenteriOS}",
                   typeof(Analytics), typeof(Crashes), typeof(Distribute));
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
