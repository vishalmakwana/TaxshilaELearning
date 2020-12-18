using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Models;
using TaxshilaMobile.ViewModels.BaseViewModels;

namespace TaxshilaMobile.ViewModels.Popups
{
    public partial class ReadMoreDescriptionPopupPageViewModel : BaseHariKrishnaViewModel
    {
        private ShowMoreItem showMoreItem;
        public ShowMoreItem ShowMoreItem
        {
            get { return showMoreItem; }
            set { SetProperty(ref showMoreItem, value); }
        }
        public ReadMoreDescriptionPopupPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

        }

        #region Commands
        private DelegateCommand close;
        public DelegateCommand CloseCommand => close ?? (close = new DelegateCommand(OnCloseCommandExecuted));

        #endregion

        #region Methods
        private async void OnCloseCommandExecuted()
        {
            await NavigationService.ClearPopupStackAsync();
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.TryGetValue("ShowMoreItem", out ShowMoreItem item))
            {
                ShowMoreItem = item;
            }
        }
        #endregion
    }
}
