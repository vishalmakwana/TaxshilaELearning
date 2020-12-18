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
using TaxshilaMobile.ServiceBus.OnlineSync.Models;
using TaxshilaMobile.Services.Interfaces;

namespace TaxshilaMobile.Services.Implementations
{
    public class VideoLectureService : IVideoLectureService
    {
        private readonly IAppSettings _settings;
        private readonly RestApiHelper _restApiHelper;

        public VideoLectureService(IAppSettings appSettings)
        {
            _settings = appSettings;
            _restApiHelper = new RestApiHelper();
        }

        public async Task<List<FoundationVideoLectureDTO>> GetAllFoundationVideoLecture()
        {
            var videoLectureRequest = new VideoLectureRequest();
            videoLectureRequest.Username = _settings.CurrentUser.Username;
            videoLectureRequest.StdId = _settings.CurrentUser.StandardId;
            var json = JsonConvert.SerializeObject(videoLectureRequest);
            var response = await _restApiHelper.PostAsync<Response<List<FoundationVideoLectureDTO>>>(Endpoint.VideoLectureEndPoint.GetUserFoundationVideoLecture, json);
            return response.ResponseContent;
        }

        public async Task<List<VideoLectureDTO>> GetAllVideoLectures()
        {
            var videoLectureRequest = new VideoLectureRequest();
            videoLectureRequest.Username = _settings.CurrentUser.UserId;
            videoLectureRequest.StdId = _settings.CurrentUser.StandardId;
            var json = JsonConvert.SerializeObject(videoLectureRequest);
            var response = await _restApiHelper.PostAsync<Response<List<VideoLectureDTO>>>(Endpoint.VideoLectureEndPoint.GetAllVideoLectures, json);
            return response.ResponseContent;
        }

       
    }
}
