using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.Navigation;
using Prism.Services;
using TaxshilaMobile.Services.Interfaces;
using System.Collections.ObjectModel;

using System.Threading.Tasks;
using Prism.Events;
namespace TaxshilaMobile.ViewModels
{
    public class HomePageViewModel : BaseHariKrishnaViewModel
    {
        #region service
        private readonly IAccountService _accountService;


        #endregion

        #region Properties


        private MyMenuItem _selectedMenuItem;

        public MyMenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { SetProperty(ref _selectedMenuItem, value); }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _std;

        public string Std
        {
            get { return _std; }
            set { SetProperty(ref _std, value); }
        }

        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }


        public ObservableCollection<MyMenuItem> MenuItems { get; set; }
        #endregion
        #region Commands
        private DelegateCommand _navigateCommand;

        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(async () => await Navigate()));
        private DelegateCommand<MyMenuItem> collectionViewTapCommand;
        public DelegateCommand<MyMenuItem> CollectionViewTapCommand =>
          collectionViewTapCommand ?? (collectionViewTapCommand = new DelegateCommand<MyMenuItem>(async (a) => await CollectionViewTapCommandExecute(a)));

        private async Task CollectionViewTapCommandExecute(MyMenuItem a)
        {
            SelectedMenuItem = a;
            await Navigate();
        }

        #endregion
        public HomePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IAccountService accountService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

            IsBusy = true;
            IsBusy = false;

            _accountService = accountService;
            MenuItems = new ObservableCollection<MyMenuItem>();
            SelectedMenuItem = new MyMenuItem();
            UserName = settings.CurrentUser.FullName;
            Std = settings.CurrentUser.StandardName;
            UserId = settings.CurrentUser.Username;
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "Notice",
                PageName = PageName.Notice.NoticeTabbedPage,
                Title = "Notice",
                IsNavigate = true,
                SubTitle = "Dashboard"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "VideoLecture",
                PageName = PageName.VideoLecture.VideoLectureTabbedPage,
                Title = "Video Lectures",
                IsNavigate = true,
                SubTitle = "Dashboard"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "Foundations.png",
                PageName = PageName.VideoLecture.FoundationVideoLectureTabbedPage,
                Title = "Foundation Lecture",
                IsNavigate = true,
                SubTitle = "Dashboard"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "Homeworkandstudy",
                PageName = PageName.HomeworkAndStudyMatireal.HomeworkAndStudyMatirealTabbedPage,
                Title = "Homework and Study",
                IsNavigate = true,
                SubTitle = "Homework and Study material"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "TeacherDetails",
                PageName = PageName.TeacherDetailPage,
                Title = "Teacher details",
                IsNavigate = true,
                SubTitle = "TeacherDetails"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "HomeworkSubmission",
                PageName = "HomeworkandSubmission",
                Title = "Homework Submission",
                IsNavigate = false,
                SubTitle = "Homework Submission "
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "DoubtSolvingusingchat",
                PageName = "",
                Title = "Doubt Solving using chat",
                IsNavigate = false,
                SubTitle = "Doubt Solving using chat"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "DoutSolvingMaterial",
                PageName = PageName.DoubtSolvingMaterial.DoubtSolvingMaterialPage,
                Title = "Doubt Solving",
                IsNavigate = false,
                SubTitle = "Doubt Solving"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "TimeTable",
                PageName = "",
                Title = "Time table",
                IsNavigate = false,
                SubTitle = "Time table"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ExamTest",
                PageName = "",
                Title = "Test/Key",
                IsNavigate = false,
                SubTitle = "Time table"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ExamResult",
                PageName = PageName.IsReachableOrNotPage,
                Title = "Result",
                IsNavigate = false,
                SubTitle = "Time table"
            });



            //MenuItems.Add(new MyMenuItem()
            //{
            //    Icon = FontAwesome.IconFonts.Video,
            //    PageName = "Homework",
            //    Title = "Homework and Study material",
            //    IsNavigate = false,
            //    SubTitle = "Homework and Study material"
            //});
            //MenuItems.Add(new MyMenuItem()
            //{
            //    Icon = FontAwesome.IconFonts.SignOutAlt,
            //    PageName = PageName.LoginProcess.LoginPage,
            //    Title = "Logout",
            //    IsNavigate = false,
            //    SubTitle = "Logout"
            //});
            //UserName = _settings.CurrentUser.Username;
        }

        #region Methods
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = true;
            IsBusy = false;
            if (IsConnected)
            {
                await CheckUserIsValid();

            }

        }
        private async Task Navigate()
        {


            if (SelectedMenuItem.IsNavigate)
            {
                if (IsConnected)
                {
                    await NavigationService.NavigateAsync($"{SelectedMenuItem.PageName}");

                }
                else
                {
                    UserDialogsService.Toast(AlertMessages.NoInternet);

                }


            }
            else
            {
                switch (SelectedMenuItem.Title)
                {
                    case "Logout":
                        {
                            if (IsConnected)
                            {

                                var confirm = await UserDialogsService.ConfirmAsync("Are you sure want to Logout");
                                if (confirm)
                                {
                                    IsBusy = true;
                                    var result = await _accountService.LogOut();
                                    if (!result.ResponseContent)
                                    {
                                        IsBusy = false;
                                        UserDialogsService.Toast(result.Message, new TimeSpan(5));
                                    }
                                    else
                                    {
                                        IsBusy = false;
                                        await Logout();
                                    }
                                    IsBusy = false;


                                }
                            }
                            else
                            {
                                UserDialogsService.Toast(AlertMessages.NoInternet);
                            }

                            break;
                        }
                    case "Homework and Study material":
                    case "Homework Submission":
                    case "Teacher details":
                    case "Result":
                    case "Test/Key":
                    case "Time table":
                    case "Doubt Solving using chat":
                        {
                            UserDialogsService.Toast(AlertMessages.UnderConstruction);
                            break;

                        }
                }
            }

        }

        public async override void Initialize(INavigationParameters parameters)
        {

            IsBusy = true;
            IsBusy = false;
            base.Initialize(parameters);


        }

        private async Task CheckUserIsValid()
        {
            try
            {
                if (IsConnected)
                {
                    IsBusy = true;
                    //var Result = await _accountService.CheckUserIsValid();
                    var userValidationCheck = await _accountService.ValidateUserInfo();
                   
                    _settings.IsAccessFoundationFeature = userValidationCheck.AccessFoundationFeature;

                    if (_settings.LoginStatus == LoginStateTypes.LoggedIn)
                    {
                        var status = _settings.TokenExpirationDate < DateTime.UtcNow ? LoginStateTypes.LoggedOut : LoginStateTypes.LoggedIn;
                        switch (status)
                        {
                            case LoginStateTypes.LoggedIn:
                                break;
                            default:
                                var response = await _accountService.LogOut();
                                if (response.ResponseContent)
                                {
                                    _settings.ResetData();
                                    await NavigationService.NavigateAsync(new Uri($"/{PageName.NavigationPage}/{PageName.LoginProcess.LoginPage}", UriKind.Absolute));
                                }
                                break;
                        }

                    }
                    IsBusy = false;

                    if (!userValidationCheck.IsLogin)
                    {
                        _settings.ResetData();
                        await NavigationService.NavigateAsync(new Uri($"/{PageName.NavigationPage}/{PageName.LoginProcess.LoginPage}", UriKind.RelativeOrAbsolute), useModalNavigation: true);
                    }
                    IsBusy = false;

                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }



        }
        #endregion
    }
}
