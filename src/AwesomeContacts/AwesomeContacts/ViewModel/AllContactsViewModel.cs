using AwesomeContacts.Model;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AwesomeContacts.ViewModel
{
    public class AllContactsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Contact> Contacts { get; }

        public ICommand RefreshCommand { get; }
        public ICommand ForceRefreshCommand { get; }

        public AllContactsViewModel()
        {
            Contacts = new ObservableRangeCollection<Contact>();
            RefreshCommand = new Command(() => ExecuteRefreshCommand(false));
            ForceRefreshCommand = new Command(() => ExecuteRefreshCommand(true));
        }

        void ExecuteRefreshCommand(bool forceRefresh)
        {
            try
            {
                var contacts = DataService.GetAll();
                if (contacts != null && contacts.Count() > 0)
                    Contacts.ReplaceRange(contacts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"*** ERROR: {ex.Message}");
            }
        }

    }
}
