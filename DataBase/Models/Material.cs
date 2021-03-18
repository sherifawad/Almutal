using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Material : BaseModel
    {
        public Guid? SupplierId { get; set; }
        public Guid? ServiceTypeID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public DateTime BuyingDate { get; set; }
        public List<StockSheet> StockSheets { get; set; }
        public virtual Supplier supplier { get; set; }
        public virtual ServiceType ServiceType { get; set; }

    }
}
