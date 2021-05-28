using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Almutal.Views
{
    public class EntryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SheetTemplate { get; set; }

        public DataTemplate StripTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            throw new NotImplementedException();
        }
    }
}
