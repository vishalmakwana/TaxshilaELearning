using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using TaxshilaMobile.Services.Interfaces;
using Newtonsoft.Json;
using Prism.Events;
using Sharpnado.Presentation.Forms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Implementations
{
    public class UnitService : IUnitService
    {
        private readonly IRepository<UnitModel> _unitRepo;
        private readonly IRepository<CategoryModel> _cateGoryRepo;
        private readonly IRepository<ProductModel> _productRepo;
        private readonly IRepository<StockInOutManagementModel> _stockInOutRepo;
        private readonly IRepository<StockInOutEntryModel> _stockInOutEntryRepo;
        private readonly IRepository<MeasurementTypeModel> _measurementTypeMode;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly IRepository<SyncStatusModel> _syncMgmtRepo;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISyncService _syncService;

        public UnitService(IRepository<UnitModel> unitRepo, IRepository<CategoryModel> cateGoryRepo, IRepository<ProductModel> productRepo, IAppSettings appSettings, IRepository<SyncStatusModel> syncMgmtRepo, IEventAggregator eventAggregator, ISyncService syncService, IRepository<StockInOutManagementModel> stockInOutRepo, IRepository<StockInOutEntryModel> stockInOutEntryRepo, IRepository<MeasurementTypeModel> measurementTypeMode)
        {
            _unitRepo = unitRepo;
            _cateGoryRepo = cateGoryRepo;
            _productRepo = productRepo;
            _settings = appSettings;
            _syncMgmtRepo = syncMgmtRepo;
            _stockInOutRepo = stockInOutRepo;
            _stockInOutEntryRepo = stockInOutEntryRepo;
            _restApiHelper = new RestApiHelper();
            _eventAggregator = eventAggregator;
            _syncService = syncService;
            _measurementTypeMode = measurementTypeMode;
            AddDefaultMesaurements();
            SaveDefaultUnits();
        }

        public void SaveDefaultUnits()
        {
           
            var UnitsData = _unitRepo.GetItems();
            if (!UnitsData.AnyExtended())
            {
                var KgMesarment = _measurementTypeMode.GetItemWithChildrenById(1);
                var LtMesarment = _measurementTypeMode.GetItemWithChildrenById(2);
                var CurrentDateTime = DateTime.UtcNow;
                var unitModels = new List<UnitModel>()
                {
                    new UnitModel
                    {
                        LocalId=1,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="1 KG"
                    },
                    new UnitModel
                    {
                        LocalId=2,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="2 KG"
                    },
                     new UnitModel
                    {
                        LocalId=3,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="3 KG"
                    }, new UnitModel
                    {
                        LocalId=4,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="5 KG"
                    },new UnitModel
                    {
                        LocalId=5,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="10 KG"
                    },
                    new UnitModel
                    {
                        LocalId=6,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="20 KG"
                    },
                    new UnitModel
                    {
                        LocalId=7,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                         MeasurementTypeId=1,
                        MeasurementType=KgMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="40 KG"
                    }
                    ,new UnitModel
                    {
                        LocalId=8,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                        MeasurementTypeId=2,
                        MeasurementType=LtMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="200 ML"
                    }
                    ,new UnitModel
                    {
                        LocalId=9,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                        MeasurementTypeId=2,
                        MeasurementType=LtMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="500 ML"
                    }
                    ,new UnitModel
                    {
                        LocalId=10,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                        MeasurementTypeId=2,
                        MeasurementType=LtMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="1 LT"
                    }
                     ,new UnitModel
                    {
                        LocalId=11,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                        MeasurementTypeId=2,
                        MeasurementType=LtMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="4 LT"
                    }
                      ,new UnitModel
                    {
                        LocalId=12,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                        MeasurementTypeId=2,
                        MeasurementType=LtMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="10 LT"
                    }
                      ,new UnitModel
                    {
                        LocalId=13,
                        CreatedBy=_settings.CurrentUser.UserId,
                        CreatedAt=CurrentDateTime,
                        ModifiedBy=_settings.CurrentUser.UserId,
                        ModifiedAt=CurrentDateTime,
                        IsActive=true,
                        IsDelete=false,
                        IsSaved=true,
                        MeasurementTypeId=2,
                        MeasurementType=LtMesarment,
                        Operation=(int)Operation.Inserted,
                        UnitTitle="20 LT"
                    }
                };
                 _unitRepo.InsertOrReplaceAllWithChildren(unitModels);
            }



        }

        public void AddDefaultMesaurements()
        {

            var measurementTypes = _measurementTypeMode.GetItems();
            if (!measurementTypes.AnyExtended())
            {
                measurementTypes = new List<MeasurementTypeModel>();
                measurementTypes.Add(new MeasurementTypeModel()
                {
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    MeasurementTypeName = "KG",
                    Descriptions = "Kilogram",
                    CreatedBy = _settings.CurrentUser.UserId,
                    ModifiedBy = _settings.CurrentUser.UserId,
                    Operation = (int)Operation.Inserted,
                    LocalId = 1,
                    IsActive = true,
                    IsDelete = false
                }); ;
                measurementTypes.Add(new MeasurementTypeModel()
                {
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    MeasurementTypeName = "LT",
                    Descriptions = "Liter",
                    CreatedBy = _settings.CurrentUser.UserId,
                    ModifiedBy = _settings.CurrentUser.UserId,
                    Operation = (int)Operation.Inserted,
                    LocalId = 2,
                    IsActive = true,
                    IsDelete = false
                });
                _measurementTypeMode.InsertOrReplaceAllWithChildren(measurementTypes);
            }


        }
        public async Task<List<UnitModelDTO>> GetUnitsFromServer(MobileRequest mobileRequest)
        {
            // data = new MobileRequest();
            mobileRequest.Username = _settings.CurrentUser.UserId;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<UnitModelDTO>>>(Endpoint.UnitsEndpoint.GetUnits, json);
            return response.ResponseContent;
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

        public async Task<List<TViewModel>> GetLocalUnits<TViewModel>() where TViewModel : class, new()
        {
            var AllLocalUnits = _unitRepo.GetItemsWithChildren().Where(a => a.IsDelete == false)?.OrderBy(a => a.UnitTitle).OrderByDescending(a => a.CreatedAt);
            var unitsTypeViewModels = AllLocalUnits.Select(v => new UnitsMapper().MapToViewModel<TViewModel>(v)).ToList();
            return unitsTypeViewModels;
        }

        public TViewModel GetLocalUnitsById<TViewModel>(int id) where TViewModel : class, new()
        {
            var UnitsLocalById = _unitRepo.GetItemWithChildrenById(id);
            return new UnitsMapper().MapToViewModel<TViewModel>(UnitsLocalById);
        }

        public List<TViewModel> GetLocalUnitsViewModelByMeasurementTypId<TViewModel>(int id) where TViewModel : class, new()
        {
            var UnitsLocalById = _unitRepo.GetItemsWithChildren(a => a.MeasurementTypeId == id);

            return UnitsLocalById.Select(a => new UnitsMapper().MapToViewModel<TViewModel>(a)).ToList();
        }

        public async Task<PageResult<TViewModel>> GetUnits<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new()
        {
            var unitModels = _unitRepo.GetItems().Where(a => a.IsDelete == false).OrderBy(a=>a.UnitTitle).ToList();

            // TO DO
            //Comment this code for live call

            if (!unitModels.AnyExtended() && App.IsCallOnline)
            {
                await _syncService.GetUnitsSync(_syncService.GetStatus(SyncCategoryTypes.Units));
                unitModels = _unitRepo.GetItemsWithChildren();
            }
            var unitsTypeViewModels = unitModels?.OrderByDescending(a => a.LocalId)?.Select(v => new UnitsMapper().MapToViewModel<TViewModel>(v)).ToList();

            return new PageResult<TViewModel>(unitsTypeViewModels.Count, unitsTypeViewModels.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
        }

        public async Task<List<UnitModel>> GetUnitsByIds(MobileRequest mobileRequest)
        {
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<UnitModelDTO>>>(Endpoint.UnitsEndpoint.GetUnitByIds, json);
            var unitslist = response.ResponseContent?.Select(v => new UnitsMapper().Map(v, (int)Operation.Synced)).ToList();
            _unitRepo.InsertAllWithChildren(unitslist);
            return unitslist;

            //var unitsTypeViewModels = unitslist.Select(v => new UnitsMapper().MapToViewModel<TViewModel>(v)).ToList();
            //return unitslist;
        }

        public async Task<List<int>> GetUnitIds()
        {
            MobileRequest mobileRequest = new MobileRequest();
            mobileRequest.Username = _settings.CurrentUser.UserId;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<int>>>(Endpoint.UnitsEndpoint.GetUnitIds, json);
            return response.ResponseContent;
        }

        public void UpdateUnits<TViewModel>(TViewModel caseViewModel)
        {
            var model = new UnitsMapper().MapViewModelToModel<UnitModel>(caseViewModel);
            _unitRepo.InsertOrReplaceWithChildren(model);
        }

        public async Task<bool> RemoveUnits<TViewModel>(TViewModel caseViewModel)
        {
            var model = new UnitsMapper().MapViewModelToModel<UnitModel>(caseViewModel);
            if (model.ServerId > 0)
            {
                // _unitRepo.Delete(model);
                return false;
            }
            else
            {
                _unitRepo.Delete(model);
                return true;
            }
        }

        public async Task<TViewModel> UpdateUnitsReturnViewModel<TViewModel>(TViewModel caseViewModel) where TViewModel : class, new()
        {
            var model = new UnitsMapper().MapViewModelToModel<UnitModel>(caseViewModel);
            _unitRepo.InsertOrReplaceWithChildren(model);
            return new UnitsMapper().MapToViewModel<TViewModel>(model);
        }

        public async Task<List<UnitModel>> GetLocalUnitsModel()
        {
            var AllLocalUnits = _unitRepo.GetItemsWithChildren().Where(a => a.IsDelete == false)?.OrderBy(a => a.UnitTitle).ToList();
            return AllLocalUnits;
        }

        public async Task<UnitModel> GetLocalUnitModelByLocalId(int id)
        {
            return _unitRepo.GetItemById(id);
        }

        public List<UnitModel> GetLocalUnitsByMeasurementTypeId(int id)
        {
            var result = _unitRepo.GetItemsWithChildren(a => a.MeasurementTypeId == id).ToList();
            return result;
        }

        //public bool CheckIsUnitUseInProduct(int id)
        //{
        //    var Product = _productRepo.GetItemsByQuery<ProductModel>(a => a.UnitsId == id);
        //    return Product.AnyExtended();
        //}
    }
}