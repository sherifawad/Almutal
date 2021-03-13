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
        public int? Count { get; set; }
        public bool EditMode { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
