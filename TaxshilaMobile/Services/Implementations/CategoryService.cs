using ImTools;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using Newtonsoft.Json;
using Sharpnado.Presentation.Forms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Implementations
{

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<CategoryModel> _categoryRepo;
        private readonly IRepository<ProductModel> _productRepo;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly ISyncService _syncService;
        private readonly IRepository<StockInOutManagementModel> _stockInOutRepo;
        private readonly IRepository<StockInOutEntryModel> _stockInOutEntryRepo;

        public CategoryService(IRepository<CategoryModel> categoryRepo, IRepository<ProductModel> productRepo, IAppSettings settings, SyncService syncService, IRepository<StockInOutManagementModel> stockInOutRepo, IRepository<StockInOutEntryModel> stockInOutEntryRepo)
        {
            _stockInOutRepo = stockInOutRepo;
            _stockInOutEntryRepo = stockInOutEntryRepo;
            _categoryRepo = categoryRepo;

            _productRepo = productRepo;
            _settings = settings;
            _restApiHelper = new RestApiHelper();
            _syncService = syncService;

        }

        public async Task<PageResult<TViewModel>> GetCategories<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new()
        {
            var categoryModels = _categoryRepo.GetItemsWithChildren().Where(a => a.IsDelete == false).ToList();

            // TO DO
            //Comment this code for live call 

            if (!categoryModels.AnyExtended() && App.IsCallOnline)
            {
                await _syncService.GetCategoriesSync(_syncService.GetStatus(SyncCategoryTypes.Categories));

                categoryModels = _categoryRepo.GetItemsWithChildren();
            }
            var categoryTypeViewModels = categoryModels?.OrderByDescending(a => a.Name)?.Select(v => new CategoryMapper().MapToViewModel<TViewModel>(v)).ToList();

            return new PageResult<TViewModel>(categoryTypeViewModels.Count, categoryTypeViewModels.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
        }



        public async Task<List<CategoryModel>> GetCategoryByIds(MobileRequest mobileRequest)
        {
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<CategoryModelDTO>>>(Endpoint.CategoysEndpoint.GetCategoryByIds, json);
            var categories = response.ResponseContent?.Select(v => new CategoryMapper().Map(v, (int)Operation.Synced)).ToList();
            _categoryRepo.InsertAllWithChildren(categories);
            return categories;
        }

        public async Task<List<CategoryModelDTO>> GetCategoriesFromServer(MobileRequest mobileRequest)
        {

            mobileRequest.Username = _settings.CurrentUser.UserId;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<CategoryModelDTO>>>(Endpoint.CategoysEndpoint.GetCategory, json);
            return response.ResponseContent;
        }

        public async Task<List<int>> GetCategoryIds()
        {
            MobileRequest mobileRequest = new MobileRequest();
            mobileRequest.Username = _settings.CurrentUser.UserId;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<int>>>(Endpoint.CategoysEndpoint.GetCategoryIds, json);
            return response.ResponseContent;
        }

        public async Task<List<TViewModel>> GetLocalCategories<TViewModel>() where TViewModel : class, new()
        {
            var AllLocalCategories = _categoryRepo.GetItemsWithChildren().Where(a => a.IsDelete == false)?.OrderBy(a => a.Name);
            var categoriesTypeViewModels = AllLocalCategories.Select(v => new CategoryMapper().MapToViewModel<TViewModel>(v)).ToList();
            return categoriesTypeViewModels;
        }

        public async Task<List<CategoryModel>> GetLocalCategoriesModel()
        {

            var AllLocalCategories = _categoryRepo.GetItemsWithChildren()?.Where(a => a.IsDelete == false)?.OrderBy(a => a.CreatedAt)?.ToList();
            return AllLocalCategories;

        }

        public TViewModel GetLocalCategoryById<TViewModel>(int id) where TViewModel : class, new()
        {
            var CategoriesLocalById = _categoryRepo.GetItemWithChildrenById(id) ?? new CategoryModel();
            return new CategoryMapper().MapToViewModel<TViewModel>(CategoriesLocalById);
        }

        public async Task<bool> RemoveCategory<TViewModel>(TViewModel categoryViewModel)
        {
            var model = new CategoryMapper().MapViewModelToModel<UnitModel>(categoryViewModel);
            if (model.ServerId > 0)
            {
                // _unitRepo.Delete(model);
                return false;
            }
            else
            {
                //_unitRepo.Delete(model);
                return true;
            }
        }

        public void UpdateCategories<TViewModel>(TViewModel categoryViewModel)
        {
            var model = new CategoryMapper().MapViewModelToModel<CategoryModel>(categoryViewModel);
            _categoryRepo.InsertOrReplaceWithChildren(model);
        }

        public async Task<TViewModel> UpdateCategoryReturnViewModel<TViewModel>(TViewModel categoryViewModel) where TViewModel : class, new()
        {
            var model = new CategoryMapper().MapViewModelToModel<CategoryModel>(categoryViewModel);
            _categoryRepo.InsertOrReplaceWithChildren(model);
            return new CategoryMapper().MapToViewModel<TViewModel>(model);
        }

        public async Task<CategoryModel> GetLocalCategoryModelByLocalId(int id)
        {
            return _categoryRepo.GetItemById(id);
        }

        public async Task<bool> CheckIsCategoryUseInProduct(int id)
        {
            var Product = _productRepo.GetItemsByQuery<ProductModel>(a => a.CategorysId == id && a.IsDelete == false);
            return Product.AnyExtended();
        }

        public async Task DeleteCategories(int id)
        {
            var CategoryForDelete = _categoryRepo.GetItemById(id);
            _categoryRepo.Delete(CategoryForDelete);
        }
    }
}
