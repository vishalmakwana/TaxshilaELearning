using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models.Common;
using TaxshilaMobile.Models.Requests;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using TaxshilaMobile.Services.Interfaces;
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

namespace TaxshilaMobile.ViewModels.Products
{
    public class ProductsPageViewModel : BaseHariKrishnaViewModel
    {
        #region Services
        private readonly IProductService _productService;
        #endregion

        #region Properties

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


        private ProductThinViewModel _selectedProduct = new ProductThinViewModel();
        public ProductThinViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        private ObservableCollection<ProductThinViewModel> _productCollection = new ObservableCollection<ProductThinViewModel>();
        public ObservableCollection<ProductThinViewModel> ProductCollection
        {
            get { return _productCollection; }
            set { SetProperty(ref _productCollection, value); }
        }

        private ObservableCollection<ProductThinViewModel> _searchCollection = new ObservableCollection<ProductThinViewModel>();
        public ObservableCollection<ProductThinViewModel> SearchCollection
        {
            get { return _searchCollection; }
            set { SetProperty(ref _searchCollection, value); }
        }

        private ObservableCollection<int> _productIds = new ObservableCollection<int>();
        public ObservableCollection<int> ProductIds
        {
            get { return _productIds; }
            set { SetProperty(ref _productIds, value); }
        }
        public int CurrentPage { get; set; } = 1;
        public Paginator<ProductThinViewModel> Paginator { get; set; }

        private const int PageSize = 50;


        #endregion

        #region Command

        private DelegateCommand<object> itemSelected;
        public DelegateCommand<object> ItemSelectedCommand =>
            itemSelected ?? (itemSelected = new DelegateCommand<object>(OnItemSelectedCommandExecuted));


        private DelegateCommand<object> loadMore;
        public DelegateCommand<object> LoadMoreCommand => loadMore ?? (loadMore = new DelegateCommand<object>(OnLoadMoreCommandExecuted));

        private DelegateCommand getmoreServerCommand;
        public DelegateCommand GetMoreServerCommand => getmoreServerCommand ?? (getmoreServerCommand = new DelegateCommand(OnLoadGetMoreServerCommand));

        private DelegateCommand refreshCommand;
        public DelegateCommand RefreshCommand => refreshCommand ?? (refreshCommand = new DelegateCommand(async () =>
        {
            CurrentPage = 1;
            Paginator = new Paginator<ProductThinViewModel>(LoadNextPageAsync, pageSize: PageSize, loadingThreshold: 1f);
            await GetProducts();
            //await GetCategories();
        }));

