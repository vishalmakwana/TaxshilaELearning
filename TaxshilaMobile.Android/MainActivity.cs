using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
//using LibVLCSharp.Forms.Shared;
//using MediaManager;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup;
using System.Threading.Tasks;
using TaxshilaMobile.Droid.PlatformSpecifics;
using TaxshilaMobile.ServiceBus.Services;
using TaxshilaMobile.Styles;

namespace TaxshilaMobile.Droid
{
    [Activity(Label = "Taxshila", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Current { private set; get; }

        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
          
            UserDialogs.Init(this);

            Popup.Init(this, savedInstanceState);
            //LibVLCSharpFormsRenderer.Init();
           
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            //Xam.Forms.VideoPlayer.Android.VideoPlayerRenderer.Init();
            //CrossMediaManager.Current.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
           
           
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageViewHandler();
            LoadApplication(new App(new AndroidInitializer()));
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    // Set the filename as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        void SetAppTheme()
        {
            if (Resources.Configuration.UiMode.HasFlag(UiMode.NightYes))
                SetTheme(TaxshilaMobile.Theme.Dark);
            else
                SetTheme(TaxshilaMobile.Theme.Light);
        }
        void SetTheme(Theme mode)
        {
            if (mode == TaxshilaMobile.Theme.Dark)
            {
                if (App.AppTheme == TaxshilaMobile.Theme.Dark)
                    return;
                // App.Current.Resources = new DarkTheme();
                App.Current.Resources = new LightTheme();
            }
            else
            {
                if (App.AppTheme != TaxshilaMobile.Theme.Dark)
                    return;
                App.Current.Resources = new LightTheme();
            }
            App.AppTheme = mode;
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<IBaseUrl, BaseUrl>();
            containerRegistry.Register<ILocalFileProvider, LocalFileProvider>();
        }
    }
}

