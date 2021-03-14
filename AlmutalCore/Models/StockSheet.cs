using AlmutalCore.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlmutalCore.Models
{
    public class StockSheet
    {
        public double? Thickness { get; set; }
        public double? KerfWidth { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public Dimension Dimension { get; set; }
        public List<Box> CuttedPanels { get; set; }
    }
}
