using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Box
    {
        public Box(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public Box()
        {

        }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Area { get; set; }
        public Node Position { get; set; }
        public bool Used { get; set; }
        public int ParentId { get; set; }
        public int Id { get; set; }
        public string Color { get; set; }
        public string Title => ($"{Width}*{Length}");

    }
}
