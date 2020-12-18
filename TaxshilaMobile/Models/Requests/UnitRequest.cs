
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxshilaMobile.Models.Requests
{
    public class UnitRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }      
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Username { get; set; }
    }
}
