using Almutal.Helpers;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Almutal.ViewModels
{
    public class ClientsViewModel : BaseViewModel
    {
        #region Public Properties

        public ObservableCollection<Client> Clients { get; set; }

        #endregion

        #region Public Commands

        public ICommand LoadCommands { get; set; }
        public ICommand SelectCommand { get; set; }
        public ICommand AddCommand { get; set; }
        #endregion

        #region Constructor

        public ClientsViewModel()
        {
            Clients = new ObservableCollection<Client>();
            LoadCommands = new RelayCommand(async () => await LoadAsync());
            SelectCommand = new RelayCommand(async (parameter) => await SelectAsync(parameter));
        }

        private async Task SelectAsync(object parameter)
        {
            //if (parameter == null && parameter is Client client)
            //    await _navigationService;

        }

        public override async Task InitializeAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await _unitOfWork.Repository<Client>().GetAllAsync();
        }
        #endregion
    }
}
