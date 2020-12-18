using MvvmHelpers;
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
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Xamarin.Essentials;

namespace TaxshilaMobile.ViewModels
{
    public class TeacherDetailPageViewModel : BaseHariKrishnaViewModel
    {
        #region Services
        private readonly IAccountService _accountService;
        #endregion

        #region Properties
        private ObservableRangeCollection<UserInfoDTO> _itemCollection=new ObservableRangeCollection<UserInfoDTO>();

        public ObservableRangeCollection<UserInfoDTO> ItemCollection
        {
            get { return _itemCollection; }
            set { _itemCollection = value; }
        }

        #endregion

        #region Command
        private DelegateCommand<UserInfoDTO> collectionViewTapCommand;
        public DelegateCommand<UserInfoDTO> CollectionViewTapCommand =>
       collectionViewTapCommand ?? (collectionViewTapCommand = new DelegateCommand<UserInfoDTO>(async (a) => await CollectionViewTapCommandExecute(a)));

        private async Task CollectionViewTapCommandExecute(UserInfoDTO a)
        {
            try
            {
                PhoneDialer.Open(a.PhoneNumber);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        #endregion
        public TeacherDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IAccountService accountService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _accountService = accountService;
            ItemCollection = new ObservableRangeCollection<UserInfoDTO>();
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            
            base.OnNavigatedTo(parameters);
            try
            {
                IsBusy = true;

                var TeacherInfo = await _accountService.AllTeacher();
              
                ItemCollection.ReplaceRange(TeacherInfo);
                IsBusy = false;
                IsEmpty = !ItemCollection.AnyExtended();
            }
            catch (Exception)
            {
                IsBusy = false;
                IsEmpty = true;
                throw;
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsBusy = true;
            IsBusy = false;

        }
    }
}
