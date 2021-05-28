using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Almutal.Models
{
    public class StripModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Title { get; set; }
        public double? Length { get; set; }
        public int? Count { get; set; }
        public bool EditMode { get; set; }

    }
}
