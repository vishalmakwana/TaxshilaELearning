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
    public interface IMeasurementTypeService
    {
        
        List<MeasurementTypeModel> GetLocalMeasurementTypesModel();
        Task<MeasurementTypeModel> GetLocalMeasurementTypesModelById(int id);
      
    }
}
