using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


using GeoContacts.Controls;
using GeoContacts.iOS.Renderers;
using Foundation;
using Markdig;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MarkdownLabel), typeof(MarkdownLabelRenderer))]
namespace GeoContacts.iOS.Renderers
{
    public class MarkdownLabelRenderer : LabelRenderer
    {
        public MarkdownLabelRenderer()
            : base()
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
                var htmlData = NSData.FromString(htmlText);
                if (htmlData != null && htmlData.Length > 0)
                {
                    NSAttributedString attributedString = null;
                    var error = new NSError();
                    attributedString = new NSAttributedString(htmlData, new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML, StringEncoding = NSStringEncoding.UTF8 }, ref error);

                    Control.AttributedText = attributedString;
                }
            }
            catch
            {
            }
        }
    }
}