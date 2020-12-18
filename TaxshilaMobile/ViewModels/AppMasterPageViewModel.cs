using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TaxshilaMobile.Services.Interfaces;

namespace TaxshilaMobile.ViewModels
{
    public class AppMasterPageViewModel : BaseHariKrishnaViewModel
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

        private string _userExtraInfo;

        public string UserExtraInfo
        {
            get { return _userExtraInfo; }
            set { SetProperty(ref _userExtraInfo, value); }
        }

        public ObservableCollection<MyMenuItem> MenuItems { get; set; }
        #endregion

        #region Commands
        private DelegateCommand _navigateCommand;

        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(async () => await Navigate()));

        #endregion

        #region Constructor
        public AppMasterPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IAccountService accountService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            MenuItems = new ObservableCollection<MyMenuItem>();
            SelectedMenuItem = new MyMenuItem();
            _accountService = accountService;
            //MenuItems.Add(new MyMenuItem()
            //{
            //    Icon = FontAwesome.IconFonts.Home,
            //    PageName = PageName.MainPage,
            //    Title = "Home",
            //    IsNavigate = true,
            //    SubTitle = "Dashboard"
            //});
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "Home",
                PageName = PageName.HomePage,
                Title = "Home",
                IsNavigate = true,
                SubTitle = "Home"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "VideoLecture.png",
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
                Icon = "Notice.png",
                PageName = PageName.Notice.NoticeTabbedPage,
                Title = "Notice",
                IsNavigate = true,
                SubTitle = "Notice"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "TeacherDetails.png",
                PageName = PageName.TeacherDetailPage,
                Title = "Teacher details",
                IsNavigate = true,
                SubTitle = "TeacherDetails"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "Homeworkandstudy.png",
                PageName = PageName.HomeworkAndStudyMatireal.HomeworkAndStudyMatirealTabbedPage,
                Title = "Homework and Study material",
                IsNavigate = true,
                SubTitle = "Homework and study"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "DoutSolvingMaterial.png",
                PageName = PageName.DoubtSolvingMaterial.DoubtSolvingMaterialPage,
                Title = "Doubt Solving Material",
                IsNavigate = false,
                SubTitle = "Doubt Solving"
            });
            MenuItems.Add(new MyMenuItem()
            {
                Icon = "DoubtSolvingusingchat.png",
                PageName = "",
                Title = "Doubt Solving using chat",
                IsNavigate = false,
                SubTitle = "Doubt Solving using chat"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "HomeworkSubmission.png",
                PageName = "",
                Title = "Homework and submission",
                IsNavigate = false,
                SubTitle = "Homework and study"
            });


            MenuItems.Add(new MyMenuItem()
            {
                Icon = "TimeTable.png",
                PageName = "",
                Title = "Time table",
                IsNavigate = false,
                SubTitle = "Time table"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ExamTest.png",
                PageName = "",
                Title = "Test/Key",
                IsNavigate = false,
                SubTitle = "Time table"
            });

            MenuItems.Add(new MyMenuItem()
            {
                Icon = "ExamResult.png",
                PageName = "",
                Title = "Result",
                IsNavigate = false,
                SubTitle = "Time table"
            });



            MenuItems.Add(new MyMenuItem()
            {
                Icon = "SignOut.png",
                PageName = PageName.LoginProcess.LoginPage,
                Title = "Logout",
                IsNavigate = false,
                SubTitle = "Logout"
            });
            UserName = _settings.CurrentUser.FullName;
            if (!string.IsNullOrEmpty(_settings.CurrentUser.Group))
            {
                UserExtraInfo = _settings.CurrentUser.StandardName + " " + _settings.CurrentUser.Group;
            }
            else
            {
                UserExtraInfo = _settings.CurrentUser.CellPhone;
            }
        }

        #endregion

        #region Methods
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = false;
        }
        private async Task Navigate()
        {
            if (SelectedMenuItem.IsNavigate)
            {
                
                if (IsConnected)
                {
                    await NavigationService.NavigateAsync($"{PageName.NavigationPage}/{SelectedMenuItem.PageName}");

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
                    case "Teacher details":
                    case "Result":
                    case "Test/Key":
                    case "Time table":
                    case "Homework and study":
                    case "Doubt Solving using chat":
                        {
                            UserDialogsService.Toast(AlertMessages.UnderConstruction);
                            break;
                        }
                }
            }

        }
        #endregion



    }
}
