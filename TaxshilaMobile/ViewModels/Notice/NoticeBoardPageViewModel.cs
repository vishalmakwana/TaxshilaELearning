using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Sharpnado.Presentation.Forms.Paging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TaxshilaMobile.ViewModels.Notice
{
    public class NoticeBoardPageViewModel : BaseHariKrishnaViewModel, IActiveAware
    {

        #region services
        private readonly INoticeService noticeService;
        #endregion

        #region Properties
        public event EventHandler IsActiveChanged;
        private bool _isActive;


        private ObservableCollection<UserNoticeDTO> _itemCollections = new ObservableCollection<UserNoticeDTO>();
        public ObservableCollection<UserNoticeDTO> ItemCollections
        {
            get { return _itemCollections; }
            set { SetProperty(ref _itemCollections, value); }
        }
        private ObservableCollection<UserNoticeDTO> _searchCollection = new ObservableCollection<UserNoticeDTO>();
        public ObservableCollection<UserNoticeDTO> SearchCollection
        {
            get { return _searchCollection; }
            set { SetProperty(ref _searchCollection, value); }
        }
        private UserNoticeDTO _selectedItem = new UserNoticeDTO();
        public UserNoticeDTO SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, "IsActive", RaiseIsActiveChanged); }
        }
        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }
        private bool isUpToDate = false;
        public bool IsUpToDate
        {
            get { return isUpToDate; }
            set { SetProperty(ref isUpToDate, value); }
        }
        private ObservableCollection<int> _itemIds = new ObservableCollection<int>();
        public ObservableCollection<int> ItemIds
        {
            get { return _itemIds; }
            set { SetProperty(ref _itemIds, value); }
        }

        public int CurrentPage { get; set; } = 1;
        public Paginator<UserNoticeDTO> Paginator { get; set; }

        private const int PageSize = 50;

        #endregion


        #region Command
        private DelegateCommand<object> openFileCommand;
        public DelegateCommand<object> OpenFileCommand => openFileCommand ?? (openFileCommand = new DelegateCommand<object>((obj) => OnOpenFileCommandExecuted(obj)));

        private DelegateCommand<object> readMoreCommand;
        public DelegateCommand<object> ReadMoreCommand => readMoreCommand ?? (readMoreCommand = new DelegateCommand<object>((obj) => OnReadMoreCommandExecuted(obj)));


        
        #endregion

        #region Constructor
        public NoticeBoardPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, INoticeService noticeService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            this.noticeService = noticeService;
        }
        #endregion



        #region Methods


        protected async Task GetUserNotice()
        {
            IsUpToDate = false;
            SearchCollection = new ObservableCollection<UserNoticeDTO>();
            ItemCollections = new ObservableCollection<UserNoticeDTO>();

            if (IsConnected)
            {
                try
                {
                    IsBusy = true;
                    var AllItems = await noticeService.GetStudentNotice();
                    AllItems.ForEach(a => a.NoticeImageUrl = _settings.ImageBaseUrl + a.NoticeFile);
                    SearchCollection.AddRange(AllItems);
                    ItemCollections.AddRange(AllItems);
                    IsBusy = false;
                    IsEmpty = !ItemCollections.AnyExtended();
                    IsBusy = false;

                }
                catch (Exception ex)
                {

                    IsBusy = false;
                    IsEmpty = true;
                    EmptyStateTitle = AlertMessages.EmptyState.DefaultTitle;
                }
            }
            else
            {
                IsBusy = false;
                IsEmpty = true;
                EmptyStateTitle = AlertMessages.NoInternet;
            }

        }
        protected async virtual void RaiseIsActiveChanged()
        {
            //GetAllVideoLectures();
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
            if (IsActive == true)
            {
                await GetUserNotice();

            }

        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        private async void OnReadMoreCommandExecuted(object obj)
        {
            var StudentHomeWork = (UserNoticeDTO)obj;
            try
            {
                NavigationParameters navigationparameter = new NavigationParameters();
                ShowMoreItem showMoreItem = new ShowMoreItem();
                showMoreItem.Description = StudentHomeWork.Descriptions;
                showMoreItem.IsSubjectVisible = false;
                navigationparameter.Add("ShowMoreItem", showMoreItem);
                await NavigationService.NavigateAsync(PageName.Popup.ReadMoreDescriptionPopupPage, navigationparameter);
            }
            catch (Exception ex)
            {

            }
        }

        private async void OnOpenFileCommandExecuted(object obj)
        {
            var userNotice = (UserNoticeDTO)obj;
            try
            {
                string filepath = await FileExtensions.DownloadFileandSaveInLocalfolderAsync(userNotice.NoticeImageUrl, userNotice.NoticeFile);
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filepath)
                });
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
