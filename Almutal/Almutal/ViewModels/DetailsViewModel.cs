using Almutal.Extensions;
using Almutal.Services.UnitOfWork;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almutal.ViewModels
{
    [QueryProperty("ClientId", "clientId")]
    [QueryProperty("SupplierId", "supplierId")]
    public class DetailsViewModel : BaseViewModel
    {
        #region Private Properties

        private string _clientId;
        private string _supplierId;

        #endregion

        #region Public Properties
        public IUnitOfWork _unitOfWork => DependencyService.Get<IUnitOfWork>();
        public string Name { get; set; }
        public string Date { get; set; }
        public ObservableCollection<ServiceType> ClientServiceTypes { get; set; }
        public Details Details { get; set; }

        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = Uri.UnescapeDataString(value);
            }
        }
        public string SupplierId
        {
            get => _supplierId;
            set
            {
                _supplierId = Uri.UnescapeDataString(value);
            }
        }

        #endregion

        #region Constructor
        public DetailsViewModel()
        {
            ClientServiceTypes = new ObservableCollection<ServiceType>();
        }
        #endregion

        #region Private Methods

        public override async Task InitializeAsync()
        {
            if (!string.IsNullOrEmpty(ClientId))
            {
                var clientId = Guid.Parse(ClientId);

                if (clientId != null)
                {
                    var client = await _unitOfWork.Repository<Client>().GetFirstOrDefault(x => x.Id == clientId,
                        $"{nameof(Client.Details)}" +
                        $"{nameof(Client.ClientServices)}.{nameof(ClientService.Service)}");

                    if (client != null) 
                    {
                        Name = client.Name;

                        if (client.Details != null)
                        {
                            Details = client.Details;

                        }
                        if (client.CreatedDate.HasValue)
                        {
                            Date = client.CreatedDate.Value.Date.ToString();

                        }
                        if (client.ClientServices != null)
                        {
                            client.ClientServices.ForEach(x => 
                            {
                                ClientServiceTypes.Add(x.Service);
                            });

                        }
                    }

                }


            }

            if (!string.IsNullOrEmpty(SupplierId))
            {
                var supplierId = Guid.Parse(SupplierId);

                if (supplierId != null)
                {
                    var supplier = await _unitOfWork.Repository<Supplier>().GetFirstOrDefault(x => x.Id == supplierId,
                        $"{nameof(Supplier.Details)}");

                    if (supplier != null)
                    {
                        Name = supplier.Name;

                        if (supplier.Details != null)
                        {
                            Details = supplier.Details;

                        }
                        if (supplier.CreatedDate.HasValue)
                        {
                            Date = supplier.CreatedDate.Value.Date.ToString();

                        }
                    }


                }

            }



        }

        #endregion

    }
}
