using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using GeoContacts;
using GeoContacts.Droid;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Content.Res;
using Android.Content;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MyTabsRenderer))]
namespace GeoContacts.Droid
{
    public class MyTabsRenderer : TabbedPageRenderer
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public MyTabsRenderer()
        {

        }
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
        public MyTabsRenderer(Context context)
        {

        }
#pragma warning restore CS0618 // Type or member is obsolete

        bool setup;
        ViewPager pager;
        TabLayout layout;
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (setup)
                return;

            if (e.PropertyName == "Renderer")
            {
                pager = (ViewPager)ViewGroup.GetChildAt(0);
                layout = (TabLayout)ViewGroup.GetChildAt(1);
                setup = true;

                ColorStateList colors = null;
                if ((int)Build.VERSION.SdkInt >= 23)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    colors = Resources.GetColorStateList(Resource.Color.icon_tab, Forms.Context.Theme);
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    colors = Resources.GetColorStateList(Resource.Color.icon_tab);
#pragma warning restore CS0618 // Type or member is obsolete
                }

                for (int i = 0; i < layout.TabCount; i++)
                {
                    var tab = layout.GetTabAt(i);
                    var icon = tab.Icon;
                    if (icon != null)
                    {
                        icon = Android.Support.V4.Graphics.Drawable.DrawableCompat.Wrap(icon);
                        Android.Support.V4.Graphics.Drawable.DrawableCompat.SetTintList(icon, colors);
                    }
                }

            }
        }
    }
}