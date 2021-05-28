using Almutal.Models;
using Almutal.ViewModels;
using DataBase.Models;
using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Almutal.Views
{
    public partial class NewItemPage : ContentPage
    {
        private NewItemViewModel _viewModel;
        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewItemViewModel();
        }

        #region Storage Suggestionbox

        private void supplierSuggestBox_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {

            if (e.SelectedItem != null)
            {
                var value = e.SelectedItem as Supplier;
                if (value != null)
                {
                    _viewModel.SelectedSupplier = value;

                }
            }

            //Move focus to the next control or stop focus
            if (sender is AutoSuggestBox)
            {
                (sender as AutoSuggestBox).Unfocus();
                //(sender as AutoSuggestBox).Unfocused += StorageSuggestBox_Unfocused;
            }
        }

        private void supplierSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            var list = _viewModel?.Supplieres;
            if (list == null)
                return;

            if (e.CheckCurrent() && list.Count > 0)
            {
                var term = (sender as AutoSuggestBox).Text.ToLower();
                var results = list.Where(i => i.Name.ToLower().Contains(term)).ToList();
                (sender as AutoSuggestBox).ItemsSource = results;
            }

        }
        private void SuggestBox_Focused(object sender, FocusEventArgs e)
        {
            (sender as AutoSuggestBox).ItemsSource?.Clear();
        }
        #endregion
    }
}