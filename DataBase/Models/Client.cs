using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class Client : BaseModel
    {
        public string Name { get; set; }
        public Guid? DetailsId { get; set; }

        public virtual List<ClientService> ClientServices { get; set; }
        public virtual Details Details { get; set; }
    }
}
