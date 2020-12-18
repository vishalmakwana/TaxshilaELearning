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
    public class NoticeService: INoticeService
    {
        private readonly IAppSettings appSettings;
        private readonly RestApiHelper _restApiHelper;

        public NoticeService(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            _restApiHelper = new RestApiHelper();

        }

        public async Task<List<UserNoticeDTO>> GetStudentNotice()
        {
            var mobileRequest = new MobileRequest();
            mobileRequest.Username = appSettings.CurrentUser.Username;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<UserNoticeDTO>>>(Endpoint.NoticeEndPoint.GetStudentNotice, json);
            return response.ResponseContent;
        }

        public async Task<List<PublicEventDTO>> GetStudentPublicEvents()
        {
            var mobileRequest = new MobileRequest();
            mobileRequest.Username = appSettings.CurrentUser.Username;
            var json = JsonConvert.SerializeObject(mobileRequest);
            var response = await _restApiHelper.PostAsync<Response<List<PublicEventDTO>>>(Endpoint.NoticeEndPoint.GetStudentPublicEvents, json);
            return response.ResponseContent;
        }
    }
}
