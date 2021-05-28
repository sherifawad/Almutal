using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Strip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Length { get; set;}
        public string Color { get; set;}
        public Strip(double length, string title = null, int id = 0 )
        {
            Id = id;
            Length = length;
            Title = title;
        }
    }
}