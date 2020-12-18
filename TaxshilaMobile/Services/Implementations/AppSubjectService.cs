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
    public class AppSubjectService : IAppSubjectService
    {
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;
        public AppSubjectService(IAppSettings appSettings)
        {
            _settings = appSettings;
            _restApiHelper = new RestApiHelper();
        }
        public async Task<List<SubjectDTO>> GetSubjectVideos()
        {
            var mobileRequest = new MobileRequest();
            mobileRequest.Username = _settings.CurrentUser.Username;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<SubjectDTO>>>(Endpoint.SubjectEndPoint.GetStudentSubject, json);
            return response.ResponseContent;
        }
    }
}
