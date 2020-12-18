using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.Services;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Xamarin.Forms;

namespace TaxshilaMobile.ViewModels.FoundationClass
{
    public class PlayFoundationVideoLecturePageViewModel : BaseHariKrishnaViewModel
    {
        #region Services
        private readonly IVideoLectureService _videoLectureService;
        private readonly INavigationService NavigationServices;
        //public LibVLC _libvlc;
        #endregion

        private DelegateCommand itemSelected;
        public DelegateCommand ItemSelectedCommand =>
            itemSelected ?? (itemSelected = new DelegateCommand(OnItemSelectedCommandExecuted));
        private bool isForceLandscape = false;
        public bool IsForceLandscape
        {
            get { return isForceLandscape; }
            set { SetProperty(ref isForceLandscape, value); }
        }
        private void OnItemSelectedCommandExecuted()
        {
            if (IsForceLandscape)
            {
                Xamarin.Forms.DependencyService.Get<IOrientationHandler>().ForceLandscape();

            }
            else
            {
                Xamarin.Forms.DependencyService.Get<IOrientationHandler>().ForcePortrait();
            }
        }

        //private VideoSource _videoSource;
        //public VideoSource VideoSource
        //{
        //    get { return _videoSource; }
        //    set { SetProperty(ref _videoSource, value); }
        //}

        private HtmlWebViewSource _htmlWebSource;
        public HtmlWebViewSource HtmlWebSource
        {
            get { return _htmlWebSource; }
            set { SetProperty(ref _htmlWebSource, value); }
        }

        private UrlWebViewSource _urlWebSource;
        public UrlWebViewSource UrlWebSource
        {
            get { return _urlWebSource; }
            set { SetProperty(ref _urlWebSource, value); }
        }

        //private MediaPlayer _mediaPlayer;
        //public MediaPlayer MediaPlayer
        //{
        //    get { return _mediaPlayer; }
        //    set { SetProperty(ref _mediaPlayer, value); }
        //}

        private FoundationVideoLectureDTO _selectedItem = new FoundationVideoLectureDTO();
        public FoundationVideoLectureDTO SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public PlayFoundationVideoLecturePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IVideoLectureService videoLectureService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _videoLectureService = videoLectureService;
            SelectedItem = new FoundationVideoLectureDTO();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue("PlayVideoLecture", out FoundationVideoLectureDTO videoLectureDTO))
            {
                SelectedItem = videoLectureDTO;

                

                UrlWebSource = new UrlWebViewSource()
                {
                    Url = $"{SelectedItem.link1}"
                };
                
            }
            else
            {
                await NavigationService.GoBackAsync();
            }
        }
    }
}
