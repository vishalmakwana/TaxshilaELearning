using TaxshilaMobile.Models.Common;
using TaxshilaMobile.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface IMediaService
    {
        event EventHandler<MediaEventArgs> OnMediaAssetLoaded;
        bool IsLoading { get; }
        Task<IList<MediaAsset>> RetrieveMediaAssetsAsync(CancellationToken? token = null);
    }
}
