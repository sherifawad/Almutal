using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Almutal.ViewModels
{
    [QueryProperty("SupplierId", "supplierId")]
    [QueryProperty("ServiceId", "serviceId")]

    public class MaterialsViewModel : BaseViewModel
    {

        #region Private Properties

        private string _serviceId;
        private string _supplierId;

        #endregion

        #region Public Properties

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public DateTime BuyingDate { get; set; }

        public string ServiceId
        {
            get => _serviceId;
            set
            {
                _serviceId = Uri.UnescapeDataString(value);
            }
        }
        public string SupplierId
        {
            get => _supplierId;
            set
            {
                _supplierId = Uri.UnescapeDataString(value);


                #endregion
            }
        }
    }
}