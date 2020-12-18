using ImTools;
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
    public class MeasurementTypeService : IMeasurementTypeService
    {
        private readonly IRepository<UnitModel> _unitRepo;
        private readonly IRepository<CategoryModel> _cateGoryRepo;
        private readonly IRepository<ProductModel> _productRepo;
        private readonly IRepository<MeasurementTypeModel> _measurementTyperepo;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly IRepository<SyncStatusModel> _syncMgmtRepo;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISyncService _syncService;

        public MeasurementTypeService(IRepository<UnitModel> unitRepo, IRepository<CategoryModel> cateGoryRepo, IRepository<ProductModel> productRepo, IAppSettings appSettings, IRepository<SyncStatusModel> syncMgmtRepo, IEventAggregator eventAggregator, ISyncService syncService, IRepository<MeasurementTypeModel> easurementTyperepo)
        {
            _measurementTyperepo = easurementTyperepo;
            _unitRepo = unitRepo;
            _cateGoryRepo = cateGoryRepo;
            _productRepo = productRepo;
            _settings = appSettings;
            _syncMgmtRepo = syncMgmtRepo;
            _restApiHelper = new RestApiHelper();
            _eventAggregator = eventAggregator;
            _syncService = syncService;
          
            AddDefaultMesaurements();
        }

        public void AddDefaultMesaurements()
        {

            var measurementTypes = _measurementTyperepo.GetItems();
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
                _measurementTyperepo.InsertOrReplaceAllWithChildren(measurementTypes);
            }
           

        }

        public List<MeasurementTypeModel> GetLocalMeasurementTypesModel()
        {
            return _measurementTyperepo.GetItemsWithChildren();
        }

        public async Task<MeasurementTypeModel> GetLocalMeasurementTypesModelById(int id)
        {
            return _measurementTyperepo.GetItemById(id);

        }
    }
}
