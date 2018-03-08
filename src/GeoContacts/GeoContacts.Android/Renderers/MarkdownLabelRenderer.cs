using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using GeoContacts.Controls;
using GeoContacts.Droid.Renderers;
using Java.Lang;
using Markdig;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MarkdownLabel), typeof(MarkdownLabelRenderer))]
namespace GeoContacts.Droid.Renderers
{
    public class MarkdownLabelRenderer : LabelRenderer
    {
        public MarkdownLabelRenderer()
            : base ()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            SetText();
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Label.TextProperty.PropertyName && !string.IsNullOrEmpty(Element.Text))
            {
                SetText();
            }
        }

        void SetText()
        {
            try
            {
                var htmlText = Markdown.ToHtml(Element.Text);

                ISpanned html;
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
                {
                    html = Html.FromHtml(htmlText, FromHtmlOptions.ModeCompact);
                }
                else
                {
                    // handle legacy builds
                    html = Html.FromHtml(htmlText);
                }

                Control.TextFormatted = html;
            }
            catch
            {
            }

        }
    }
}