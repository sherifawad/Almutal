using Almutal.Helpers;
using Almutal.Models;
using AlmutalCore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Almutal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataEntryView : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<Panel> Panels { get; set; }
        public bool IsWidthValid { get; set; } = false;
        public bool IsLengthValid { get; set; } = false;
        public bool IsCountValid { get; set; } = false;
        public ICommand SetBoxDimensionsCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RemoveBoxCommand { get; set; }
        public DataEntryView()
        {
            InitializeComponent();
            Panels = new ObservableCollection<Panel>();
            SetBoxDimensionsCommand = new RelayCommand((parameter) => SetBoxDimensions(parameter));
            EditCommand = new RelayCommand((parameter) => Edit(parameter));
            RemoveBoxCommand = new RelayCommand((parameter) => Remove(parameter));
            BindingContext = this;
        }

        private void Edit(object parameter)
        {
            if (parameter != null && parameter is Panel panel)
            {
                panel.EditMode = true;
            }
        }

        private void SetBoxDimensions(object parameter)
        {
            if (parameter != null && parameter is Panel panel)
            {
                if(IsWidthValid && IsLengthValid && IsCountValid)
                    panel.EditMode = false;
            }
        }

        private void Remove(object parameter)
        {
            if(parameter != null && parameter is Panel panel)
            {
                Panels.Remove(panel);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Panels.Add(new Panel { EditMode = true});
        }
    }
}