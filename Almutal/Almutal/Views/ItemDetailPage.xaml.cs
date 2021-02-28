using Almutal.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Almutal.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}