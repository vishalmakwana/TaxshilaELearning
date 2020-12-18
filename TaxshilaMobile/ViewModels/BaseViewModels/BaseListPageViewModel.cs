using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Sharpnado.Presentation.Forms.Paging;
using Sharpnado.Presentation.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile.ViewModels.BaseViewModels
{
    public abstract class BaseListPageViewModel<TEntity> : BaseHariKrishnaViewModel
    {
        private Models.PageRequest request = new Models.PageRequest();
        public Models.PageRequest Request
        {
            get { return request; }
            set { SetProperty(ref request, value); }
        }
        public int CurrentPage { get; set; } = 1;
        public Paginator<TEntity> Paginator { get; }
        protected const int PageSize = 50;
        public BaseListPageViewModel(
            //TService service,
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            IAppSettings settings,
            IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

            Paginator = new Paginator<TEntity>(LoadNextPageAsync, pageSize: PageSize, loadingThreshold: 1f, maxItemCount: 30000);
        }
        public BaseListPageViewModel() : base() { }
        protected virtual async Task<PageResult<TEntity>> LoadNextPageAsync(int pageNumber, int pageSize, bool v1 )
        {
            try
            {
                Request.PageIndex = pageNumber.ToString();
                Request.PageSize = pageSize.ToString();

                var items = await GetData();

                return items;
            }
            catch (Exception ex)
            {
                EmptyStateTitle = AlertMessages.EmptyState.ConnectivityTitle;
                EmptyStateSubtitle = AlertMessages.EmptyState.ConnectivitySubtitle;
                return PageResult<TEntity>.Empty;
            }
        }

        public abstract Task<PageResult<TEntity>> GetData();
    }
}
