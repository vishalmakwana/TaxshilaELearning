using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface IQueuedSyncService
    {
        Task<int> GetUnitQueuedSync(SyncStatusThinViewModel sync);

    }
}
