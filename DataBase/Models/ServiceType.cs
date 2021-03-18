using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class ServiceType : BaseModel
    {
        public string Name { get; set; }


        public virtual List<Material> Materials { get; set; }
        public virtual List<Process> Processes { get; set; }
        public virtual List<ClientService> ClientServices { get; set; }
    }
}
