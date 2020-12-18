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
    public interface IStockInOutService
    {
        
        Task<TViewModel> UpdateStocksInOutReturnViewModel<TViewModel>(TViewModel stocksInOutViewModel) where TViewModel : class, new();

        Task<PageResult<TViewModel>> GetAllPaginatorStockdata<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new();

        TViewModel GetLocalStockThinViewModelDataByLocalId<TViewModel>(int id) where TViewModel : class, new();

        Task<List<StockInOutManagementModel>> GetLocalStockManagementModeldata();

        Task<List<TViewModel>> GetLocalAllStockdata<TViewModel>() where TViewModel : class, new();
        UnitModel SetProductUnitStock(int UnitId, int Productid);


    }
}
