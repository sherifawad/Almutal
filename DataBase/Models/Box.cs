using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Box : BaseModel
    {
        private string title;

        public Box(double length, double width, string title = null)
        {
            Length = length;
            Width = width;
            Title = title;
        }

        public Box()
        {

        }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Area => Width * Length;
        public Node Position { get; set; }
        public bool Used { get; set; }
        public int ParentId { get; set; }
        public string Color { get; set; }
        public string Dimension => $"{Width}*{Length}";
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    title = Dimension;
                else
                    title = value;
            }
        }
    }
}
