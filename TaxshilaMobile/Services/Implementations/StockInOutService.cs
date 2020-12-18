using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Queue;
using TaxshilaMobile.Services.Interfaces;
using Sharpnado.Presentation.Forms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Implementations
{
    public class StockInOutService : IStockInOutService
    {
        private readonly IRepository<CategoryModel> _categoryRepo;
        private readonly IRepository<ProductModel> _productRepo;
        private readonly IRepository<UnitModel> _unitRepo;
        private readonly IRepository<StockInOutManagementModel> _stockInOutRepo;
        //private readonly IQueuedRepository<StockInOutManagementModel> _stockInOutQueuedRepo;
        private readonly IRepository<StockInOutEntryModel> _stockInOutEntryRepo;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly ISyncService _syncService;

        public StockInOutService(IRepository<CategoryModel> categoryRepo, IRepository<ProductModel> productRepo, IAppSettings settings, SyncService syncService, IRepository<UnitModel> unitRepo, IRepository<StockInOutManagementModel> stockInOutRepo, IRepository<StockInOutEntryModel> stockInOutEntryRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _unitRepo = unitRepo;
            _stockInOutEntryRepo = stockInOutEntryRepo;
            _stockInOutRepo = stockInOutRepo;
            _settings = settings;
            _restApiHelper = new RestApiHelper();
            _syncService = syncService;
        }

        public async Task<PageResult<TViewModel>> GetAllPaginatorStockdata<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new()
        {
            var stockItemsModels = _stockInOutRepo.GetItemsWithChildren().Where(a => a.StockInOutProductModel != null).ToList();

            // TO DO
            //Comment this code for live call 

            if (!stockItemsModels.AnyExtended() && App.IsCallOnline)
            {
                await _syncService.GetCategoriesSync(_syncService.GetStatus(SyncCategoryTypes.Categories));

                //stockItemsModels = _categoryRepo.GetItemsWithChildren();
            }
            var stockItemsModeleViewModels = stockItemsModels?.OrderByDescending(a => a.CreatedAt)?.Select(v => new StocInOutMapper().MapToViewModel<TViewModel>(v)).ToList();


            return new PageResult<TViewModel>(stockItemsModeleViewModels.Count, stockItemsModeleViewModels.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
        }

        public async Task<List<TViewModel>> GetLocalAllStockdata<TViewModel>() where TViewModel : class, new()
        {
            var GetLocalAllStockdata = _stockInOutRepo.GetItemsWithChildren().Where(a => a.StockInOutProductModel != null).OrderBy(a => a.CreatedAt);
            var categoriesTypeViewModels = GetLocalAllStockdata.Select(v => new StocInOutMapper().MapToViewModel<TViewModel>(v)).ToList();
            return categoriesTypeViewModels;
        }

        public async Task<List<StockInOutManagementModel>> GetLocalStockManagementModeldata()
        {
            var GetLocalStockManagementModeldata = _stockInOutRepo.GetItemsWithChildren().Where(a => a.StockInOutProductModel.IsDelete == false).OrderBy(a => a.CreatedAt)?.ToList();
            return GetLocalStockManagementModeldata;
        }

        public TViewModel GetLocalStockThinViewModelDataByLocalId<TViewModel>(int id) where TViewModel : class, new()
        {
            var StockThinViewModelDataByLocalId = _stockInOutRepo.GetItemWithChildrenById(id);
            return new CategoryMapper().MapToViewModel<TViewModel>(StockThinViewModelDataByLocalId);
        }

        public async Task<TViewModel> UpdateStocksInOutReturnViewModel<TViewModel>(TViewModel stocksInOutViewModel) where TViewModel : class, new()
        {
            var model = new StocInOutMapper().MapViewModelToModel<StockInOutManagementModel>(stocksInOutViewModel);

            _stockInOutRepo.InsertOrReplaceWithChildren(model);
            return new CategoryMapper().MapToViewModel<TViewModel>(model);
        }


        public UnitModel SetProductUnitStock(int UnitId, int Productid)
        {
            var GetUints = _unitRepo.GetItemWithChildrenById(UnitId);
            var GetUnitWiseProductEntry = _stockInOutEntryRepo.GetItemsByQuery<StockInOutEntryModel>(a => a.StockInOutProductId == Productid && a.StockInOutUnitId == UnitId);
            int StockIn = 0;
            int StockOut = 0;
            int FinalStock = 0;
            StockIn = GetUnitWiseProductEntry?.Where(a => a.StockInOutOption == (int)StockInOutEnums.StockIn).ToList().Sum(a => a.StockCount) ?? 0;
            StockOut = GetUnitWiseProductEntry?.Where(a => a.StockInOutOption == (int)StockInOutEnums.StockOut
            ).ToList().Sum(a => a.StockCount) ?? 0;

            FinalStock = StockIn - StockOut;
            // FinalStock = FinalStock > 0 ? FinalStock : FinalStock * -1;
            GetUints.TotalStockIn = StockIn;
            GetUints.TotalStockOut = StockOut;
            GetUints.TotalStok = FinalStock;
            return GetUints;
        }
    }
}
