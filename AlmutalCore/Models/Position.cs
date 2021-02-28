using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
