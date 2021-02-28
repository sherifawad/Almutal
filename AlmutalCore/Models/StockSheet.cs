using AlmutalCore.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class StockSheet
    {
        public Almut Matrial { get; private set; }
        public double Thickness { get; private set; }
        public string Title { get; private set; }
        public Dimension Dimension { get; private set; }
        public StockSheet(Almut matrial, double thickness, string title, Dimension dimension)
        {
            Matrial = matrial;
            Thickness = thickness;
            Title = title;
            Dimension = dimension;
        }

    }
}
