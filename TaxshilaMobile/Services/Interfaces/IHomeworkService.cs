using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface IHomeworkService 
    {
        Task<List<HomeworkDTO>> GetStudentHomeWork();

    }
}
