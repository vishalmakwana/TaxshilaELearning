using TaxshilaMobile.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Services.Implementations
{
    public class MediaEventArgs : EventArgs
    {
        public MediaAsset Media { get; }
        public MediaEventArgs(MediaAsset media)
        {
            Media = media;
        }
    }
}
