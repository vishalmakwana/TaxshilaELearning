using Acr.UserDialogs;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.Validations;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace TaxshilaMobile.ViewModels.LoginProcess
{
    public class LoginPageViewModel : BaseHariKrishnaViewModel
    {
        #region Services
        private readonly IAccountService _accountService;
        #endregion

        #region Properties
        private ValidatableObject<string> _userName;

        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        private ValidatableObject<string> _password;

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }
        #endregion


        #region Constructor
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IAccountService accountService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _accountService = accountService;
            UserName = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            AddValidations();
            _settings.Token = "";

        }
        #endregion
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = false;
            //if (_settings.LoginStatus==LoginStateTypes.LoggedIn) 
            //{

            //}
            base.OnNavigatedTo(parameters);

        }
        #region Commands
        private DelegateCommand loginCommand;
        public DelegateCommand LoginCommand =>
            loginCommand ?? (loginCommand = new DelegateCommand(ExecuteLoginCommand));


        private DelegateCommand signupcommand;
        public DelegateCommand SignupCommand =>
            signupcommand ?? (signupcommand = new DelegateCommand(ExecutesignupCommand));

        private DelegateCommand _validateUserNameCommand;

        public DelegateCommand ValidateUserNameCommand =>
            _validateUserNameCommand ?? (_validateUserNameCommand = new DelegateCommand(() => ExecuteValidateUserNameCommand()));

        private DelegateCommand _validateUserPassword;

        public DelegateCommand ValidateUserPasswordCommand =>
            _validateUserPassword ?? (_validateUserPassword = new DelegateCommand(() => ExecuteValidateUserPasswordCommand()));

        #endregion

        #region Methods
        async void ExecuteLoginCommand()
        {
            try
            {
                if (IsConnected)
                {


                    ValidateFields();
                    if (_userName.IsValid && _password.IsValid)
                    {
                        IsBusy = true;

                        if (string.IsNullOrWhiteSpace(_settings.DeviceId))
                        {
                            var id = System.Guid.NewGuid().ToString();
                            _settings.DeviceId = id;
                        }
                        var response = await _accountService.LoginValidateUser(UserName.Value.Trim(), Password.Value.Trim(), false);
                        if (response.Success)
                        {
                            UserInfoDTO obj = new UserInfoDTO();
                            obj = response.ResponseContent;
                            IUser user = new Models.User
                            {
                                Username = UserName.Value,
                                Password = Password.Value,
                                Email = obj.Email,
                                FullName = obj.Name,
                                CellPhone = obj.PhoneNumber,
                                UserId = obj.UserId,
                                SchoolName = obj.SchoolName,
                                FatherName = obj.FatherName,
                                District = obj.District,
                                Medium = obj.Medium,
                                Std = obj.Std,
                                Group = obj.Group,
                                StandardId = obj.StandardId,
                                StandardName = obj.StandardName,
                                ClassId = obj.ClassId,
                                ClassName = obj.ClassName,
                                AccessFoundationFeature = obj.AccessFoundationFeature
                            };


                            _settings.Token = obj.Token;
                            _settings.IsAccessFoundationFeature = user.AccessFoundationFeature;

                            _settings.TokenExpirationDate = obj.ValidTo ?? DateTime.Now.AddDays(60);

                            _settings.CurrentUser = user;
                            _settings.LoginStatus = LoginStateTypes.LoggedIn;
                            _settings.IsLoginCall = true;
                            await NavigationService.NavigateAsync($"/{PageName.AppMasterPage}/{PageName.NavigationPage}/{PageName.HomePage}");





                            //var handler = new JwtSecurityTokenHandler();
                            //if (handler.ReadToken(response.ResponseContent.Jwt) is JwtSecurityToken token)
                            //{
                            //    IUser user = new Models.User
                            //    {
                            //        Username = UserName.Value,
                            //        Password = Password.Value,
                            //        Email = token.Claims.First(claim => claim.Type == "Email").Value,
                            //        FullName = token.Claims.First(claim => claim.Type == "Name").Value,
                            //        CellPhone = token.Claims.First(claim => claim.Type == "PhoneNumber").Value,
                            //        UserId = token.Claims.First(claim => claim.Type == "UserId").Value,
                            //    };


                            //    _settings.Token = response.ResponseContent.Jwt;
                            //    _settings.TokenExpirationDate = token.ValidTo;

                            //    _settings.CurrentUser = user;
                            //    _settings.LoginStatus = LoginStateTypes.LoggedIn;
                            //    _settings.IsLoginCall = true;
                            //    await NavigationService.NavigateAsync(new Uri($"/{PageName.AppMasterDetailPage}/{PageName.NavigationPage}/{PageName.MainPage}", UriKind.RelativeOrAbsolute));

                            //}
                        }
                        else
                        {
                            IsBusy = false;
                            if (response.Message.ToLower().Contains("already"))
                            {
                                var confirm = await UserDialogsService.ConfirmAsync("If you want to register this device, then choose, Yes Thank You.", response.Message, "Yes", "No");
                                if (confirm)
                                {
                                    IsBusy = true;
                                    var ForceToLoginResponse = await _accountService.LoginValidateUser(UserName.Value.Trim(), Password.Value.Trim(), true);
                                    IsBusy = false;

                                    if (ForceToLoginResponse.Success)
                                    {
                                        UserDialogsService.Alert("Your device register successfully. Login Now", "Message", "Ok");
                                    }
                                    else
                                    {
                                        UserDialogsService.Alert(ForceToLoginResponse.Message, "Message", "Ok");
                                    }

                                }
                            }
                            else
                            {
                                UserDialogsService.Alert(response.Message, "Message", "Ok");
                            }


                        }
                        IsBusy = false;
                    }
                }
                else
                {
                    UserDialogsService.Alert(AlertMessages.GoOnline, "Error!", "Ok");

                }
            }
            catch (Exception)
            {


            }


        }

        public void ValidateFields()
        {
            ValidateUserPasswordCommand.Execute();
            ValidateUserNameCommand.Execute();
        }

        private bool ExecuteValidateUserNameCommand()
        {
            return _userName.Validate();
        }

        private bool ExecuteValidateUserPasswordCommand()
        {
            return _password.Validate();
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Login Id is required"
            });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter password"
            });
        }


        private async void ExecutesignupCommand()
        {

            //  MessageTextColor = Color.FromHex(Convert.ToString(App.Current.Resources["mainLabel"])),
            //var BackgroundColor = Convert.ToString((Xamarin.Forms.Application)App.Current.Resources["background"]);

            ToastConfig toastConfig = new ToastConfig(AlertMessages.UnderConstruction)
            {
                Position = ToastPosition.Bottom,

                Message = AlertMessages.UnderConstruction
            };
            UserDialogsService.Toast(toastConfig);

            //await NavigationService.NavigateAsync(PageName.LoginProcess.RegisterPage);
        }
        #endregion


    }
}
