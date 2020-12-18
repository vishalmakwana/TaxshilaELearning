using TaxshilaMobile.DataTypesApp.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class SyncStatusThinViewModel : BaseThinViewModel
    {
        private SyncStatusTypes _status;
        [ModelProperty(nameof(SyncStatusModel.Status))]
        public SyncStatusTypes Status
        {
            get => _status;
            set { SetProperty(ref _status, value); }
        }
        private SyncCategoryTypes _syncCatType;
        [ModelProperty(nameof(SyncStatusModel.Category))]
        public SyncCategoryTypes Category
        {
            get => _syncCatType;
            set { SetProperty(ref _syncCatType, value); }
        }
        private DateTime? _lastSyncDate;
        [ModelProperty(nameof(SyncStatusModel.LastSyncedAt))]
        public DateTime? LastSyncDate
        {
            get => _lastSyncDate;
            set { SetProperty(ref _lastSyncDate, value); }
        }
    }
}
