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
    public interface IUnitService
    {


        Task<List<UnitModelDTO>> GetUnitsFromServer(MobileRequest mobileRequest);

        Task<PageResult<TViewModel>> GetUnits<TViewModel>(int pageNumber, int pageSize) where TViewModel : class, new();

        Task<List<TViewModel>> GetLocalUnits<TViewModel>() where TViewModel : class, new();
        TViewModel GetLocalUnitsById<TViewModel>(int id) where TViewModel : class, new();

        List<TViewModel> GetLocalUnitsViewModelByMeasurementTypId<TViewModel>(int id) where TViewModel : class, new();

        Task<List<UnitModel>> GetUnitsByIds(MobileRequest mobileRequest);
        Task<List<int>> GetUnitIds();
        void UpdateUnits<TViewModel>(TViewModel caseViewModel);
        Task<TViewModel> UpdateUnitsReturnViewModel<TViewModel>(TViewModel caseViewModel) where TViewModel : class, new();
      
        Task<bool> RemoveUnits<TViewModel>(TViewModel caseViewModel);

        Task<List<UnitModel>> GetLocalUnitsModel();
        List<UnitModel> GetLocalUnitsByMeasurementTypeId(int id);

        Task<UnitModel> GetLocalUnitModelByLocalId(int id);
        //bool CheckIsUnitUseInProduct(int id);




    }
}
