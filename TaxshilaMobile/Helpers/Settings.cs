using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(AppSettings))]
namespace TaxshilaMobile.Helpers
{
    public class AppSettings : BaseViewModel, IAppSettings, INotifyPropertyChanged
    {
        private ISettings AppSetting
        {
            get { return CrossSettings.Current; }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //bool SetProperty<T>(T value, T defaultValue = default(T), [CallerMemberName] string propertyName = null)
        //{
        //    if (string.IsNullOrWhiteSpace(propertyName)) return false;

        //    if (Equals(AppSettings.GetValueOrDefault(propertyName, defaultValue), value)) return false;

        //    AppSettings.AddOrUpdateValue(propertyName, value);
        //    RaisePropertyChanged(propertyName);

        //    return true;
        //}

        //T GetProperty<T>(T defaultValue = default(T), [CallerMemberName]string propertyName = null)
        //{
        //    if (string.IsNullOrWhiteSpace(propertyName))
        //        return defaultValue;

        //    return AppSettings.GetValueOrDefault(propertyName, defaultValue);
        //}
        #endregion INotifyPropertyChanged

        public bool IsOnline
        {
            get => AppSetting.GetValueOrDefault(nameof(IsOnline), default(bool));
            set => AppSetting.AddOrUpdateValue(nameof(IsOnline), value);
        } 
        public bool IsLoginCall
        {
            get => AppSetting.GetValueOrDefault(nameof(IsLoginCall), default(bool));
            set => AppSetting.AddOrUpdateValue(nameof(IsLoginCall), value);
        }
        public bool IsAccessFoundationFeature
        {
            get => AppSetting.GetValueOrDefault(nameof(IsAccessFoundationFeature), default(bool));
            set => AppSetting.AddOrUpdateValue(nameof(IsAccessFoundationFeature), value);
        }
        


        public string BaseUrl
        {
            get => AppSetting.GetValueOrDefault(nameof(BaseUrl), App.IsLiveCall? App.ProductionServiceUrl : App.StagingServerUrl);
            set => AppSetting.AddOrUpdateValue(nameof(BaseUrl), value);
        }

        public string StagingBaseUrl
        {
            get => AppSetting.GetValueOrDefault(nameof(StagingBaseUrl), App.StagingServerUrl);
            set => AppSetting.AddOrUpdateValue(nameof(StagingBaseUrl), value);
        }
        public string DeviceId
        {
            get => AppSetting.GetValueOrDefault(nameof(DeviceId), string.Empty);
            set => AppSetting.AddOrUpdateValue(nameof(DeviceId), value);
        }
        public string Token
        {
            get => CrossSecureStorage.Current.GetValue(nameof(Token));
            set
            {
                if (value.IsNotNullOrEmpty())
                    CrossSecureStorage.Current.SetValue(nameof(Token), value);
            }
        }
        public DateTime TokenExpirationDate
        {
            get => AppSetting.GetValueOrDefault(nameof(TokenExpirationDate), DateTime.UtcNow);
            set => AppSetting.AddOrUpdateValue(nameof(TokenExpirationDate), value);
        }
        public string DBPath
        {
            get => CrossSecureStorage.Current.GetValue(nameof(DBPath));
            set
            {
                if (value.IsNotNullOrEmpty())
                    CrossSecureStorage.Current.SetValue(nameof(DBPath), value);
            }
        }

        public string AuthorizeUrl
        {
            get => CrossSecureStorage.Current.GetValue(nameof(AuthorizeUrl));
            set
            {
                if (value.IsNotNullOrEmpty())
                    CrossSecureStorage.Current.SetValue(nameof(AuthorizeUrl), value);
            }
        }
        public string AccessTokenUrl
        {
            get => CrossSecureStorage.Current.GetValue(nameof(AccessTokenUrl));
            set
            {
                if (value.IsNotNullOrEmpty())
                    CrossSecureStorage.Current.SetValue(nameof(AccessTokenUrl), value);
            }
        }
        public string RedirectUrl
        {
            get => CrossSecureStorage.Current.GetValue(nameof(RedirectUrl));
            set
            {
                if (value.IsNotNullOrEmpty())
                    CrossSecureStorage.Current.SetValue(nameof(RedirectUrl), value);
            }
        }
        public IUser CurrentUser
        {
            get
            {
                var json = CrossSecureStorage.Current.GetValue(nameof(CurrentUser));
                if (json != null)
                {
                    return JsonConvert.DeserializeObject<User>(json);
                }

                return null;
            }
            set
            {
                CrossSecureStorage.Current.SetValue(nameof(CurrentUser), JsonConvert.SerializeObject(value));
            }
        }
        public LoginStateTypes LoginStatus
        {
            get => (LoginStateTypes)AppSetting.GetValueOrDefault(nameof(LoginStatus), (int)LoginStateTypes.LoggedOut);
            set => AppSetting.AddOrUpdateValue(nameof(LoginStatus), (int)value);
        }
        public string ImageBaseUrl {
            get => AppSetting.GetValueOrDefault(nameof(BaseUrl), App.IsLiveCall ? "https://taxshila.azurewebsites.net/Images/" : "https://taxshila.azurewebsites.net/Images/");
            set => AppSetting.AddOrUpdateValue(nameof(BaseUrl), value);
        }

        public void ResetData()
        {
            CurrentUser = null;
            Token = string.Empty;
            LoginStatus = LoginStateTypes.LoggedOut;

        }
    }
}
