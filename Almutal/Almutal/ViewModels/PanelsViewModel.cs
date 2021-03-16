using Almutal.Helpers;
using DataBase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Almutal.ViewModels
{
    [QueryProperty("JsonAlgorithm", "algorithm")]
    [QueryProperty("JsonBoxes", "boxes")]
    public class PanelsViewModel : Almutal.BaseViewModel
    {

        #region Private Properties
        private Algorithm algorithm;
        private string _jsonAlgorithm;
        private string _jsonBoxes;

        #endregion

        #region Public Properties
        public string SheetsNumber { get; set; }
        public string BoxDetails { get; set; }
        public bool PopupVisible { get; set; }
        public ObservableCollection<StockSheet> Items { get; set; }
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

        #endregion

        #region Public Commands

        public ICommand DetailsCommand { get; set; }
        public ICommand DismissCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor
        public PanelsViewModel()
        {
            DetailsCommand = new RelayCommand((parameter) => PanelDetails(parameter));
            DismissCommand = new RelayCommand(() => PopupVisible = false);
            SaveCommand = new RelayCommand(async() => await SaveAsync());
            Items = new ObservableCollection<StockSheet>();

        }

        private void PanelDetails(object parameter)
        {
            if(parameter != null && parameter is Box box)
            {
                if (string.IsNullOrEmpty(box.Title) || string.IsNullOrWhiteSpace(box.Title))
                    BoxDetails = $"Width: {box.Width} Length: {box.Length}";
                else
                    BoxDetails = $"{box.Title}\nWidth: {box.Width} Length: {box.Length}";

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
            if (string.IsNullOrEmpty(JsonAlgorithm) || string.IsNullOrEmpty(JsonBoxes))
                return;

            try
            {
                var panels = JsonConvert.DeserializeObject<List<Box>>(JsonBoxes);
                var alg = JsonConvert.DeserializeObject<Algorithm>(JsonAlgorithm);

                if (alg == null || panels == null || panels.Count == 0)
                    return;

                algorithm = new Algorithm(alg.Width, alg.Length, panels, alg.Kerf);

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

        #endregion
    }
}
