using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using AwesomeContacts.Droid.Effects;

[assembly: ResolutionGroupName("Microsoft")]
[assembly: ExportEffect(typeof(ListViewSelectionOnTopEffect), "ListViewSelectionOnTopEffect")]
namespace AwesomeContacts.Droid.Effects
{
    public class ListViewSelectionOnTopEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var listView = Control as AbsListView;

                if (listView == null)
                    return;

                listView.SetDrawSelectorOnTop(true);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnDetached()
        {

        }
    }
}