using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxshilaMobile.DataTypesApp.Default;
using DryIoc;

namespace TaxshilaMobile.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly RestApiHelper _restApiHelper;
        private readonly RestApiResponseHelper _restApiResponseHelper;
        private readonly IAppSettings _settings;
        public AccountService(IAppSettings settings)
        {
            _restApiHelper = new RestApiHelper();
            _settings = settings;
            _restApiResponseHelper = new RestApiResponseHelper();
        }

        public async Task<List<UserInfoDTO>> AllTeacher()
        {
            var data = new MobileRequest();
            data.Username = _settings.CurrentUser.Username;

            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<List<UserInfoDTO>>>(Endpoint.GetAllTeachers, json).ConfigureAwait(false);
            return response.ResponseContent;
        }

        public async Task<bool> CheckUserIsValid()
        {
            var data = new ValidateUserRequest();
            data.UserName = _settings.CurrentUser.Username;
            data.DeviceId = _settings.DeviceId;
            data.Password = _settings.CurrentUser.Password;

            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<bool>>(Endpoint.LoginEndpoint.CheckUserIsValid, json).ConfigureAwait(false);
            return response.ResponseContent;
        }

        public async Task<Response<LoginResponse>> LoginUser(string username, string password)
        {
            var data = new LoginUserRequest();
            data.UserName = username;
            data.Password = password;
            data.Email = username;
            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<LoginResponse>>(Endpoint.LoginEndpoint.Authenticate, json).ConfigureAwait(false);
            return response;
        }

        public async Task<Response<UserInfoDTO>> LoginUserWithCreateToken(string username, string password)
        {
            var data = new LoginUserRequest();
            data.UserName = username;
            data.Password = password;
            data.Email = username;
            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<UserInfoDTO>>(Endpoint.LoginEndpoint.CreateToken, json).ConfigureAwait(false);
            return response;
        }

        public async Task<Response<UserInfoDTO>> LoginValidateUser(string username, string password, bool IsForceToUpdate = false)
        {
            var data = new LoginUserRequest();
            data.UserName = username;
            data.Password = password;
            data.DeviceId = _settings.DeviceId;
            data.VerificationCode = _settings.DeviceId;
            data.IsForceToUpdate = IsForceToUpdate;
            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<UserInfoDTO>>(Endpoint.LoginEndpoint.ValidateUser, json).ConfigureAwait(false);
            return response;
        }

        public async Task<Response<bool>> LogOut()
        {
            var data = new LoginUserRequest();
            data.UserName = _settings.CurrentUser.Username;
            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<bool>>(Endpoint.LoginEndpoint.LogOut, json).ConfigureAwait(false);
            return response;
        }

        public async Task<Response<UserInfoDTO>> RegisterUser(string username, string password, string confirmPassword, string phoneNumber, string EmailAddress)
        {
            var data = new MobileRegisterRequest();
            data.Email = EmailAddress;
            data.ConfirmPassword = confirmPassword;
            data.Phonenumber = phoneNumber;
            data.Password = password;
            data.Name = username;
            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<UserInfoDTO>>(Endpoint.LoginEndpoint.Register, json).ConfigureAwait(false);
            return response;
        }

        public async Task<UserInfoDTO> ValidateUserInfo()
        {
            var data = new ValidateUserRequest();
            data.UserName = _settings.CurrentUser.Username;
            data.DeviceId = _settings.DeviceId;
            data.Password = _settings.CurrentUser.Password;
            var json = JsonConvert.SerializeObject(data);
            var response = await _restApiHelper.PostAsync<Response<UserInfoDTO>>(Endpoint.LoginEndpoint.ValidateUserInfo, json).ConfigureAwait(false);
            return response.ResponseContent;
        }
    }
}