        private DelegateCommand _addProductCommand;
        public DelegateCommand AddProductCommand => _addProductCommand ?? (_addProductCommand = new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync(PageName.Products.AddProductPage, animated: true);
        }));



        #endregion


        #region Constructor
        public ProductsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IProductService productService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _productService = productService;
        }
        #endregion

        #region Methods
        private async Task<PageResult<ProductThinViewModel>> LoadNextPageAsync(int pageNumber, int pageSize, bool v1)
        {
            try
            {
                return await _productService.GetProducts<ProductThinViewModel>(pageNumber, pageSize);
            }
            catch (Exception)
            {
                EmptyStateTitle = AlertMessages.EmptyState.ConnectivityTitle;
                EmptyStateSubtitle = AlertMessages.EmptyState.ConnectivitySubtitle;
                return PageResult<ProductThinViewModel>.Empty;

            }
        }

        async public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsUpToDate = false;
            SetSortOrder = new SortOrder();
            SetSortOrder.ColumnName = "Name";
            SetSortOrder.Title = "Name";
            SetSortOrder.SortTypes = SortTypes.Ascending;
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    {
                        if (parameters.TryGetValue("NeedRefresh", out int unitId))
                        {

                            if (unitId != 0)
                            {
                                var getlocalunitsbyid = _productService.GetLocalProductById<ProductThinViewModel>(unitId);

                                if (getlocalunitsbyid != null)
                                {
                                    if (getlocalunitsbyid.IsSaved)
                                    {
                                        CurrentPage = 1;
                                        Paginator = new Paginator<ProductThinViewModel>(LoadNextPageAsync, pageSize: PageSize, loadingThreshold: 1f);
                                        await GetProducts();
                                    }
                                }
                            }
                        }

                        break;
                    }
                default:
                    CurrentPage = 1;
                    Paginator = new Paginator<ProductThinViewModel>(LoadNextPageAsync, pageSize: PageSize, loadingThreshold: 1f);
                     await GetProducts();
                    break;
            }
        }

        private async Task GetProductsIds()
        {
            if (IsConnected && App.IsCallOnline)
            {
                var ids = await _productService.GetProductIds();
                ProductIds.AddRange(ids);
            }
            else
            {
                var ids = SearchCollection.Select(a => a.ServerId).ToList();
                ProductIds.AddRange(ids);
            }
        }

        async Task GetProducts()
        {

            IsBusy = true;
            SearchCollection = new ObservableCollection<ProductThinViewModel>();
            ProductCollection = new ObservableCollection<ProductThinViewModel>();


            // TO DO
            //Comment this code for live call 
            //Temporary comment for live call This is use for live server call
            await GetProductsIds();
            List<ProductThinViewModel> productList = new List<ProductThinViewModel>();
            productList = (await Paginator.LoadPage(CurrentPage)).Items.OrderByDescending(a => a.LocalId).ToList();

            ProductCollection.AddRange(productList);
            CurrentPage++;
            var AllUnits = await _productService.GetLocalProducts<ProductThinViewModel>();
            SearchCollection.AddRange(AllUnits);

            IsBusy = false;
            IsEmpty = !productList.AnyExtended();


        }
        private async void OnLoadGetMoreServerCommand()
        {
            if (IsUpToDate)
            {
                UserDialogsService.Alert("You're up to date!", "Alert", "Ok");
            }
            else
            {
                var existentUnits = SearchCollection.Where(x => x.ServerId > 0).Select(x => x.ServerId).ToList();
                var missingUnits = ProductIds.Where(m => existentUnits.All(e => e != m)).ToList();
                var itemIdsForApiRequest = missingUnits.Take(PageSize).ToList();

                var mobileRequest = new MobileRequest
                {
                    Username = _settings.CurrentUser.Username,
                    Ids = itemIdsForApiRequest
                };
                IsLoading = true;
                var newItems = await _productService.GetProductsByIds(mobileRequest);
                var productsTypeViewModels = newItems.Select(v => new ProductsMapper().MapToViewModel<ProductThinViewModel>(v)).ToList();
                IsLoading = false;
                SearchCollection.AddRange(productsTypeViewModels);
                ProductCollection.AddRange(productsTypeViewModels);
                existentUnits = SearchCollection.Where(x => x.ServerId > 0).Select(x => x.ServerId).ToList();
                missingUnits = ProductIds.Where(m => existentUnits.All(e => e != m)).ToList();
                IsUpToDate = !missingUnits.AnyExtended();
                IsLoading = false;
            }
        }

        async private void OnLoadMoreCommandExecuted(object obj)
        {


            var productlist = (await Paginator.LoadPage(CurrentPage, true)).Items.ToList();
            if (productlist.Count > 0)
            {
                CurrentPage++;
                ProductCollection.AddRange(productlist);
            }



        }
        async private void OnItemSelectedCommandExecuted(object item)
        {
            //await NavigationService.NavigateAsync(PageName.Popup.UnitUpsertPopupPage);

            var selectedProduct = item as ProductThinViewModel;  
            var navigationParams = new NavigationParameters
                {

                    { "PageRequest",
                        new BaseViewModels.PageRequest{
                            LocalId = selectedProduct.LocalId,
                            ServerId= selectedProduct.ServerId,
                            Id = selectedProduct.LocalId
                        }
                    }
                };

            await NavigationService.NavigateAsync(PageName.Products.UpdateProductPage, navigationParams);
        }

        #endregion
    }
}
