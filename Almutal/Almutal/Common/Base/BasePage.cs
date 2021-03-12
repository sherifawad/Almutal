using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Almutal
{
    public class BasePage : ContentPage
    {
        private object ViewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null && ViewModel is BaseViewModel vm)
                await vm.InitializeAsync();

        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null && ViewModel is BaseViewModel vm)
                await vm.UninitializeAsync();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ViewModel = BindingContext;

        }
        public BasePage()
        {
            //Content = new StackLayout
            //{
            //    Children = {
            //        new Label { Text = "Welcome to Xamarin.Forms!" }
            //    }
            //};
        }
    }
}