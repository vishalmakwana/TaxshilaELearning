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

namespace TaxshilaMobile.ViewModels.VideoLecture
{
    public class TodayVideoLecturePageViewModel : BaseHariKrishnaViewModel, IActiveAware
    {

        #region Services
        private readonly IVideoLectureService _videoLectureService;
        private readonly IAppSubjectService subjectService;
        #endregion
        #region Properties

        private System.Collections.ObjectModel.ObservableCollection<GroupedDataList<VideoLectureDTO>> _groupedSubjectViseVideoData = new System.Collections.ObjectModel.ObservableCollection<GroupedDataList<VideoLectureDTO>>();
        public System.Collections.ObjectModel.ObservableCollection<GroupedDataList<VideoLectureDTO>> GroupedSubjectViseVideoData
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

        private VideoLectureDTO _selectedProduct = new VideoLectureDTO();
        public VideoLectureDTO SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        private ObservableCollection<VideoLectureDTO> _itemCollections = new ObservableCollection<VideoLectureDTO>();
        public ObservableCollection<VideoLectureDTO> ItemCollections
        {
            get { return _itemCollections; }
            set { SetProperty(ref _itemCollections, value); }
        }

        private ObservableCollection<VideoLectureDTO> _searchCollection = new ObservableCollection<VideoLectureDTO>();
        public ObservableCollection<VideoLectureDTO> SearchCollection
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
        public Paginator<VideoLectureDTO> Paginator { get; set; }

        private const int PageSize = 50;
        #endregion



        #region Command
        private DelegateCommand<object> itemSelected;
        public DelegateCommand<object> ItemSelectedCommand =>
            itemSelected ?? (itemSelected = new DelegateCommand<object>(OnItemSelectedCommandExecuted));
        private DelegateCommand<object> headerTapped;
        public DelegateCommand<object> HeaderTappedCommand => headerTapped ?? (headerTapped = new DelegateCommand<object>(OnHeaderTappedCommandExecuted));
        #endregion

        #region Constrictors
        public TodayVideoLecturePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IVideoLectureService videoLectureService, IAppSubjectService subjectService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _videoLectureService = videoLectureService;
            this.subjectService = subjectService;

        }
        #endregion

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


                SearchCollection = new ObservableCollection<VideoLectureDTO>();
                ItemCollections = new ObservableCollection<VideoLectureDTO>();
                if (IsConnected)
                {
                    IsBusy = true;
                    var AllItems = await _videoLectureService.GetAllVideoLectures();
                    var SubjectWiseGroupVideoLectures = await subjectService.GetSubjectVideos();
                    var TodayLectures = AllItems.Where(a => a.PublishDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy")).ToList();
                    var Group = TodayLectures.OrderBy(a => a.SubjectMasterId).GroupBy(a => a.SubjectMasterId);


                    foreach (var item in Group)
                    {
                        var subjectName = SubjectWiseGroupVideoLectures.Where(a => a.SubjectId == item.Key).FirstOrDefault().Subjectname;

                        var groupData = new GroupedDataList<VideoLectureDTO>(item.Key)
                        {
                            Title = subjectName,
                            ItemCount = item.ToList().Count
                        };
                        groupData.AddRange(item.ToList());
                        GroupedSubjectViseVideoData.Add(groupData);
                    }

                    SearchCollection.AddRange(TodayLectures);
                    ItemCollections.AddRange(TodayLectures);
                    IsBusy = false;
                    IsEmpty = !ItemCollections.AnyExtended();
                    IsBusy = false;
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

            // var selectedProduct = item as VideoLectureDTO;
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
        private DelegateCommand<VideoLectureDTO> videoLinkTapCommand;

        public DelegateCommand<VideoLectureDTO> VideoLinkTapCommand =>
          videoLinkTapCommand ?? (videoLinkTapCommand = new DelegateCommand<VideoLectureDTO>(async (a) => await GetVideoLink(a)));

        private async Task GetVideoLink(VideoLectureDTO complaints)
        {
            var currnetDateTime = DateTime.Now;

            if (complaints.StartTime <= currnetDateTime)
            {
                NavigationParameters navigationParameter = new NavigationParameters();
                navigationParameter.Add("PlayVideoLecture", complaints);
                await NavigationService.NavigateAsync(PageName.VideoLecture.PlayVideoLecturePage, navigationParameter);
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
                //GroupedSubjectViseVideoData = new ObservableCollection<GroupedDataList<VideoLectureDTO>>();
                //await GetAllVideoLectures();

            }

        }

        private void OnHeaderTappedCommandExecuted(object obj)
        {
            if (obj is GroupedDataList<VideoLectureDTO> selectedCase)
            {
                IsBusy = true;
                var SearchList = SearchCollection;
                //onlineList = new List<CaseThinViewModel>();
                int selectedIndex = GroupedSubjectViseVideoData.IndexOf(selectedCase);
                GroupedSubjectViseVideoData[selectedIndex].Expanded = !GroupedSubjectViseVideoData[selectedIndex].Expanded;

                if (GroupedSubjectViseVideoData[selectedIndex].Expanded)
                {
                    GroupedSubjectViseVideoData[selectedIndex].Clear();
                    var group = SearchCollection.Where(x => x.SubjectMasterId == GroupedSubjectViseVideoData[selectedIndex].TypeId).ToList();
                    GroupedSubjectViseVideoData[selectedIndex].AddRange(group);
                }
                else
                {
                    GroupedSubjectViseVideoData[selectedIndex].Clear();
                }
                IsBusy = false;
            }
        }
        #endregion
    }
}
