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
        private StockSheet Sheet;
        public ObservableCollection<Panel> Panels { get; set; }
        public double? SheetWidth { get; set; }
        public double? SheetLength { get; set; }
        public double? KerfWidth { get; set; }
        public double MaxNumber { get; set; }
        public bool CanAddPanels { get; set; }
        public bool SheetEditMode { get; set; }
        public bool IsSheetWidthValid { get; set; }
        public bool IsSheetLengthValid { get; set; }
        public bool IsWidthValid { get; set; }
        public bool IsKerfValid { get; set; }
        public bool IsLengthValid { get; set; }
        public bool IsCountValid { get; set; }
        public ICommand SetSheetDimensionsCommand { get; set; }
        public ICommand SheetEditCommand { get; set; }
        public ICommand SetBoxDimensionsCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RemoveBoxCommand { get; set; }
        public DataEntryView()
        {
            InitializeComponent();
            Panels = new ObservableCollection<Panel>();
            SetSheetDimensionsCommand = new RelayCommand(() => SetSheetDimensions());
            SheetEditCommand = new RelayCommand((parameter) => EditSheet(parameter));
            SetBoxDimensionsCommand = new RelayCommand((parameter) => SetBoxDimensions(parameter));
            EditCommand = new RelayCommand((parameter) => Edit(parameter));
            RemoveBoxCommand = new RelayCommand((parameter) => Remove(parameter));
            Sheet = new StockSheet { Dimension = new Dimension(0,0) };
            SheetEditMode = true;
            MaxNumber = 1;
            BindingContext = this;
        }

        private void EditSheet(object parameter)
        {
            SheetEditMode = true;
            CanAddPanels = false;
        }

        private void SetSheetDimensions()
        {
                if (IsSheetWidthValid && IsSheetLengthValid && IsKerfValid)
                {
                    if(SheetWidth != null && SheetLength != null &&
                        SheetWidth != 0 && SheetLength != 0 &&
                        KerfWidth != null)
                    {
                    Sheet = new StockSheet { Dimension = new Dimension(SheetLength, SheetWidth) };
                        SheetEditMode = false;
                        if (Panels.Count > 0)
                        {
                            foreach (var item in Panels.ToList())
                            {
                                if (!Sheet.Dimension.canHold(item.Dimension))
                                    Panels.Remove(item);

                            }
                        }

                        CanAddPanels = true;

                        MaxNumber = Math.Max((double)SheetLength, (double)SheetWidth);
                    }
                }
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
                if (IsWidthValid && IsLengthValid && IsCountValid)
                {
                    if(panel.Width > 0 && panel.Length > 0 && panel.Count > 0)
                    {
                        if(Sheet.Dimension.canHold(new Dimension(panel.Length, panel.Width)))
                            panel.EditMode = false;
                    }
                }
            }
        }

        private void Remove(object parameter)
        {
            if (parameter != null && parameter is Panel panel)
            {
                Panels.Remove(panel);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Panels.Add(new Panel { EditMode = true });
        }
    }
}