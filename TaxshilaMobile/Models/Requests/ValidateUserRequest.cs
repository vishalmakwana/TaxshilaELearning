using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models.Requests
{
    public class ValidateUserRequest
    {
        public string UserName { get; set; }
        public string DeviceId { get; set; }
        public string Password { get; set; }
    }
}
