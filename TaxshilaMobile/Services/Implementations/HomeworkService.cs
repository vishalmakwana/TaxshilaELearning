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
    public class HomeworkService : IHomeworkService
    {
        private readonly IAppSettings appSettings;
        private readonly RestApiHelper _restApiHelper;

        public HomeworkService(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            _restApiHelper = new RestApiHelper();

        }

        public async Task<List<HomeworkDTO>> GetStudentHomeWork()
        {
            var request = new MobileRequest();
            request.Username = appSettings.CurrentUser.Username;
            var json = JsonConvert.SerializeObject(request);
            var response = await _restApiHelper.PostAsync<Response<List<HomeworkDTO>>>(Endpoint.HomeworkAndStudyMaterialEndPoint.GetStudentHomeWork, json);
            return response.ResponseContent;
        }
    }
}
