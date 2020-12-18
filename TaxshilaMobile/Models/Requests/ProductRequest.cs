using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxshilaMobile.Models.Requests
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? Status { get; set; }
        public string ItemCode { get; set; }
        public int CategorysId { get; set; }
        public int UnitsId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Username { get;  set; }
    }
}
