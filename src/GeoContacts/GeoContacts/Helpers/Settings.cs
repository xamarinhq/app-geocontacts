using MvvmHelpers;
using Xamarin.Essentials;

namespace GeoContacts.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public class Settings : BaseViewModel
    {
        static Settings settings;
        public static Settings Current =>
          settings ?? (settings = new Settings());

        public bool InGuestMode
        {
            get => Preferences.Get(nameof(InGuestMode), false);
            set
            {
                var original = InGuestMode;
                Preferences.Set(nameof(InGuestMode), value);
                SetProperty(ref original, value);
            }
        }

        public bool LoggedInMSFT
        {
            get => Preferences.Get(nameof(LoggedInMSFT), false);
            set
            {
                var original = LoggedInMSFT;
                Preferences.Set(nameof(LoggedInMSFT), value);
                SetProperty(ref original, value);
            }
        }

    }
}
