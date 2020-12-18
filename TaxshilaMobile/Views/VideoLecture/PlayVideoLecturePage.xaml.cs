//using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using TaxshilaMobile.ServiceBus.Services;
using TaxshilaMobile.ViewModels.VideoLecture;
//using Xam.Forms.VideoPlayer;
using Xamarin.Forms;

namespace TaxshilaMobile.Views.VideoLecture
{
   
    [DesignTimeVisible(false)]
    public partial class PlayVideoLecturePage : ContentPage
    {
        //public LibVLC _libvlc;
        public PlayVideoLecturePageViewModel PlayVideoLecturePageViewModel;
        public PlayVideoLecturePage()
        {
            InitializeComponent();
            //PlayVideoLecturePageViewModel=(PlayVideoLecturePageViewModel)BindingContext;
           
            //_libvlc = new LibVLC();
            //var Media = new Media(_libvlc, $"https://taxshila.harikrishnainfotech.com/Videos/{PlayVideoLecturePageViewModel.SelectedItem.link1}", FromType.FromLocation);

            //MyVideo.MediaPlayer = new MediaPlayer(Media) { EnableHardwareDecoding = true };
            //MyVideo.MediaPlayer.Play();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //var OnPotrate = DependencyService.Get<IOrientationHandler>();
           // OnPotrate.ForceLandscape();
            if (Device.RuntimePlatform == Device.Android)
            {
                //NavigationPage.SetHasNavigationBar(this, false);
               // DependencyService.Get<IStatusBar>().HideStatusBar();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //MyVideo.MediaPlayer.Stop();
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    //NavigationPage.SetHasNavigationBar(this, true);
            //    DependencyService.Get<IStatusBar>().ShowStatusBar();
            //}
            //videoPlayer.Stop();

        }
        private void VideoPlayer_BufferingStart(object sender, EventArgs e)
        {

        }

        private void VideoPlayer_BufferingEnd(object sender, EventArgs e)
        {

        }

        private void videoPlayer_PlayCompletion(object sender, System.EventArgs e)
        {

        }

        //private void videoPlayer_PlayError(object sender, Xam.Forms.VideoPlayer.VideoPlayer.PlayErrorEventArgs e)
        //{

        //}
    }
}
