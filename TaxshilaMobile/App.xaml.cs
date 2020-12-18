using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using System;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Repository;
using TaxshilaMobile.Services.Implementations;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels;
using TaxshilaMobile.ViewModels.DoubtSolvingMaterial;
using TaxshilaMobile.ViewModels.FoundationClass;
using TaxshilaMobile.ViewModels.HomeWorkAndStudyMatireal;
using TaxshilaMobile.ViewModels.LoginProcess;
using TaxshilaMobile.ViewModels.Notice;
using TaxshilaMobile.ViewModels.Popups;
using TaxshilaMobile.ViewModels.VideoLecture;
using TaxshilaMobile.Views;
using TaxshilaMobile.Views.DoubtSolvingMaterial;
using TaxshilaMobile.Views.FoundationClass;
using TaxshilaMobile.Views.HomeWorkAndStudyMatireal;
using TaxshilaMobile.Views.LoginProcess;
using TaxshilaMobile.Views.Notice;
using TaxshilaMobile.Views.Popups;
using TaxshilaMobile.Views.VideoLecture;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace TaxshilaMobile
{
    public partial class App
    {
        /*
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor.
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public static Theme AppTheme { get; set; }
        public static string DebugMode { get; set; }
        public static bool UseLocalService = false;
        public static bool IsCallOnline = false;
        public static bool IsLiveCall = true;
        public static string StagingServerUrl = "https://stagingtaxshila.harikrishnainfotech.com";
        public static string ProductionServiceUrl = "https://taxshila.azurewebsites.net";

        //public static string LocalhostUrl = "https://taxshila.azurewebsites.net";
        public static string LocalhostUrl = "https://taxshila.azurewebsites.net";

        public static DateTime? LastTimeDisplayedAPICallWarning = null;
        public static DateTime? LastFullSyncTime = null;

        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Device.SetFlags(new string[] { "Shapes_Experimental", "Expander_Experimental" });

            //await NavigationService.NavigateAsync(new Uri($"{PageName.NavigationPage}/{PageName.WalkthroughPage}", UriKind.Absolute));
            var _settings = DependencyService.Get<IAppSettings>();
            if (_settings.LoginStatus == LoginStateTypes.LoggedIn)
            {
                await NavigationService.NavigateAsync($"/{PageName.AppMasterPage}/{PageName.NavigationPage}/{PageName.HomePage}");

                //await NavigationService.NavigateAsync($"/{PageName.AppMasterPage}/{PageName.NavigationPage}/{PageName.VideoLecture.VideoLectureListPage}");
            }
            else
            {
                await NavigationService.NavigateAsync($"/{PageName.NavigationPage}/{PageName.LoginProcess.LoginPage}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<VideoLectureTabbedPage, VideoLectureTabbedPageViewModel>(PageName.VideoLecture.VideoLectureTabbedPage);
            containerRegistry.RegisterForNavigation<NavigationPage>(PageName.NavigationPage);
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>(PageName.HomePage);
            containerRegistry.RegisterForNavigation<AppMasterPage, AppMasterPageViewModel>(PageName.AppMasterPage);
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>(PageName.MainPage);
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>(PageName.LoginProcess.LoginPage);
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>(PageName.LoginProcess.RegisterPage);
            containerRegistry.RegisterForNavigation<DefaultPickerPopup, DefaultPickerPopupViewModel>(PageName.Popup.DefaultPickerPopup);

            containerRegistry.RegisterForNavigation<TodayVideoLecturePage, TodayVideoLecturePageViewModel>(PageName.VideoLecture.TodayVideoLecturePage);

            containerRegistry.RegisterForNavigation<VideoLectureListPage, VideoLectureListPageViewModel>(PageName.VideoLecture.VideoLectureListPage);

            containerRegistry.RegisterForNavigation<PlayVideoLecturePage, PlayVideoLecturePageViewModel>(PageName.VideoLecture.PlayVideoLecturePage);

            containerRegistry.RegisterForNavigation<NoticeTabbedPage, NoticeTabbedPageViewModel>(PageName.Notice.NoticeTabbedPage);
            containerRegistry.RegisterForNavigation<NoticeBoardPage, NoticeBoardPageViewModel>(PageName.Notice.NoticeBoardPage);
            containerRegistry.RegisterForNavigation<PublicEventNoticePage, PublicEventNoticePageViewModel>(PageName.Notice.PublicEventNoticePage);

            containerRegistry.RegisterForNavigation<DoubtSolvingMaterialPage, DoubtSolvingMaterialPageViewModel>(PageName.DoubtSolvingMaterial.DoubtSolvingMaterialPage);
            containerRegistry.RegisterForNavigation<ShowDoubtSolvingMaterialPage, ShowDoubtSolvingMaterialPageViewModel>(PageName.DoubtSolvingMaterial.ShowDoubtSolvingMaterialPage);

            containerRegistry.RegisterForNavigation<HomeworkAndStudyMatirealTabbedPage, HomeworkAndStudyMatirealTabbedPageViewModel>(PageName.HomeworkAndStudyMatireal.HomeworkAndStudyMatirealTabbedPage);
            containerRegistry.RegisterForNavigation<HomeWorkPage, HomeWorkPageViewModel>(PageName.HomeworkAndStudyMatireal.HomeWorkPage);
            containerRegistry.RegisterForNavigation<StudyMatirealPage, StudyMatirealPageViewModel>(PageName.HomeworkAndStudyMatireal.StudyMatirealPage);


            containerRegistry.RegisterForNavigation<ReadMoreDescriptionPopupPage, ReadMoreDescriptionPopupPageViewModel>(PageName.Popup.ReadMoreDescriptionPopupPage);
            containerRegistry.RegisterForNavigation<TeacherDetailPage, TeacherDetailPageViewModel>(PageName.TeacherDetailPage);


            containerRegistry.RegisterForNavigation<WalkthroughPage, WalkthroughPageViewModel>(PageName.WalkthroughPage);
            containerRegistry.RegisterForNavigation<NoInterNetConnectionPage, NoInterNetConnectionPageViewModel>(PageName.NoInterNetConnectionPage);
            containerRegistry.RegisterForNavigation<IsReachableOrNotPage, IsReachableOrNotPageViewModel>(PageName.IsReachableOrNotPage);


            containerRegistry.RegisterForNavigation<FoundationVideoLectureTabbedPage, FoundationVideoLectureTabbedPageViewModel>(PageName.VideoLecture.FoundationVideoLectureTabbedPage);
            containerRegistry.RegisterForNavigation<FoundationTodayLecturePage, FoundationTodayLecturePageViewModel>(PageName.VideoLecture.FoundationTodayLecturePage);
            containerRegistry.RegisterForNavigation<FoundationHistoryLecturePage, FoundationHistoryLecturePageViewModel>(PageName.VideoLecture.FoundationHistoryLecturePage);
            containerRegistry.RegisterForNavigation<PlayFoundationVideoLecturePage, PlayFoundationVideoLecturePageViewModel>(PageName.VideoLecture.PlayFoundationVideoLecturePage);

            // Instances
            containerRegistry.RegisterInstance(typeof(IUserDialogs), UserDialogs.Instance);
            containerRegistry.RegisterInstance(typeof(Plugin.Connectivity.Abstractions.IConnectivity), CrossConnectivity.Current);
            containerRegistry.RegisterInstance(typeof(ISettings), CrossSettings.Current);
            containerRegistry.RegisterInstance<IAppSettings>(new AppSettings());

            containerRegistry.Register<IAccountService, AccountService>();
            containerRegistry.Register<IVideoLectureService, VideoLectureService>();
            containerRegistry.Register<IAppSubjectService, AppSubjectService>();
            containerRegistry.Register<INoticeService, NoticeService>();
            containerRegistry.Register<IHomeworkService, HomeworkService>();
            containerRegistry.Register<IStudyMaterialService, StudyMaterialService>();

            containerRegistry.Register<IRepository<SyncStatusModel>, Repository<SyncStatusModel>>();

            containerRegistry.RegisterPopupNavigationService();




        }
    }

    public enum Theme { Light, Dark }
}