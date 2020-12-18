using System;
using System.Threading.Tasks;

namespace TaxshilaMobile
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}
