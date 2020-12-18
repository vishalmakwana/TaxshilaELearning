using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.DataTypesApp.Default
{
    public interface IAppSettings
    {
        LoginStateTypes LoginStatus { get; set; }
        bool IsOnline { get; set; }
        bool IsAccessFoundationFeature { get; set; }
        bool IsLoginCall { get; set; }
        string BaseUrl { get; set; }
        string ImageBaseUrl { get; set; }
        string StagingBaseUrl { get; set; }
        string Token { get; set; }
        DateTime TokenExpirationDate { get; set; }
        string DBPath { get; set; }
        string AuthorizeUrl { get; set; }
        string AccessTokenUrl { get; set; }

        string RedirectUrl { get; set; }
        string DeviceId { get; set; }
        IUser CurrentUser { get; set; }
        void ResetData();
    }
}
