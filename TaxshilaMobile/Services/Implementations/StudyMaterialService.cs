using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;

namespace TaxshilaMobile.Services.Implementations
{
    public class StudyMaterialService: IStudyMaterialService
    {
        private readonly IAppSettings appSettings;
        private readonly RestApiHelper _restApiHelper;

        public StudyMaterialService(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            _restApiHelper = new RestApiHelper();
        }

        public async Task<List<StudyMaterialsDTO>> GetStudentStudyMaterials()
        {
            var request = new MobileRequest();
            request.Username = appSettings.CurrentUser.Username;
            var json = JsonConvert.SerializeObject(request);
            var response = await _restApiHelper.PostAsync<Response<List<StudyMaterialsDTO>>>(Endpoint.HomeworkAndStudyMaterialEndPoint.GetStudentStudyMaterials, json);
            return response.ResponseContent;
        }
    }

}
