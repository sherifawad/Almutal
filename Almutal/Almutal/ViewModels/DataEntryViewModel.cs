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
using Xamarin.Forms;

namespace Almutal.ViewModels
{
    [QueryProperty("IsSheetCut", "issheetcut")]
    public class DataEntryViewModel : BaseViewModel
    {
        #region Private Properties

        private StockSheet Sheet;
        private StockStrip _stockStrip;
        private string _isSheetCut;

        #endregion

        #region Public Properties

        public bool CanAdd => CanAddPanels || CanAddStrips;

        public ObservableCollection<StripModel> StripList { get; set; }

        public bool SheetDataEntry { get; set; }
        public string BarTitle { get; set; }
        public double? BarLength { get; set; }
        public double? CutterEndWidth { get; set; }
        public double? BladeWidth { get; set; }
        public int? Bins { get; set; }
        public bool IsStripLengthValid { get; set; }
        public bool IsCutterEndWidthValid { get; set; }
        public bool IsBarLengthValid { get; set; }
        public bool IsBinsValid { get; set; }
        public bool IsBladeWidthValid { get; set; }
        public bool IsStripCountValid { get; set; }
        public bool CanAddStrips { get; set; }
        public bool StripEditMode { get; set; }
        public double StripMaxNumber { get; set; }

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

        public string IsSheetCut
        {
            get => _isSheetCut;
            set
            {
                _isSheetCut = Uri.UnescapeDataString(value);
            }
        }

        #endregion

        #region Public Commands
        public ICommand SetSheetDimensionsCommand { get; set; }
        public ICommand SetStripCommand { get; set; }
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
            StripList = new ObservableCollection<StripModel>();
            SetSheetDimensionsCommand = new RelayCommand(() => SetSheetDimensions());
            SetStripCommand = new RelayCommand(() => SetStrip());
            SheetEditCommand = new RelayCommand((parameter) => EditSheet(parameter));
            SetBoxDimensionsCommand = new RelayCommand((parameter) => SetBoxDimensions(parameter));
            EditCommand = new RelayCommand((parameter) => Edit(parameter));
            RemoveBoxCommand = new RelayCommand((parameter) => Remove(parameter));
            AddBoxCommand = new RelayCommand(() => Add());
            CalculateCommand = new RelayCommand(async () => await Calculate());
            Sheet = new StockSheet { Dimension = new Dimension(0, 0) };
            SheetEditMode = true;
            StripEditMode = true;
            MaxNumber = 1;
            StripMaxNumber = 1;
        }
        #endregion

        #region Command Methods
        private void SetStrip()
        {
            if (BarLength.HasValue && BarLength != 0 &&
                CutterEndWidth.HasValue &&
                BladeWidth.HasValue && BarLength > (CutterEndWidth + BladeWidth))
            {
                _stockStrip = new StockStrip { Length = (double)BarLength, Title = BarTitle };
                StripEditMode = false;
                if (StripList.Count > 0)
                {
                    foreach (var item in StripList.ToList())
                    {
                        if (_stockStrip.Length < item.Length)
                            StripList.Remove(item);

                    }
                }

                CanAddStrips = true;

                StripMaxNumber = _stockStrip.Length;
            }

        }
        private void EditSheet(object parameter)
        {
            if (SheetDataEntry)
            {
                SheetEditMode = true;
                CanAddPanels = false;
            }
            else
            {
                StripEditMode = true;
                CanAddStrips = false;
            }

        }
        private void SetSheetDimensions()
        {
            if (SheetWidth.HasValue && SheetLength.HasValue &&
                SheetWidth != 0 && SheetLength != 0 &&
                KerfWidth.HasValue && KerfWidth < SheetWidth && KerfWidth < SheetLength)
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
        private void Edit(object parameter)
        {
            if (parameter != null && parameter is Panel panel)
            {
                panel.EditMode = true;
            }

            if (parameter != null && parameter is StripModel strip)
            {
                strip.EditMode = true;
            }
        }
        private void SetBoxDimensions(object parameter)
        {
            if (parameter != null && parameter is Panel panel)
            {
                if (panel.Width.HasValue && panel.Width > 0 && panel.Length.HasValue && panel.Length > 0 && panel.Count.HasValue && panel.Count > 0 )
                {
                    if (Sheet.Dimension.canHold(new Dimension(panel.Length, panel.Width)))
                    {
                        panel.EditMode = false;
                        panel.DimensionError = false;

                    }
                    else
                        panel.DimensionError = true;
                }
            }

            if (parameter != null && parameter is StripModel strip)
            {
                if (IsStripLengthValid && IsStripCountValid)
                {
                    if (strip.Length > 0 && strip.Count > 0)
                    {
                        if (_stockStrip.Length >= strip.Length)
                            strip.EditMode = false;
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

            if (parameter != null && parameter is StripModel strip)
            {
                StripList.Remove(strip);
            }
        }
        private void Add()
        {
            SheetDataEntry = true;
            if (SheetDataEntry)
            {
                Panels.Add(new Panel { EditMode = true });
            }
            else
            {
                StripList.Add(new StripModel { EditMode = true });
            }
        }
        private async Task Calculate()
        {
            if (Panels.Count > 0)
            {

                if (!SheetLength.HasValue || SheetLength == 0 ||
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
            else if(StripList.Count > 0)
            {
                if (BarLength.HasValue || BarLength == 0 ||
                    !CutterEndWidth.HasValue || !BladeWidth.HasValue)
                    return;

                var strips = new List<Strip>();
                try
                {
                    foreach (var item in StripList)
                    {
                        for (int i = 0; i < item.Count; i++)
                        {
                            strips.Add(new Strip((double)item.Length, item.Title));

                        }
                    }
                    if (strips.Count <= 0)
                        return;
                    var stripAlgorithm = new StripCutAlgorithm(strips.ToArray(), (double)BarLength, (double)CutterEndWidth, (double)BladeWidth);
                    var stripPacks = stripAlgorithm.best();
                    var stripPacksString = JsonConvert.SerializeObject(stripPacks);

                    await _navigationService.PushAsync<PanelsViewModel>($"{nameof(PanelsViewModel.Strips)}={stripPacksString}");
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        #endregion

        #region Private Methods

        public override async Task InitializeAsync()
        {
            SheetDataEntry = IsSheetCut == "true" ? true : false;
            await Task.FromResult(true);
        }

        #endregion

    }
}
