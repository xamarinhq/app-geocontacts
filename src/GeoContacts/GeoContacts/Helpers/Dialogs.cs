using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoContacts.Helpers
{
    public interface IDialogs
    {
        Task AlertAsync(string title, string message, string cancel);
    }
    public class Dialogs : IDialogs
    {
        public Task AlertAsync(string title, string message, string cancel)
        {
            if (App.Current.MainPage == null)
                return Task.CompletedTask;

            return App.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
