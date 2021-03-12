using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almutal.Services.NavigationService
{
    public class ShellRoutingService : INavigationService
    {
        public void GoToMainFlow()
        {
            Application.Current.MainPage = new AppShell();
        }

        public void GoToLoginFlow()
        {
            
        }

        public async Task PopAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel
        {
            return GoToAsync("//", typeof(TViewModel), parameters);
        }

        public Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel
        {
            return GoToAsync("", typeof(TViewModel), parameters);
        }

        private async Task GoToAsync(string routePrefix, Type TViewModel, string parameters)
        {
            //Page page = CreatePage(TViewModel);
            //if (page.BindingContext is null && ViewModelLocator.GetAutowireViewModel(page) is null)
            //    ViewModelLocator.OnAutoWireViewModelChanged(page, true);

            var route = routePrefix + TViewModel.Name;
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                route += $"?{parameters}";
            }

            //var viewModel = Activator.CreateInstance<TViewModel>();
            //page.BindingContext = viewModel;

            //await (page.BindingContext as BaseViewModel).InitializeAsync(false);


            await Shell.Current.GoToAsync(route);
        }


        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        public Task PushAsync(Type viewModelType, string parameters = null)
        {
            return GoToAsync("", viewModelType, parameters);
        }

        public Task InsertAsRoot(Type viewModelType, string parameters = null)
        {
            return GoToAsync("//", viewModelType, parameters);
        }
    }
}
