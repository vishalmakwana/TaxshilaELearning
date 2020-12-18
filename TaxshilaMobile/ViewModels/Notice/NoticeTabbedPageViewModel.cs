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

namespace TaxshilaMobile.ViewModels.Notice
{
    public class NoticeTabbedPageViewModel : BaseHariKrishnaViewModel
    {

        #region Service
        private readonly INoticeService noticeService;
        #endregion


        #region Constrictor
        public NoticeTabbedPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, INoticeService noticeService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            this.noticeService = noticeService;
        }

        #endregion


    }
}
