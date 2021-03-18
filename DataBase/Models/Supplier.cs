using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Supplier : BaseModel
    {
        public string Name { get; set; }
        public Guid? DetailsId { get; set; }

        public virtual List<Material> Materials { get; set; }
        public virtual List<Process> Processes { get; set; }
        public virtual Details Details { get; set; }
    }
}
