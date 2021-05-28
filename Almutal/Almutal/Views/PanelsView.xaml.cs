
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using Almutal.ViewModels;

namespace Almutal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PanelsView : BasePage
    {
        private double width = 0;
        private double height = 0;

        public PanelsView()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //BindingContext = new PanelsViewModel();
        }


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                //reconfigure layout
                UpdateLayout();
            }
        }

        void UpdateLayout()
        {

            if (width > height)
            {
                collection.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
            }
            else
            {
                collection.ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical);
            }
        }

    }
}