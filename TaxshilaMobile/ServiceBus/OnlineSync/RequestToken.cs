using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OnlineSync
{
    public class RequestToken
    {
        public string grant_type { get; set; } = "password";
        public string username { get; set; } = "api_admin";
        public string password { get; set; } = "Boxer@123";
    }
}
