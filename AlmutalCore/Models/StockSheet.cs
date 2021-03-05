using AlmutalCore.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class StockSheet
    {
        public double Thickness { get; private set; }
        public string Title { get; private set; }
        public Dimension Dimension { get; private set; }
        public StockSheet(Dimension dimension, double thickness = 0, string title = "")
        {
            Thickness = thickness;
            Title = title;
            Dimension = dimension;
        }

    }
}
