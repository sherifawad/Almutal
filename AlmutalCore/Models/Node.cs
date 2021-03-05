using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Node
    {
        public Node RightNode { get; set; }
        public Node BottomNode { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public bool IsOccupied{ get; set; }
        public bool rotated{ get; set; }
        public int Id{ get; set; }
    }          
}
