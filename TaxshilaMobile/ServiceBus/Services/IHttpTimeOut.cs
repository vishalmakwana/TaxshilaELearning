using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TaxshilaMobile.ServiceBus.Services
{
    public interface IHttpTimeOut
    {
        HttpClient GetHttpClient();
    }
}
