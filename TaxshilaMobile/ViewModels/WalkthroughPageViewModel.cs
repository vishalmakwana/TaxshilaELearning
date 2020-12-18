using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.ViewModels.BaseViewModels;

namespace TaxshilaMobile.ViewModels
{
    public class WalkthroughPageViewModel : BaseHariKrishnaViewModel
    {
        #region Properties

        private DelegateCommand onSkipCommandClick;

        public DelegateCommand OnSkipCommandClick =>
            onSkipCommandClick ?? (onSkipCommandClick = new DelegateCommand(async () => await ExecuteOnSkipCommandClickAsync()));

        private bool CanExecuteOnSkipCommandClick()
        {
            return true;
        }

        private List<string> cardimages;

        public List<string> CardImages
        {
            get { return cardimages; }
            set
            {
                cardimages = value;
                RaisePropertyChanged(() => cardimages);
            }
        }

        #endregion Properties

        #region Constructor

        public WalkthroughPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            CardImages = new List<string>()
            {
               "bg2.jpg", "bg2.jpg", "bg2.jpg"
            };
        }

        #endregion Constructor

        #region Methods

        private async Task ExecuteOnSkipCommandClickAsync()
        {
            if (_settings.LoginStatus == LoginStateTypes.LoggedIn)
            {
                await NavigationService.NavigateAsync(new Uri($"{PageName.AppMasterPage}/{PageName.NavigationPage}/{PageName.VideoLecture.VideoLectureListPage}",UriKind.RelativeOrAbsolute));

            }
            else
            {
                await NavigationService.NavigateAsync(new Uri($"{PageName.NavigationPage}/{PageName.LoginProcess.LoginPage}",UriKind.RelativeOrAbsolute));
            }
        }

        #endregion Methods
    }
}
