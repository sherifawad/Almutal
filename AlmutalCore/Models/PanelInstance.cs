using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class PanelInstance
    {

        public string Title { get; set; }
        public StockSheet Sheet { get; set; }
        public Dimension Dimension { get; set; }
        public bool CanRotate { get; set; }
        public int Id { get; set; }

        public PanelInstance(string title, StockSheet sheet, Dimension dimension, bool canRotate, int id)
        {
            if (dimension.Width > sheet.Dimension.Width || dimension.Length > sheet.Dimension.Length)
                throw new Exception("panel too large for sheet");

            Title = title;
            Sheet = sheet;
            Dimension = dimension;
            CanRotate = canRotate;
            Id = id;
        }

    }
}
