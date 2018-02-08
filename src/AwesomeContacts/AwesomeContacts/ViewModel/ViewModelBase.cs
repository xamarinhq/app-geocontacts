using System;
using System.Collections.Generic;
using System.Text;
using AwesomeContacts.Helpers;
using AwesomeContacts.Services;
using MvvmHelpers;
using Xamarin.Forms;

namespace AwesomeContacts.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {
        public Settings Settings => Settings.Current;

        IDataService dataService;
        public IDataService DataService => dataService ?? (dataService = DependencyService.Get<IDataService>());
    }
}
