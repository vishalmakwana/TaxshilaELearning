using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.ViewModels.BaseViewModels;

namespace TaxshilaMobile.ViewModels
{
    public class IsReachableOrNotPageViewModel : BaseHariKrishnaViewModel
    {
        public IsReachableOrNotPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

        }
    }
}
