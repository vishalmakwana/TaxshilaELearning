using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using Sharpnado.Presentation.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductModelDTO>> GetProductsFromServer(MobileRequest mobileRequest);

        Task<PageResult<TViewModel>> GetProducts<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new();

        Task<List<TViewModel>> GetLocalProducts<TViewModel>() where TViewModel : class, new();
        TViewModel GetLocalProductById<TViewModel>(int id) where TViewModel : class, new();
        Task<List<ProductModel>> GetProductsByIds(MobileRequest mobileRequest);
        Task<List<int>> GetProductIds();
        void UpdateProducts<TViewModel>(TViewModel productViewModel);
        Task<TViewModel> UpdateProductReturnViewModel<TViewModel>(TViewModel productViewModel) where TViewModel : class, new();

        Task<bool> RemoveProduct<TViewModel>(TViewModel productViewModel);

        Task<ProductModel> GetLocalProductModelByLocalId(int id);

        List<ProductModel> GetLocalProductModelsByCategoryId(int id);
        List<ProductModel> GetLocalProductModels();

        List<ProductModel> GetLocalProductByCategoryId(int categoryid);

    }
}
