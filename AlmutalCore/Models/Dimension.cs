using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Dimension
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public Dimension(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public double getArea()
        {
            return Length * Width;
        }

        public bool canHold(Dimension other)
        {
            return other.Length <= Length && other.Width <= Width;
        }

        public Dimension rotated()
        {
            return new Dimension(Width, Length);
        }
    }
}
