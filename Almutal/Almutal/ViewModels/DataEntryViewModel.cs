using Almutal.Helpers;
using Almutal.Models;
using DataBase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Almutal.ViewModels
{
    public class DataEntryViewModel : Almutal.BaseViewModel
    {
        #region Private Properties

        private StockSheet Sheet;

        #endregion

        #region Public Properties
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
        public string Title { get; set; }

        #endregion

        #region Public Commands
        public ICommand SetSheetDimensionsCommand { get; set; }
        public ICommand SheetEditCommand { get; set; }
        public ICommand SetBoxDimensionsCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RemoveBoxCommand { get; set; }
        public ICommand AddBoxCommand { get; set; }
        public ICommand CalculateCommand { get; set; }

        #endregion

        #region Constructor
        public DataEntryViewModel()
        {
            Panels = new ObservableCollection<Panel>();
            SetSheetDimensionsCommand = new RelayCommand(() => SetSheetDimensions());
            SheetEditCommand = new RelayCommand((parameter) => EditSheet(parameter));
            SetBoxDimensionsCommand = new RelayCommand((parameter) => SetBoxDimensions(parameter));
            EditCommand = new RelayCommand((parameter) => Edit(parameter));
            RemoveBoxCommand = new RelayCommand((parameter) => Remove(parameter));
            AddBoxCommand = new RelayCommand(() => Add());
            CalculateCommand = new RelayCommand(async() => await Calculate());
            Sheet = new StockSheet { Dimension = new Dimension(0, 0) };
            SheetEditMode = true;
            MaxNumber = 1;
        }

        #endregion

        #region Command Methods

        private void EditSheet(object parameter)
        {
            SheetEditMode = true;
            CanAddPanels = false;
        }

        private void SetSheetDimensions()
        {
            if (IsSheetWidthValid && IsSheetLengthValid && IsKerfValid)
            {
                if (SheetWidth != null && SheetLength != null &&
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
                    if (panel.Width > 0 && panel.Length > 0 && panel.Count > 0)
                    {
                        if (Sheet.Dimension.canHold(new Dimension(panel.Length, panel.Width)))
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

        private void Add()
        {
            Panels.Add(new Panel { EditMode = true });
        }

        private async Task Calculate()
        {
            if (Panels.Count <= 0 || !SheetLength.HasValue || SheetLength == 0 ||
                !SheetWidth.HasValue || SheetWidth == 0 || !KerfWidth.HasValue)
                return;

            var boxes = new List<Box>();
            try
            {
                foreach (var panel in Panels)
                {
                    for (int i = 0; i < panel.Count; i++)
                    {
                        boxes.Add(new Box((double)panel.Length, (double)panel.Width, panel.Title));

                    }
                }
                if (boxes.Count <= 0)
                    return;

                var jsonBoxes = JsonConvert.SerializeObject(boxes);

                var algorithm = new Algorithm((double)SheetWidth, (double)SheetLength, boxes, (double)KerfWidth);

                var jsonString = JsonConvert.SerializeObject(algorithm);

                await _navigationService.PushAsync<PanelsViewModel>($"algorithm={jsonString}&boxes={jsonBoxes}");
            }
            catch (Exception ex)
            {
               await _dialogService.DisplayAlert("Error", ex.Message, "OK");
            }


        }

        #endregion

    }
}
