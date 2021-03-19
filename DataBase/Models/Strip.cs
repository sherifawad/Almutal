using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Strip
    {
        public int Id { get; private set; }
        public List<double> Cuts { get; private set;}

        public Strip(int id, List<double> cuts)
        {
            Id = id;
            Cuts = cuts;
        }
    }
}