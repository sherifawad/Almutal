using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Box
    {
        public Box(float length, float width)
        {
            Length = length;
            Width = width;
        }

        public Box()
        {

        }

        public float Length { get; set; }
        public float Width { get; set; }
        public float Area { get; set; }
        public Node Position { get; set; }
        public bool Used { get; set; }
        public int ParentId { get; set; }
        public int Id { get; set; }
    }
}
