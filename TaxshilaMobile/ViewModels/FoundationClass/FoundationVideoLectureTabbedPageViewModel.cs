using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;

namespace TaxshilaMobile.ViewModels.FoundationClass
{
    public class FoundationVideoLectureTabbedPageViewModel : BaseHariKrishnaViewModel
    {
        public FoundationVideoLectureTabbedPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IVideoLectureService videoLectureService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {

        }
    }
}
