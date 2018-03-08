using GeoContacts.Model;
using GeoContacts.Resources;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GeoContacts.ViewModel
{
    public class DetailsViewModel : ViewModelBase
    {
        public Contact Contact { get; }
        public ICommand FollowCommand { get; }
        public DetailsViewModel(Contact contact)
        {
            Contact = contact;
            FollowCommand = new Command(async () => await ExecuteFollowCommand());

            Analytics.TrackEvent("Details", new Dictionary<string, string>
            {
                ["name"] = contact.Name
            });
        }

        async Task ExecuteFollowCommand()
        {
            var social = new List<string>();
            if (!string.IsNullOrWhiteSpace(Contact.Blog))
                social.Add(nameof(Contact.Blog));
            if (!string.IsNullOrWhiteSpace(Contact.Facebook))
                social.Add(nameof(Contact.Facebook));
            if (!string.IsNullOrWhiteSpace(Contact.GitHub))
                social.Add(nameof(Contact.GitHub));
            if (!string.IsNullOrWhiteSpace(Contact.Instagram))
                social.Add(nameof(Contact.Instagram));
            if (!string.IsNullOrWhiteSpace(Contact.LinkedIn))
                social.Add(nameof(Contact.LinkedIn));
            if (!string.IsNullOrWhiteSpace(Contact.Podcast))
                social.Add(nameof(Contact.Podcast));
            if (!string.IsNullOrWhiteSpace(Contact.StackOverflow))
                social.Add(nameof(Contact.StackOverflow));
            if (!string.IsNullOrWhiteSpace(Contact.Twitch))
                social.Add(nameof(Contact.Twitch));
            if (!string.IsNullOrWhiteSpace(Contact.Twitter))
                social.Add(nameof(Contact.Twitter));

            if (social.Count == 0)
                return;

            var pick = await App.Current.MainPage.DisplayActionSheet(AppResources.ButtonFollow, AppResources.Cancel, null, social.ToArray());
            switch(pick)
            {
                case nameof(Contact.Blog):
                    ExecuteGoToSiteExtCommand(Contact.Blog);
                    break;
                case nameof(Contact.Facebook):
                    ExecuteGoToSiteExtCommand(Contact.Facebook);
                    break;
                case nameof(Contact.GitHub):
                    ExecuteGoToSiteExtCommand(Contact.GitHub);
                    break;
                case nameof(Contact.Instagram):
                    ExecuteGoToSiteExtCommand(Contact.Instagram);
                    break;
                case nameof(Contact.LinkedIn):
                    ExecuteGoToSiteExtCommand(Contact.LinkedIn);
                    break;
                case nameof(Contact.Podcast):
                    ExecuteGoToSiteExtCommand(Contact.Podcast);
                    break;
                case nameof(Contact.StackOverflow):
                    ExecuteGoToSiteExtCommand(Contact.StackOverflow);
                    break;
                case nameof(Contact.Twitch):
                    ExecuteGoToSiteExtCommand(Contact.Twitch);
                    break;
                case nameof(Contact.Twitter):
                    ExecuteGoToSiteExtCommand(Contact.Twitter);
                    break;
            }

        }
    }
}
