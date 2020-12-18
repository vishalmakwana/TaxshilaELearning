using TaxshilaMobile.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Response<LoginResponse>> LoginUser(string username, string password);
        Task<Response<UserInfoDTO>> LoginUserWithCreateToken(string username, string password);

        Task<Response<UserInfoDTO>> RegisterUser(string username, string password, string confirmPassword, string phoneNumber, string EmailAddress);

        Task<Response<bool>> LogOut();
        Task<Response<UserInfoDTO>> LoginValidateUser(string username, string password, bool IsForceToUpdate);

        Task<List<UserInfoDTO>> AllTeacher();

        Task<bool> CheckUserIsValid();
        Task<UserInfoDTO> ValidateUserInfo();

    }
}
