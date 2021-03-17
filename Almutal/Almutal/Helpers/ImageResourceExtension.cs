using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Almutal
{
    [ContentProperty(nameof(Source))]

    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;

            string imagesFolderName = "Images";
            string imagePath = $"{App.Current.GetType().Namespace}.{imagesFolderName}.{Source}";
            var imageSource = ImageSource.FromResource(imagePath, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
            //return ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }
    }
}
