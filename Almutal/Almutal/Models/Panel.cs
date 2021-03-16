using DataBase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Almutal.Models
{
    public class Panel : INotifyPropertyChanged
    {
        public double? Width { get; set; }
        public double? Length { get; set; }

        public Dimension Dimension => new Dimension(Length, Width);
        public int? Count { get; set; }
        public bool EditMode { get; set; }

        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
