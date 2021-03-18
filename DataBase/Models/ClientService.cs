using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public class ClientService
    {
        public Guid ClientId { get; set; }
        public Guid ServiceId { get; set; }
        public ServiceType Service { get; set; }
        public Client Client { get; set; }
    }
}
