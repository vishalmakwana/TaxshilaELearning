using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface INoticeService
    {
        Task<List<UserNoticeDTO>> GetStudentNotice();
        Task<List<PublicEventDTO>> GetStudentPublicEvents();

    }
}
