using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OnlineSync
{
    public class ResponseToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }
}
