using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Process : BaseModel
    {
        public Guid? SupplierId { get; set; }
        public Guid? ServiceTypeID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public virtual Supplier supplier { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}
