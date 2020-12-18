using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Models;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
namespace TaxshilaMobile.ViewModels.BaseViewModels
{
    public abstract class BasePageViewModel<TEntity, TService> : BaseHariKrishnaViewModel
    {
        public BasePageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            IAppSettings settings,
            IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

        }

        public BasePageViewModel() : base() { }
        public TService DataService { get; set; }
        public abstract Task UpdateModel(TEntity model);
        public virtual async Task RefreshData()
        {
            //CurrentItem = GetData();
            //FromModel<TEntity>(GetData());
            try
            {
                CopyToViewModel(GetData());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error populating View Model - Exception: " + ex);
            }
        }

        public abstract TEntity GetData();
        public abstract void Update();

        private PageRequest _pageRequest;
        public PageRequest PageRequest
        {
            get => _pageRequest;
            set { SetProperty(ref _pageRequest, value); }
        }

        private NavigationPageRequest _navigationPageRequest;
        public NavigationPageRequest NavigationPageRequest
        {
            get => _navigationPageRequest;
            set { SetProperty(ref _navigationPageRequest, value); }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                base.OnNavigatedTo(parameters);

                await RefreshData();
            }
        }


    }
}
