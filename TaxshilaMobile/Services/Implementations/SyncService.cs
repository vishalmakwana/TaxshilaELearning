using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.PrismEvents;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
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
    public class SyncService : ISyncService
    {
        private readonly IRepository<UnitModel> _unitRepo;
        private readonly IRepository<CategoryModel> _cateGoryRepo;
        private readonly IRepository<ProductModel> _productRepo;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly IRepository<SyncStatusModel> _syncMgmtRepo;
        private readonly IEventAggregator _eventAggregator;
       
        public SyncService(IRepository<UnitModel> unitRepo, IRepository<CategoryModel> cateGoryRepo, IRepository<ProductModel> productRepo, IAppSettings appSettings, IRepository<SyncStatusModel> syncMgmtRepo, IEventAggregator eventAggregator)
        {
            _unitRepo = unitRepo;
            _cateGoryRepo = cateGoryRepo;
            _productRepo = productRepo;
            _settings = appSettings;
            _syncMgmtRepo = syncMgmtRepo;
            _restApiHelper = new RestApiHelper();
            _eventAggregator = eventAggregator;
            
        }

        public SyncStatusThinViewModel GetStatus(SyncCategoryTypes statusType)
        {
            try
            {
                var status = _syncMgmtRepo.GetItemByQuery(x => x.Category == (int)statusType);

                if (status == null)
                {
                    CreateSyncStatuses();
                    status = _syncMgmtRepo.GetItemByQuery(x => x.Category == (int)statusType);
                }

                return new UnitsMapper().MapToViewModel<SyncStatusThinViewModel>(status);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR!!! Get stting status for SyncType: {statusType} with ex: {ex}");
                return null;
            }
        }


        /// <summary>
        /// Creates all missing sync status types
        /// </summary>
        /// <returns></returns>
        public bool CreateSyncStatuses()
        {
            try
            {
                List<SyncStatusModel> statuses = new List<SyncStatusModel>();
                foreach (var item in Enum.GetValues(typeof(SyncCategoryTypes)))
                {
                    if (_syncMgmtRepo.GetItemByQuery(x => x.Category == (int)item) == null)
                    {
                        SyncStatusModel model = new SyncStatusModel
                        {
                            Category = Convert.ToInt32(item),
                            LastSyncedAt = null,
                            Status = (int)SyncStatusTypes.NotStarted
                        };
                        statuses.Add(model);
                    }
                }

                _syncMgmtRepo.InsertOrReplaceAllWithChildren(statuses);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR!!! Creating statuses. ex: " + ex);
                return false;
            }

            return true;
        }

        public async Task<int> GetUnitsSync(SyncStatusThinViewModel sync)
        {
            try
            {
                var data = new MobileRequest();

                data.Username = _settings.CurrentUser.UserId;
                data.LastSyncDate = null;
                var json = JsonConvert.SerializeObject(data);


                var response = await _restApiHelper.PostAsync<Response<List<UnitModelDTO>>>(Endpoint.UnitsEndpoint.GetUnits, json).ConfigureAwait(false);

                var unitslist = response.ResponseContent?.Select(v => v.MapToUnitModel()).ToList();
                if (unitslist?.Count > 0)
                {
                    foreach (var record in unitslist)
                    {
                        var existingRecord = _unitRepo.GetItemByQuery(f => f.ServerId == record.ServerId);
                        if (existingRecord == null)
                        {
                            _unitRepo.InsertOrReplaceWithChildren(record);
                        }
                    }
                    UpdateSyncTime(response.ResponseContent, sync);
                }
                Debug.WriteLine($"Starting GetUnitsSync");

                _eventAggregator.GetEvent<SyncUpdateNotificationEvent>().Publish(
                 new SyncUpdatePayload
                 {
                     Message = "Units synchronize completed.",
                     Success = true
                 }
                 );

                return unitslist.Count;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("ERROR Sync:GetUnitsSync ex: " + ex);
                return -1;
            }


        }

        public async Task<int> GetCategoriesSync(SyncStatusThinViewModel sync)
        {
            try
            {
                var data = new MobileRequest();

                data.Username = _settings.CurrentUser.UserId;
                data.LastSyncDate = null;
                var json = JsonConvert.SerializeObject(data);


                var response = await _restApiHelper.PostAsync<Response<List<CategoryModelDTO>>>(Endpoint.CategoysEndpoint.GetCategory, json).ConfigureAwait(false);

                var categories = response.ResponseContent?.Select(v => v.MapToCategoryModel()).ToList();
                if (categories?.Count > 0)
                {
                    foreach (var record in categories)
                    {
                        var existingRecord = _unitRepo.GetItemByQuery(f => f.ServerId == record.ServerId);
                        if (existingRecord == null)
                        {
                            _cateGoryRepo.InsertOrReplaceWithChildren(record);
                        }
                    }
                    UpdateSyncTime(response.ResponseContent, sync);
                }
                Debug.WriteLine($"Starting GetCategories Sync");

                //_eventAggregator.GetEvent<SyncUpdateNotificationEvent>().Publish(
                // new SyncUpdatePayload
                // {
                //     Message = "Units synchronize completed.",
                //     Success = true
                // }
                // );

                return categories.Count;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("ERROR Sync:GetCategoriesSync ex: " + ex);
                return -1;
            }


        }

        public async Task<int> GetProductSync(SyncStatusThinViewModel sync)
        {
            try
            {
                var data = new MobileRequest();

                data.Username = _settings.CurrentUser.UserId;
                data.LastSyncDate = null;
                var json = JsonConvert.SerializeObject(data);


                var response = await _restApiHelper.PostAsync<Response<List<ProductModelDTO>>>(Endpoint.ProductsEndpoint.GetProducts, json).ConfigureAwait(false);

                var products = response.ResponseContent?.Select(v => v.MapToProductModel()).ToList();
                if (products?.Count > 0)
                {
                    foreach (var record in products)
                    {
                        var existingRecord = _productRepo.GetItemByQuery(f => f.ServerId == record.ServerId);
                        if (existingRecord == null)
                        {
                            _productRepo.InsertOrReplaceWithChildren(record);
                        }
                    }
                    UpdateSyncTime(response.ResponseContent, sync);
                }
                Debug.WriteLine($"Starting GetProductSync Sync");

                //_eventAggregator.GetEvent<SyncUpdateNotificationEvent>().Publish(
                // new SyncUpdatePayload
                // {
                //     Message = "Units synchronize completed.",
                //     Success = true
                // }
                // );

                return products.Count;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("ERROR Sync:GetProductSync ex: " + ex);
                return -1;
            }


        }
        private void UpdateSyncTime<T>(List<T> apiResult, SyncStatusThinViewModel sync)
        {
            //Update sync time only if results were returned
            if (apiResult != null && apiResult.Count > 0)
            {
                sync.LastSyncDate = DateTime.UtcNow;
                _syncMgmtRepo.Update(new UnitsMapper().MapViewModelToModel<SyncStatusModel>(sync));
            }
        }



    }
}
