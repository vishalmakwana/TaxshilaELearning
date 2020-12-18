using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class NavigationPageRequest
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public int LocalId { get; set; }
        public int ExternalId { get; set; }
        public bool IsNew { get; set; }
        public string UserName { get; set; }

        public NavigationPageRequest()
        {
            IsNew = false;
        }

    }
}
