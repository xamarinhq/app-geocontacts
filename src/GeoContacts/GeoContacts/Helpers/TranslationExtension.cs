using Plugin.Multilingual;
using System;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoContacts.Helpers
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string resourceId = "GeoContacts.Resources.AppResources";

        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(resourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var ci = CrossMultilingual.Current.CurrentCultureInfo;

            var translation = resmgr.Value.GetString(Text, ci);

            if (translation == null)
            {

#if DEBUG
                throw new ArgumentException(
                    $"Key '{Text}' was not found in resources '{resourceId}' for culture '{ci.Name}'.",
                    "Text");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
