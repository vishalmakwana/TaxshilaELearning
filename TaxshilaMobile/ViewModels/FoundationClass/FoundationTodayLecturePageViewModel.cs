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
using TaxshilaMobile.Models.Common;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;

namespace TaxshilaMobile.ViewModels.FoundationClass
{
    public class FoundationTodayLecturePageViewModel :  BaseHariKrishnaViewModel, IActiveAware
    {
        #region Services
        private readonly IVideoLectureService _videoLectureService;
        private readonly IAppSubjectService subjectService;
        #endregion
        #region Properties

        private System.Collections.ObjectModel.ObservableCollection<GroupedDataList<FoundationVideoLectureDTO>> _groupedSubjectViseVideoData = new System.Collections.ObjectModel.ObservableCollection<GroupedDataList<FoundationVideoLectureDTO>>();
        public System.Collections.ObjectModel.ObservableCollection<GroupedDataList<FoundationVideoLectureDTO>> GroupedSubjectViseVideoData
        {
            get { return _groupedSubjectViseVideoData; }
            set { SetProperty(ref _groupedSubjectViseVideoData, value); }
        }

        public event EventHandler IsActiveChanged;
        private bool _isActive;
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

        private FoundationVideoLectureDTO _selectedProduct = new FoundationVideoLectureDTO();
        public FoundationVideoLectureDTO SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }
        
        private ObservableCollection<FoundationVideoLectureDTO> _itemCollections = new ObservableCollection<FoundationVideoLectureDTO>();
        public ObservableCollection<FoundationVideoLectureDTO> ItemCollections
        {
            get { return _itemCollections; }
            set { SetProperty(ref _itemCollections, value); }
        }

        private ObservableCollection<FoundationVideoLectureDTO> _searchCollection = new ObservableCollection<FoundationVideoLectureDTO>();
        public ObservableCollection<FoundationVideoLectureDTO> SearchCollection
        {
            get { return _searchCollection; }
            set { SetProperty(ref _searchCollection, value); }
        }

        private ObservableCollection<int> _itemIds = new ObservableCollection<int>();
        public ObservableCollection<int> ItemIds
        {
            get { return _itemIds; }
            set { SetProperty(ref _itemIds, value); }
        }

        public int CurrentPage { get; set; } = 1;
        public Paginator<FoundationVideoLectureDTO> Paginator { get; set; }

        private const int PageSize = 50;
        #endregion



        #region Command
        private DelegateCommand<object> itemSelected;
        public DelegateCommand<object> ItemSelectedCommand =>
            itemSelected ?? (itemSelected = new DelegateCommand<object>(OnItemSelectedCommandExecuted));
      
        #endregion


        public FoundationTodayLecturePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IVideoLectureService videoLectureService, IAppSubjectService subjectService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _videoLectureService = videoLectureService;
            this.subjectService = subjectService;
        }

        #region Methods


        async public override void OnNavigatedTo(INavigationParameters parameters)
        {

            base.OnNavigatedTo(parameters);

            //GroupedSubjectViseVideoData = new ObservableCollection<GroupedDataList<VideoLectureDTO>>();
            //await GetAllVideoLectures();
            // await GetAllVideoLectures();
        }


        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            await GetAllVideoLectures();

        }
        async Task GetAllVideoLectures()
        {
            if (!IsConnected)
            {
                IsEmpty = true;
                EmptyStateTitle = AlertMessages.NoInternet;
            }
            else
            {
                IsUpToDate = false;
                SetSortOrder = new SortOrder();
                SetSortOrder.ColumnName = "VideoLectureName";
                SetSortOrder.Title = "VideoLectureName";
                SetSortOrder.SortTypes = SortTypes.Ascending;


                SearchCollection = new ObservableCollection<FoundationVideoLectureDTO>();
                ItemCollections = new ObservableCollection<FoundationVideoLectureDTO>();
                if (IsConnected)
                {
                    IsBusy = true;
                    if (_settings.IsAccessFoundationFeature) 
                    {
                        var AllItems = await _videoLectureService.GetAllFoundationVideoLecture();
                        var TodayLectures = AllItems.Where(a => a.PublishDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy")).ToList();
                        SearchCollection.AddRange(TodayLectures);
                        ItemCollections.AddRange(TodayLectures);
                        IsBusy = false;
                        IsEmpty = !ItemCollections.AnyExtended();
                        IsBusy = false;
                    }
                    else
                    {
                        IsEmpty = true;
                        IsBusy = false;
                        EmptyStateTitle = AlertMessages.YouAreNotEligible;
                        EmptyStateSubtitle = string.Empty;
                    }
                    
                }
                else
                {
                    IsBusy = false;
                    IsEmpty = true;
                    EmptyStateTitle = AlertMessages.GoOnline;
                }

            }

        }

        async private void OnItemSelectedCommandExecuted(object item)
        {
            //await NavigationService.NavigateAsync(PageName.Popup.UnitUpsertPopupPage);

            // var selectedProduct = item as FoundationVideoLectureDTO;
            //var navigationParams = new NavigationParameters
            //    {

            //        { "PageRequest",
            //            new BaseViewModels.PageRequest{
            //                LocalId = selectedProduct.LocalId,
            //                ServerId= selectedProduct.ServerId,
            //                Id = selectedProduct.LocalId
            //            }
            //        }
            //    };

            //await NavigationService.NavigateAsync(PageName.Products.UpdateProductPage, navigationParams);
        }
        private DelegateCommand<FoundationVideoLectureDTO> videoLinkTapCommand;

        public DelegateCommand<FoundationVideoLectureDTO> VideoLinkTapCommand =>
          videoLinkTapCommand ?? (videoLinkTapCommand = new DelegateCommand<FoundationVideoLectureDTO>(async (a) => await GetVideoLink(a)));

        private async Task GetVideoLink(FoundationVideoLectureDTO complaints)
        {
            var currnetDateTime = DateTime.Now;

            if (complaints.StartTime <= currnetDateTime)
            {
                NavigationParameters navigationParameter = new NavigationParameters();
                navigationParameter.Add("PlayVideoLecture", complaints);
                await NavigationService.NavigateAsync(PageName.VideoLecture.PlayFoundationVideoLecturePage, navigationParameter);
            }
            else
            {
                UserDialogsService.Toast(AlertMessages.CanNotPlayBeforStarttimeVideoAlert);
            }


        }

        protected async virtual void RaiseIsActiveChanged()
        {
            //GetAllVideoLectures();
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
            if (IsActive == true)
            {
                //await GetAllVideoLectures();

            }

        }

       
        #endregion

    }
}
