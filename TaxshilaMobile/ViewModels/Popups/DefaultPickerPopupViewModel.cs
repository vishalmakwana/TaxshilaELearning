using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Sharpnado.Presentation.Forms.Paging;
using Sharpnado.Presentation.Forms.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TaxshilaMobile.ViewModels.Popups
{
    public class DefaultPickerPopupViewModel : BaseHariKrishnaViewModel
    {
        #region Properties
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }

        private PickerItem selectedItem;
        public PickerItem SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }

        private ObservableCollection<PickerItem> items = new ObservableCollection<PickerItem>();
        public ObservableCollection<PickerItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        private ObservableCollection<PickerItem> searchableList = new ObservableCollection<PickerItem>();
        public ObservableCollection<PickerItem> SearchableList
        {
            get { return searchableList; }
            set { SetProperty(ref searchableList, value); }
        }

        //public ObservableCollection<PickerItemGroup> GroupedList { get; private set; } = new ObservableCollection<PickerItemGroup>();

        private double layoutWidth = 350;
        public double LayoutWidth
        {
            get { return layoutWidth; }
            set { SetProperty(ref layoutWidth, value); }
        }

        public int CurrentPage { get; set; } = 1;
        public Paginator<PickerItem> Paginator { get; }
        private const int PageSize = 50;
        #endregion

        #region Services
        #endregion

        #region Commands
        private DelegateCommand close;
        public DelegateCommand CloseCommand => close ?? (close = new DelegateCommand(OnCloseCommandExecuted));

        private DelegateCommand<object> itemSelected;
        public DelegateCommand<object> ItemSelectedCommand =>
            itemSelected ?? (itemSelected = new DelegateCommand<object>(OnItemSelectedCommandExecuted));

        private DelegateCommand<object> loadMore;
        public DelegateCommand<object> LoadMoreCommand => loadMore ?? (loadMore = new DelegateCommand<object>(OnLoadMoreCommandExecuted));

        private DelegateCommand getMoreFromServer;
        public DelegateCommand GetMoreFromServerCommand => getMoreFromServer ?? (getMoreFromServer = new DelegateCommand(GetMoreFromServerCommandExecuted));
        #endregion

        #region Variables

        #endregion

        #region Constructor
        public DefaultPickerPopupViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            Paginator = new Paginator<PickerItem>(LoadNextPageAsync, pageSize: PageSize, loadingThreshold: 1f, maxItemCount: 30000);
        }
        #endregion

        #region Methods
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue("PickerItems", out List<PickerItem> items))
            {
                SearchableList.ReplaceRange(items);

                var itemsList = (await Paginator.LoadPage(CurrentPage)).Items.ToList();
                CurrentPage++;
                Items.AddRange(itemsList);

                if (Device.RuntimePlatform != Device.UWP)
                {
                    var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                    LayoutWidth = (mainDisplayInfo.Width / mainDisplayInfo.Density) - 64;
                }
            }
        }

        private async Task<PageResult<PickerItem>> LoadNextPageAsync(int pageNumber, int pageSize,bool v1)
        {
            return new PageResult<PickerItem>(SearchableList.Count, SearchableList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
        }

        private async void OnItemSelectedCommandExecuted(object obj)
        {
            if (obj != null)
            {
                SelectedItem = obj as PickerItem;
                var parameters = new NavigationParameters
                {
                    { "SelectedItem", SelectedItem }
                };
                await NavigationService.ClearPopupStackAsync(parameters);
            }
        }

        private async void OnLoadMoreCommandExecuted(object obj)
        {
            var items = (await Paginator.LoadPage(CurrentPage)).Items.ToList();
            CurrentPage++;

            if (items.Any())
            {
                Items.AddRange(items);
            }
        }

        private async void GetMoreFromServerCommandExecuted()
        {

        }

        private async void OnCloseCommandExecuted()
        {
            await NavigationService.ClearPopupStackAsync();
        }
        #endregion
    }
}
