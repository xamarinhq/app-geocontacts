using System;
using GeoContacts.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MyTabsRenderer))]
namespace GeoContacts.iOS.Renderers
{
    public class MyTabsRenderer : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            if (TabBar?.Items == null)
                return;

            var tabs = Element as TabbedPage;
            if (tabs != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i], tabs.Children[i].Icon);
                }
            }

            base.ViewWillAppear(animated);
        }

        void UpdateItem(UITabBarItem item, string icon)
        {
            if (item == null)
                return;
            try
            {
                if(icon.Contains(".png"))
                    icon = icon.Replace(".png", "_selected.png");
                else
                    icon = icon + "_selected.png";
                if (item?.SelectedImage?.AccessibilityIdentifier == icon)
                    return;
                item.SelectedImage = UIImage.FromBundle(icon);
                item.SelectedImage.AccessibilityIdentifier = icon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to set selected icon: " + ex);
            }

        }
    }
}
