using Almutal.Extensions;
using Almutal.Helpers;
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
    [QueryProperty("JsonAlgorithm", "algorithm")]
    [QueryProperty("JsonBoxes", "boxes")]
    [QueryProperty(nameof(Strips), nameof(Strips))]
    public class PanelsViewModel : Almutal.BaseViewModel
    {

        #region Private Properties
        private string _jsonAlgorithm;
        private string _jsonBoxes;
        private string _strips;

        #endregion

        #region Public Properties
        public bool IsSheet { get; private set; }
        public string SheetsNumber { get; set; }
        public string BoxDetails { get; set; }
        public bool PopupVisible { get; set; }
        public ObservableCollection<StockSheet> Items { get; private set; }
        public ObservableCollection<StockStrip> StripsItems { get; private set; }
        public string JsonAlgorithm
        {
            get => _jsonAlgorithm;
            set
            {
                _jsonAlgorithm = Uri.UnescapeDataString(value);
            }
        }
        public string JsonBoxes
        {
            get => _jsonBoxes;
            set
            {
                _jsonBoxes = Uri.UnescapeDataString(value);
            }
        }
        public string Strips
        {
            get => _strips;
            set
            {
                _strips = Uri.UnescapeDataString(value);
            }
        }

        #endregion

        #region Public Commands

        public ICommand DetailsCommand { get;}
        public ICommand DismissCommand { get;}
        public ICommand SaveCommand { get;}

        #endregion

        #region Constructor
        public PanelsViewModel()
        {
            DetailsCommand = new RelayCommand((parameter) => PanelDetails(parameter));
            DismissCommand = new RelayCommand(() => PopupVisible = false);
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            Items = new ObservableCollection<StockSheet>();
            StripsItems = new ObservableCollection<StockStrip>();

        }

        private void PanelDetails(object parameter)
        {
            if (parameter != null && parameter is Box box)
            {
                if (string.IsNullOrEmpty(box.Title) || string.IsNullOrWhiteSpace(box.Title))
                    BoxDetails = $"Width: {box.Width} Length: {box.Length}";
                else
                    BoxDetails = $"{box.Title}\nWidth: {box.Width} Length: {box.Length}";

                PopupVisible = true;
            }

            if (parameter != null && parameter is Strip strip)
            {
                if (string.IsNullOrEmpty(strip.Title) || string.IsNullOrWhiteSpace(strip.Title))
                    BoxDetails = $"Length: {strip.Length}";
                else
                    BoxDetails = $"{strip.Title}\n Length: {strip.Length}";

                PopupVisible = true;
            }

        }

        private Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Commands Methods

        #endregion

        #region Private Methods

        public override async Task InitializeAsync()
        {
            if (!string.IsNullOrEmpty(JsonAlgorithm) && !string.IsNullOrEmpty(JsonBoxes))
            {
                IsSheet = true;
                try
                {

                    var panels = JsonConvert.DeserializeObject<List<Box>>(JsonBoxes);
                    var alg = JsonConvert.DeserializeObject<Algorithm>(JsonAlgorithm);

                    if (alg == null || panels == null || panels.Count == 0)
                        return;

                    var algorithm = new Algorithm(alg.Width, alg.Length, panels, alg.Kerf);

                    var boxes = algorithm.Pack();
                    if (boxes.Count > 0)
                    {
                        double boxesTotalArea = 0;
                        foreach (var sheet in boxes)
                        {
                            foreach (var box in sheet.CuttedPanels)
                            {
                                boxesTotalArea += box.Area;
                            }
                        }
                        var totalSheetsArea = algorithm.Length * algorithm.Width * boxes.Count;
                        var used = Math.Round(100 * boxesTotalArea / totalSheetsArea, 2);
                        var wast = Math.Round(100 * (1 - boxesTotalArea / totalSheetsArea), 2);
                        SheetsNumber = $"Sheets Count: {boxes.Count} \nUsed: {used}%  Wast: {wast}%";
                        foreach (var item in boxes)
                        {
                            Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {

                    await _dialogService.DisplayAlert("Error", ex.Message, "OK");
                }
            }
            if (!string.IsNullOrEmpty(Strips))
            {
                IsSheet = false;
                try
                {

                    var list = JsonConvert.DeserializeObject<List<StockStrip>>(Strips);
                    foreach (var item in list)
                    {
                        foreach (var strip in item.CuttedStrips)
                        {
                            strip.Color = RandomStringColors.GenerateColor();
                        }
                        StripsItems.Add(item);
                    }
                    //StripsItems = JsonConvert.DeserializeObject<List<StockStrip>>(Strips).ToObservableCollection();

                    double barLength = StripsItems.ToList().FirstOrDefault().Length;
                    var totalLength = barLength * StripsItems.Count;
                    double usedLength = default;

                    foreach (var stock in StripsItems)
                    {
                        foreach (var cutted in stock.CuttedStrips)
                        {
                            usedLength += cutted.Length;
                        }
                    }
                    var used = Math.Round(100 * usedLength / totalLength, 2);
                    var wast = Math.Round(100 * (1 - usedLength / totalLength), 2);
                    SheetsNumber = $"Strips Count: {StripsItems.Count} \nUsed: {used}%  Wast: {wast}%";
                }
                catch (Exception ex)
                {

                    await _dialogService.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        #endregion
    }
}
