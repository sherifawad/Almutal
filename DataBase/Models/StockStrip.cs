using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class StockStrip
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public double Length { get; set; }
        public List<Strip> CuttedStrips { get; set; } = new List<Strip>();
    }
}
