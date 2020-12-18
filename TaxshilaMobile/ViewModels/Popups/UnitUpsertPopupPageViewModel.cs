using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxshilaMobile.ViewModels.Popups
{
    public class UnitUpsertPopupPageViewModel : BaseHariKrishnaViewModel
    {


        private DelegateCommand closeCommand;
        public DelegateCommand CloseCommand =>
            closeCommand ?? (closeCommand = new DelegateCommand(OnCloseCommandExecuted));
        public UnitUpsertPopupPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

        }
        private async void OnCloseCommandExecuted()
        {
            await NavigationService.ClearPopupStackAsync();
        }
    }
}
