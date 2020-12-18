using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface ISyncService
    {
        Task<int> GetUnitsSync(SyncStatusThinViewModel sync);
        Task<int> GetCategoriesSync(SyncStatusThinViewModel sync);
        Task<int> GetProductSync(SyncStatusThinViewModel sync);
        SyncStatusThinViewModel GetStatus(SyncCategoryTypes statusType);
    }
}
