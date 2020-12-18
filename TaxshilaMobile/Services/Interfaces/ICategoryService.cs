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
    public interface ICategoryService
    {
        Task<List<CategoryModelDTO>> GetCategoriesFromServer(MobileRequest mobileRequest);

        Task<PageResult<TViewModel>> GetCategories<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new();

        Task<List<TViewModel>> GetLocalCategories<TViewModel>() where TViewModel : class, new();
        TViewModel GetLocalCategoryById<TViewModel>(int id) where TViewModel : class, new();
        Task<List<CategoryModel>> GetCategoryByIds(MobileRequest mobileRequest);
        Task<List<int>> GetCategoryIds();
        void UpdateCategories<TViewModel>(TViewModel categoryViewModel);
        Task<TViewModel> UpdateCategoryReturnViewModel<TViewModel>(TViewModel categoryViewModel) where TViewModel : class, new();

        Task<bool> RemoveCategory<TViewModel>(TViewModel categoryViewModel);

        Task<List<CategoryModel>> GetLocalCategoriesModel();
        Task<CategoryModel> GetLocalCategoryModelByLocalId(int id);

        Task<bool> CheckIsCategoryUseInProduct(int id);

        Task DeleteCategories(int id);
       
    }
}
