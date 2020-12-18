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
    public class StudyMatirealPageViewModel : BaseHariKrishnaViewModel, IActiveAware
    {
        #region services
        private readonly IStudyMaterialService studyMaterialService;
        #endregion
        #region Properties
        public event EventHandler IsActiveChanged;
        private bool _isActive;


        private ObservableCollection<StudyMaterialsDTO> _itemCollections = new ObservableCollection<StudyMaterialsDTO>();
        public ObservableCollection<StudyMaterialsDTO> ItemCollections
        {
            get { return _itemCollections; }
            set { SetProperty(ref _itemCollections, value); }
        }
        private ObservableCollection<StudyMaterialsDTO> _searchCollection = new ObservableCollection<StudyMaterialsDTO>();
        public ObservableCollection<StudyMaterialsDTO> SearchCollection
        {
            get { return _searchCollection; }
            set { SetProperty(ref _searchCollection, value); }
        }
        private StudyMaterialsDTO _selectedItem = new StudyMaterialsDTO();
        public StudyMaterialsDTO SelectedItem
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
        public Paginator<StudyMaterialsDTO> Paginator { get; set; }

        private const int PageSize = 50;

        #endregion

        #region Command
        private DelegateCommand<object> openFileCommand;
        public DelegateCommand<object> OpenFileCommand => openFileCommand ?? (openFileCommand = new DelegateCommand<object>((obj) => OnOpenFileCommandExecuted(obj)));

        private async void OnOpenFileCommandExecuted(object obj)
        {
            var StudentStudyMaterials = (StudyMaterialsDTO)obj;
            try
            {
                string filepath = await FileExtensions.DownloadFileandSaveInLocalfolderAsync(StudentStudyMaterials.StudyMaterialFileURL, StudentStudyMaterials.StudyMaterialFile);
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filepath)
                });
            }
            catch (Exception ex)
            {

            }
        }

        private DelegateCommand<object> readMoreCommand;
        public DelegateCommand<object> ReadMoreCommand => readMoreCommand ?? (readMoreCommand = new DelegateCommand<object>((obj) => OnReadMoreCommandExecuted(obj)));

        private async void OnReadMoreCommandExecuted(object obj)
        {
            var StudentStudyMaterials = (StudyMaterialsDTO)obj;
            try
            {
                NavigationParameters navigationparameter = new NavigationParameters();
                ShowMoreItem showMoreItem = new ShowMoreItem();
                showMoreItem.Description = StudentStudyMaterials.Descriptions;
                showMoreItem.IsSubjectVisible = true;

                showMoreItem.Subject = StudentStudyMaterials.SubjectName;
                navigationparameter.Add("ShowMoreItem", showMoreItem);
                await NavigationService.NavigateAsync(PageName.Popup.ReadMoreDescriptionPopupPage, navigationparameter);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        public StudyMatirealPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IStudyMaterialService studyMaterialService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            this.studyMaterialService = studyMaterialService;

        }

        #region Methods


        protected async Task GetStudentHomeWork()
        {
            IsUpToDate = false;
            SearchCollection = new ObservableCollection<StudyMaterialsDTO>();
            ItemCollections = new ObservableCollection<StudyMaterialsDTO>();

            if (IsConnected)
            {
                try
                {
                    IsBusy = true;
                    var AllItems = await studyMaterialService.GetStudentStudyMaterials();
                    AllItems.ForEach(a => a.StudyMaterialFileURL = _settings.ImageBaseUrl + a.StudyMaterialFile);
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
