using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models
{
    public interface IMapper<T, R>
    {
        T Map(R obj);
    }
}
