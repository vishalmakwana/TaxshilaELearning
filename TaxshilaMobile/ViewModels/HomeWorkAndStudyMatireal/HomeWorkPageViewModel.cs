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

namespace TaxshilaMobile.ViewModels.HomeWorkAndStudyMatireal
{
    public class HomeWorkPageViewModel : BaseHariKrishnaViewModel, IActiveAware
    {
        #region services
        private readonly IHomeworkService homeworkService;
        #endregion

        #region Properties
        public event EventHandler IsActiveChanged;
        private bool _isActive;


        private ObservableCollection<HomeworkDTO> _itemCollections = new ObservableCollection<HomeworkDTO>();
        public ObservableCollection<HomeworkDTO> ItemCollections
        {
            get { return _itemCollections; }
            set { SetProperty(ref _itemCollections, value); }
        }
        private ObservableCollection<HomeworkDTO> _searchCollection = new ObservableCollection<HomeworkDTO>();
        public ObservableCollection<HomeworkDTO> SearchCollection
        {
            get { return _searchCollection; }
            set { SetProperty(ref _searchCollection, value); }
        }
        private HomeworkDTO _selectedItem = new HomeworkDTO();
        public HomeworkDTO SelectedItem
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
        public Paginator<HomeworkDTO> Paginator { get; set; }

        private const int PageSize = 50;

        #endregion

        #region Command
        private DelegateCommand<object> openFileCommand;
        public DelegateCommand<object> OpenFileCommand => openFileCommand ?? (openFileCommand = new DelegateCommand<object>((obj) => OnOpenFileCommandExecuted(obj)));

        private DelegateCommand<object> readMoreCommand;
        public DelegateCommand<object> ReadMoreCommand => readMoreCommand ?? (readMoreCommand = new DelegateCommand<object>((obj) => OnReadMoreCommandExecuted(obj)));

        private async void OnReadMoreCommandExecuted(object obj)
        {
            var StudentHomeWork = (HomeworkDTO)obj;
            try
            {
                NavigationParameters navigationparameter = new NavigationParameters();
                ShowMoreItem showMoreItem = new ShowMoreItem();
                showMoreItem.Description = StudentHomeWork.Descriptions;
                showMoreItem.IsSubjectVisible = true;
                showMoreItem.Subject = StudentHomeWork.SubjectName;
                navigationparameter.Add("ShowMoreItem", showMoreItem);
                await NavigationService.NavigateAsync(PageName.Popup.ReadMoreDescriptionPopupPage, navigationparameter);
            }
            catch (Exception ex)
            {

            }
        }

        private async void OnOpenFileCommandExecuted(object obj)
        {
            var StudentHomeWork = (HomeworkDTO)obj;
            try
            {
                string filepath = await FileExtensions.DownloadFileandSaveInLocalfolderAsync(StudentHomeWork.HomeWorkFileURL, StudentHomeWork.HomeWorkFile);
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

        public HomeWorkPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IHomeworkService homeworkService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            this.homeworkService = homeworkService;
        }


        #region Methods


        protected async Task GetStudentHomeWork()
        {
            IsUpToDate = false;
            SearchCollection = new ObservableCollection<HomeworkDTO>();
            ItemCollections = new ObservableCollection<HomeworkDTO>();

            if (IsConnected)
            {
                try
                {
                    IsBusy = true;
                    var AllItems = await homeworkService.GetStudentHomeWork();
                    AllItems.ForEach(a => a.HomeWorkFileURL = _settings.ImageBaseUrl + a.HomeWorkFile);
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
                EmptyStateTitle = AlertMessages.GoOnline;
            }

        }
        protected async virtual void RaiseIsActiveChanged()
        {
            //GetAllVideoLectures();
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
            if (IsActive == true)
            {
                await GetStudentHomeWork();
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

        #endregion
    }
}
