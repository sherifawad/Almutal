using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Node
    {
        public Node RightNode { get; set; }
        public Node BottomNode { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public bool IsOccupied{ get; set; }
        public bool rotated{ get; set; }
        public int Id{ get; set; }
    }          
}
