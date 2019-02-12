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
using Android.Widget;

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

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);
            var layout = (ViewGroup)this.GetChildAt(0);
            var bottomNavigationView = (BottomNavigationView)layout.GetChildAt(1);
            var topShadow = LayoutInflater.From(Context).Inflate(Resource.Layout.top_shadow, null);

            var layoutParams =
                new Android.Widget.RelativeLayout.LayoutParams(LayoutParams.MatchParent, 15);
            layoutParams.AddRule(LayoutRules.Above, bottomNavigationView.Id);

            layout.AddView(topShadow, 2, layoutParams);

        }
    }
}