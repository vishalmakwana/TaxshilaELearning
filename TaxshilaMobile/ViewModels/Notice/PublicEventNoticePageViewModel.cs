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
using System.Linq;
using System.Threading.Tasks;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Xamarin.Essentials;

namespace TaxshilaMobile.ViewModels.Notice
{
    public class PublicEventNoticePageViewModel : BaseHariKrishnaViewModel, IActiveAware
    {
        #region services
        private readonly INoticeService noticeService;
        #endregion


        #region Properties
        public event EventHandler IsActiveChanged;
        private bool _isActive;


        private ObservableCollection<PublicEventDTO> _itemCollections = new ObservableCollection<PublicEventDTO>();
        public ObservableCollection<PublicEventDTO> ItemCollections
        {
            get { return _itemCollections; }
            set { SetProperty(ref _itemCollections, value); }
        }
        private ObservableCollection<PublicEventDTO> _searchCollection = new ObservableCollection<PublicEventDTO>();
        public ObservableCollection<PublicEventDTO> SearchCollection
        {
            get { return _searchCollection; }
            set { SetProperty(ref _searchCollection, value); }
        }
        private PublicEventDTO _selectedItem = new PublicEventDTO();
        public PublicEventDTO SelectedItem
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
        public Paginator<PublicEventDTO> Paginator { get; set; }

        private const int PageSize = 50;

        #endregion


        #region Command
       
        private DelegateCommand<object> openFileCommand;
        public DelegateCommand<object> OpenFileCommand => openFileCommand ?? (openFileCommand = new DelegateCommand<object>((obj) => OnOpenFileCommandExecuted(obj)));
       
        private DelegateCommand<object> readMoreCommand;
        public DelegateCommand<object> ReadMoreCommand => readMoreCommand ?? (readMoreCommand = new DelegateCommand<object>((obj) => OnReadMoreCommandExecuted(obj)));

       


        #endregion
        #region Constructor

        public PublicEventNoticePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator,INoticeService noticeService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            this.noticeService = noticeService;
        }
        #endregion

        #region Methods


        protected async Task GetPublicEvents()
        {
            IsUpToDate = false;
            SearchCollection = new ObservableCollection<PublicEventDTO>();
            ItemCollections = new ObservableCollection<PublicEventDTO>();

            if (IsConnected)
            {
                try
                {
                    IsBusy = true;
                    var AllItems = await noticeService.GetStudentPublicEvents();
                    AllItems.ForEach(a => a.PublicEventImageUrl = _settings.ImageBaseUrl + a.PublicEventFile);
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
                await GetPublicEvents();

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

        private async void OnOpenFileCommandExecuted(object obj)
        {
            var userNotice = (PublicEventDTO)obj;
            try
            {
                string filepath = await FileExtensions.DownloadFileandSaveInLocalfolderAsync(userNotice.PublicEventImageUrl, userNotice.PublicEventFile);
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filepath)
                });
            }
            catch (Exception ex)
            {

            }
        }
        private async void OnReadMoreCommandExecuted(object obj)
        {
            var StudentHomeWork = (PublicEventDTO)obj;
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
        #endregion
    }
}
