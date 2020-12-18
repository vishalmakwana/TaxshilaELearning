//using LibVLCSharp.Shared;
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
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.Services;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.ViewModels.BaseViewModels;
//using Xam.Forms.VideoPlayer;
using Xamarin.Forms;

namespace TaxshilaMobile.ViewModels.VideoLecture
{
    public class PlayVideoLecturePageViewModel : BaseHariKrishnaViewModel
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

        private VideoLectureDTO _selectedItem = new VideoLectureDTO();
        public VideoLectureDTO SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public PlayVideoLecturePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IVideoLectureService videoLectureService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _videoLectureService = videoLectureService;
            SelectedItem = new VideoLectureDTO();
        }
       
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue("PlayVideoLecture", out VideoLectureDTO videoLectureDTO))
            {
                SelectedItem = videoLectureDTO;

                //SelectedItem.link1 = $"https://taxshila.harikrishnainfotech.com/VideoLecture/PlayVideo/{SelectedItem.VideoLectureId}";
                ////SelectedItem.link1 = $"https://www.youtube.com/watch?v=9vwJ1kLgmM8&feature=youtu.be";
                //string url = @"<html><body><video width='100%' height='100%' controls><source src='" + SelectedItem.link1 + "' type='video/mp4'></video></body></html>";

                UrlWebSource = new UrlWebViewSource()
                {
                    Url = $"{SelectedItem.link1}"
                };
                //string url = @"<html ><body><iframe width='420' height='315' src='" + SelectedItem.link1 + "' ></iframe></body></html>";
                //HtmlWebSource = new HtmlWebViewSource()
                //{
                //    Html = url
                //};
            }
            else
            {
                await NavigationService.GoBackAsync();
            }
        }



        public async override void OnAppearing()
        {

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    UriVideoSource uriVideoSurce = new UriVideoSource()
            //    {
            //        Uri = $"https://taxshila.harikrishnainfotech.com/Videos/{SelectedItem.link1}"
            //    };
            //    VideoSource = uriVideoSurce;
            //});




            base.OnAppearing();

        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            //MediaPlayer.Stop();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            //Device.BeginInvokeOnMainThread(()=> {
            //    _libvlc = new LibVLC();
            //    //var Media = new Media(_libvlc, $"https://taxshila.harikrishnainfotech.com/Videos/{PlayVideoLecturePageViewModel.SelectedItem.link1}", FromType.FromLocation);
            //    Core.Initialize();
            //    //MyVideo.MediaPlayer = new MediaPlayer(Media) { EnableHardwareDecoding = true };
            //    //MyVideo.MediaPlayer.Play();
            //    if (parameters.TryGetValue("PlayVideoLecture", out VideoLectureDTO videoLectureDTO))
            //    {
            //        SelectedItem = videoLectureDTO;
            //        var Media = new Media(_libvlc, $"https://taxshila.harikrishnainfotech.com/Videos/{videoLectureDTO.link1}", FromType.FromLocation);
            //        MediaPlayer = new MediaPlayer(Media) { EnableHardwareDecoding = true };
            //        MediaPlayer.Play();
            //    }

            //});

        }


    }
}
