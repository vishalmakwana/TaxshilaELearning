using ImTools;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.PrismEvents;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using TaxshilaMobile.ServiceBus.OfflineSync.Queue;
using TaxshilaMobile.Services.Interfaces;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Implementations
{
    public class QueuedSyncService : IQueuedSyncService
    {
        private readonly IQueuedRepository<UnitModel> _unitRepo;
        private readonly IQueuedRepository<CategoryModel> _cateGoryRepo;
        private readonly IQueuedRepository<ProductModel> _productRepo;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly IQueuedRepository<SyncStatusModel> _syncMgmtRepo;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISyncService _syncService;
        public QueuedSyncService(IQueuedRepository<UnitModel> unitRepo, IQueuedRepository<CategoryModel> cateGoryRepo, IQueuedRepository<ProductModel> productRepo, IAppSettings appSettings, IQueuedRepository<SyncStatusModel> syncMgmtRepo, IEventAggregator eventAggregator, ISyncService syncService)
        {
            _unitRepo = unitRepo;
            _cateGoryRepo = cateGoryRepo;
            _productRepo = productRepo;
            _settings = appSettings;
            _syncMgmtRepo = syncMgmtRepo;
            _restApiHelper = new RestApiHelper();
            _eventAggregator = eventAggregator;
            _syncService = syncService;
        }


        public async Task<int> GetUnitQueuedSync(SyncStatusThinViewModel sync)
        {
            try
            {
                Debug.WriteLine("Get Unit Queued Sync started");
                try
                {
                    var data = new MobileRequest();
                    data.Username = _settings.CurrentUser.UserId;
                    data.LastSyncDate = sync?.LastSyncDate;
                    var json = JsonConvert.SerializeObject(data);
                    Debug.WriteLine($"Starting Queued GetUnitQueuedSync");

                    var result = await _restApiHelper.PostAsync<List<UnitModelDTO>>(Endpoint.Sync.GetUnitsSync, json).ConfigureAwait(false);
                    var UnitsList = result.Select(v => v.MapToUnitModel()).ToList();
                    if (UnitsList.Count > 0)
                    {
                        await _unitRepo.InsertOrReplaceAllWithChildren(UnitsList);
                        await UpdateSyncTime(result, sync);
                    }
                 //   _eventAggregator.GetEvent<SyncUpdateNotificationEvent>().Publish(
                 //new SyncUpdatePayload
                 //{
                 //    Message = "Get Unit synchronize completed.",
                 //    Success = true
                 //}
                 //);

                    return UnitsList.Count;

                }
                catch (Exception ex)
                {

                    Debug.WriteLine("ERROR Sync:GetUnitQueuedSync ex: " + ex);
                    return -1;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task UpdateSyncTime<T>(List<T> apiResult, SyncStatusThinViewModel sync)
        {
            //Update sync time only if results were returned
            if (apiResult != null && apiResult.Count > 0)
            {
                sync.LastSyncDate = DateTime.UtcNow;
                await _syncMgmtRepo.UpdateAsync(new UnitsMapper().MapViewModelToModel<SyncStatusModel>(sync));
            }
        }
    }
}
