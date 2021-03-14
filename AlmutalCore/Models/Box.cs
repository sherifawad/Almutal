using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Box
    {
        private string title;

        public Box(double length, double width, string tile = null )
        {
            Length = length;
            Width = width;
            Title = tile;
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
        public int Id { get; set; }
        public string Color { get; set; }
        public string Title 
        {
            get 
            { 
                if(string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
                    return $"{Width}*{Length}";
                else
                    return title; 
            }
            set => title = value; 
        }

    }
}
