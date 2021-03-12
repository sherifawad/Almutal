using System;
using System.Threading.Tasks;

namespace Almutal.Services.NavigationService
{
    public interface INavigationService
    {
        Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;
        Task PushAsync(Type viewModelType, string parameters = null);
        Task PopAsync();
        Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;
        Task InsertAsRoot(Type viewModelType, string parameters = null);
        Task GoBackAsync();
        void GoToMainFlow();
        void GoToLoginFlow();
    }
}
