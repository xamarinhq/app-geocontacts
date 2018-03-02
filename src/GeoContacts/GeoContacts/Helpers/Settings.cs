using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GeoContacts.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public class Settings : BaseViewModel
    {
        private static ISettings AppSettings => CrossSettings.Current;
        static Settings settings;
        public static Settings Current =>
          settings ?? (settings = new Settings());

        public bool InGuestMode
        {
            get => AppSettings.GetValueOrDefault(nameof(InGuestMode), false);
            set
            {
                var original = InGuestMode;
                if (AppSettings.AddOrUpdateValue(nameof(InGuestMode), value))
                    SetProperty(ref original, value);
            }
        }

        public bool LoggedInMSFT
        {
            get => AppSettings.GetValueOrDefault(nameof(LoggedInMSFT), false);
            set
            {
                var original = LoggedInMSFT;
                if (AppSettings.AddOrUpdateValue(nameof(LoggedInMSFT), value))
                    SetProperty(ref original, value);
            }
        }

    }
}
