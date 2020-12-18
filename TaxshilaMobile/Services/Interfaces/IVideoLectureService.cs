using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OnlineSync.Models;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface IVideoLectureService
    {

        Task<List<VideoLectureDTO>> GetAllVideoLectures();
        Task<List<FoundationVideoLectureDTO>> GetAllFoundationVideoLecture();
    }
}

