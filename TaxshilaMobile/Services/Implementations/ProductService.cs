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
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Implementations
{

    public class ProductService : IProductService
    {
        private readonly IRepository<UnitModel> _unitRepo;
        private readonly IRepository<CategoryModel> _cateGoryRepo;
        private readonly IRepository<ProductModel> _productRepo;
        private readonly IRepository<StockInOutManagementModel> _stockInOutRepo;
        private readonly IRepository<StockInOutEntryModel> _stockInOutEntryRepo;
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        private readonly ISyncService _syncService;
        public ProductService(IRepository<UnitModel> unitRepo, IRepository<CategoryModel> cateGoryRepo, IRepository<ProductModel> productRepo, IAppSettings settings, ISyncService syncService, IRepository<StockInOutManagementModel> stockInOutRepo, IRepository<StockInOutEntryModel> stockInOutEntryRepo)
        {
            _unitRepo = unitRepo;
            _cateGoryRepo = cateGoryRepo;
            _stockInOutRepo = stockInOutRepo;
            _stockInOutEntryRepo = stockInOutEntryRepo;
            _productRepo = productRepo;
            _settings = settings;
            _productRepo = productRepo;
            _restApiHelper = new RestApiHelper();
            _syncService = syncService;
        }

        public List<ProductModel> GetLocalProductByCategoryId(int categoryid)
        {
            var ProductbyCategory = _productRepo.GetItemsWithChildren(a => a.CategorysId == categoryid && a.IsDelete==false)?.ToList() ?? new List<ProductModel>();
            return ProductbyCategory;
        }

        public TViewModel GetLocalProductById<TViewModel>(int id) where TViewModel : class, new()
        {
            var ProductsLocalById = _productRepo.GetItemWithChildrenById(id);
            return new ProductsMapper().MapToViewModel<TViewModel>(ProductsLocalById);
        }

        public async Task<ProductModel> GetLocalProductModelByLocalId(int id)
        {
            return _productRepo.GetItemWithChildrenById(id);
        }

        public List<ProductModel> GetLocalProductModels()
        {
            return _productRepo.GetItemsWithChildren()?.Where(a => a.IsDelete == false).ToList(); ;
        }

        public List<ProductModel> GetLocalProductModelsByCategoryId(int id)
        {
            var Products = _productRepo.GetItemsWithChildren(a => a.CategorysId == id).ToList();
            return Products;
        }

        public async Task<List<TViewModel>> GetLocalProducts<TViewModel>() where TViewModel : class, new()
        {
            var AllLocalProducts = _productRepo.GetItemsWithChildren().Where(a => a.IsDelete == false)?.OrderBy(a => a.Name);
            var productThinViewModels = AllLocalProducts.Select(v => new ProductsMapper().MapToViewModel<TViewModel>(v)).ToList();
            return productThinViewModels;
        }

        public async Task<List<int>> GetProductIds()
        {
            MobileRequest mobileRequest = new MobileRequest();
            mobileRequest.Username = _settings.CurrentUser.UserId;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<int>>>(Endpoint.ProductsEndpoint.GetProductIds, json);
            return response.ResponseContent;
        }

        public async Task<PageResult<TViewModel>> GetProducts<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new()
        {
            var productModel = _productRepo.GetItemsWithChildren().Where(a => a.IsDelete == false).ToList();

            // TO DO
            //Comment this code for live call 

            if (!productModel.AnyExtended() && App.IsCallOnline)
            {
                await _syncService.GetProductSync(_syncService.GetStatus(SyncCategoryTypes.Products));
                productModel = _productRepo.GetItemsWithChildren();
            }
            var productsTypeViewModels = productModel?.OrderByDescending(a => a.Name)?.Select(v => new ProductsMapper().MapToViewModel<TViewModel>(v)).ToList();

            return new PageResult<TViewModel>(productsTypeViewModels.Count, productsTypeViewModels.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
        }

        public async Task<List<ProductModel>> GetProductsByIds(MobileRequest mobileRequest)
        {
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<ProductModelDTO>>>(Endpoint.ProductsEndpoint.GetProductsByIds, json);
            var products = response.ResponseContent?.Select(v => new ProductsMapper().Map(v, (int)Operation.Synced)).ToList();
            _productRepo.InsertAllWithChildren(products);
            return products;
        }

        public async Task<List<ProductModelDTO>> GetProductsFromServer(MobileRequest mobileRequest)
        {
            try
            {
                mobileRequest.Username = _settings.CurrentUser.UserId;
                var json = JsonConvert.SerializeObject(mobileRequest);
                var response = await _restApiHelper.PostAsync<Response<List<ProductModelDTO>>>(Endpoint.ProductsEndpoint.GetProducts, json);
                return response.ResponseContent;
            }
            catch (Exception)
            {
                return new List<ProductModelDTO>();
            }
        }

        public Task<bool> RemoveProduct<TViewModel>(TViewModel productViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<TViewModel> UpdateProductReturnViewModel<TViewModel>(TViewModel productViewModel) where TViewModel : class, new()
        {
            var model = new CategoryMapper().MapViewModelToModel<ProductModel>(productViewModel);
            _productRepo.InsertOrReplaceWithChildren(model);
            return new ProductsMapper().MapToViewModel<TViewModel>(model);
        }

        public void UpdateProducts<TViewModel>(TViewModel productViewModel)
        {
            var model = new ProductsMapper().MapViewModelToModel<ProductModel>(productViewModel);
            _productRepo.InsertOrReplaceWithChildren(model);
        }
    }
}
