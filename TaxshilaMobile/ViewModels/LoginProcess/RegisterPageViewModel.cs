using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.Validations;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxshilaMobile.ViewModels.LoginProcess
{
    public class RegisterPageViewModel : BaseHariKrishnaViewModel
    {
        #region Services
        private readonly IAccountService _accountService;
        #endregion


        #region Properties

        #region User Email
        private ValidatableObject<string> _userEmail;
        public ValidatableObject<string> UserEmail
        {
            get
            {
                return _userEmail;
            }
            set
            {
                _userEmail = value;
                RaisePropertyChanged(() => _userEmail);
            }
        }

        private DelegateCommand _validateUserEmailCommand;

        public DelegateCommand ValidateUserEmailCommand =>
            _validateUserEmailCommand ?? (_validateUserEmailCommand = new DelegateCommand(() => ExecuteValidateUserEmailCommand()));

        private bool ExecuteValidateUserEmailCommand()
        {
            return _userEmail.Validate();
        }

        #endregion
        #region Password
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

        private DelegateCommand _validatePasswordCommand;

        public DelegateCommand ValidatePasswordCommand =>
            _validatePasswordCommand ?? (_validatePasswordCommand = new DelegateCommand(() => ExecuteValidatePasswordCommand()));
        private bool ExecuteValidatePasswordCommand()
        {
            return _password.Validate();
        }
        #endregion
        #region ConfirmPassowrd

        private ValidatableObject<string> _confirmpassword;

        public ValidatableObject<string> ConfirmPassword
        {
            get
            {
                return _confirmpassword;
            }
            set
            {
                _confirmpassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }
        private DelegateCommand _validateConfirmPasswordCommand;

        public DelegateCommand ValidateConfirmPasswordCommand =>
            _validateConfirmPasswordCommand ?? (_validateConfirmPasswordCommand = new DelegateCommand(() => ExecuteValidateConfirmPasswordCommand()));

        private bool ExecuteValidateConfirmPasswordCommand()
        {
            return _confirmpassword.Validate();
        }
        #endregion
        #region PhoneNumber

        private ValidatableObject<string> _phonenumber;

        public ValidatableObject<string> Phonenumber
        {
            get
            {
                return _phonenumber;
            }
            set
            {
                _phonenumber = value;
                RaisePropertyChanged(() => Phonenumber);
            }
        }

        private DelegateCommand _validatePhoneNumberCommand;

        public DelegateCommand ValidatePhoneNumberCommand =>
            _validatePhoneNumberCommand ?? (_validatePhoneNumberCommand = new DelegateCommand(() => ExecuteValidatePhoneNumberCommand()));

        private bool ExecuteValidatePhoneNumberCommand()
        {
            return _phonenumber.Validate();
        }

        #endregion
        #region UserName      

        private ValidatableObject<string> _username;

        public ValidatableObject<string> UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                RaisePropertyChanged(() => UserName);
            }
        }
        private DelegateCommand _validateUserNameCommand;

        public DelegateCommand ValidateUserNameCommand =>
            _validateUserNameCommand ?? (_validateUserNameCommand = new DelegateCommand(() => ExecuteValidateUserNameCommand()));

        private bool ExecuteValidateUserNameCommand()
        {
            return _username.Validate();
        }
        #endregion

        #endregion

        #region Command
        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand =>
            registerCommand ?? (registerCommand = new DelegateCommand(ExecuteRegisterCommand));

        private DelegateCommand loginPageNavigationCommand;
        public DelegateCommand LoginPageNavigationCommand =>
            loginPageNavigationCommand ?? (loginPageNavigationCommand = new DelegateCommand(async () =>
            {                await NavigationService.GoBackAsync();

            }));
        private async void ExecuteRegisterCommand()
        {
            try
            {
              
                ValidateFields();
                if (_userEmail.IsValid && _username.IsValid && _password.IsValid && _confirmpassword.IsValid && _phonenumber.IsValid)
                {
                    IsBusy = true;
                    var RegisterResponse = await _accountService.RegisterUser(UserName.Value.Trim(), Password.Value.Trim(), ConfirmPassword.Value.Trim(), Phonenumber.Value.Trim(), UserEmail.Value.Trim());
                    if (RegisterResponse.Success == true)
                    {
                        var LoginResponse = await _accountService.LoginUserWithCreateToken(UserEmail.Value.Trim(), Password.Value.Trim());
                        IsBusy = false;
                        if (LoginResponse.Success)
                        {
                            
                            if (LoginResponse.Success)
                            {
                                UserInfoDTO obj = new UserInfoDTO();
                                obj = LoginResponse.ResponseContent;
                                IUser user = new Models.User
                                {
                                    Username = UserEmail.Value,
                                    Password = Password.Value,
                                    Email = obj.Email,
                                    FullName = obj.Name,
                                    CellPhone = obj.PhoneNumber,
                                    UserId = obj.UserId,
                                };


                                _settings.Token = obj.Token;
                                _settings.TokenExpirationDate = obj.ValidTo??DateTime.Now.AddDays(10);
                                _settings.CurrentUser = user;
                                _settings.LoginStatus = LoginStateTypes.LoggedIn;
                                _settings.IsLoginCall = true;
                                await NavigationService.NavigateAsync(new Uri($"/{PageName.AppMasterPage}/{PageName.NavigationPage}/{PageName.Categories.CategoriesPage}", UriKind.RelativeOrAbsolute));
                            }
                            else
                            {
                                UserDialogsService.Alert(LoginResponse.Message, "Error!", "Ok");
                            }

                        }
                        IsBusy = false;
                    }
                    else
                    {
                        IsBusy = false;
                        UserDialogsService.Alert(RegisterResponse.Message, "Error!", "Ok");
                    }

                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
        }

        private void ValidateFields()
        {
            ValidateUserNameCommand.Execute();
            ValidatePhoneNumberCommand .Execute();
            ValidateUserEmailCommand.Execute();
            ValidatePasswordCommand.Execute();
            ValidateConfirmPasswordCommand.Execute();
        }
        #endregion

        #region Constructors
        public RegisterPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IAccountService accountService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _accountService = accountService;
            UserName = new ValidatableObject<string>();
            UserEmail = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            ConfirmPassword = new ValidatableObject<string>();
            Phonenumber = new ValidatableObject<string>();
            AddValidations();
            IsBusy = false;
        }

        #endregion

        #region Methods

        private void AddValidations()
        {
            _username.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Name is required"
            });

            _userEmail.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Email address is required"
            });
            _userEmail.Validations.Add(new EmailRule<string>
            {
                ValidationMessage = "Enter valid Email address"
            });


            _phonenumber.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Enter valid Email address"
            });

            _phonenumber.Validations.Add(new MobilenumberRule<string>
            {
                ValidationMessage = "Enter valid mobile number"
            });


            _password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter password"
            });

            _password.Validations.Add(new PasswordRule<string>
            {
                ValidationMessage = "Password must have at least one non alphanumeric character \n,Password must have at least one digit('0'-'9')\n Password must have at least one uppercase('A'-'Z')"
            });




            _confirmpassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter confirm password"
            });

            _confirmpassword.Validations.Add(new CompareRule<string>
            {
                ValidationMessage = "Password must match",
                CompareFunction = () => _password.Value
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsBusy = true;

            IsBusy = false;
        }
        #endregion


    }
}
