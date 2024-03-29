﻿using Almutal.ViewModels;
using Almutal.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Almutal
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(DataEntryViewModel), typeof(DataEntryView));
            Routing.RegisterRoute(nameof(PanelsViewModel), typeof(PanelsView));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");

        }
    }
}
