using Acr.UserDialogs;
using TaxshilaMobile.Commonfiles;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.Models;
using TaxshilaMobile.Models.Common;
using TaxshilaMobile.PrismEvents;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.Exceptions;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using TaxshilaMobile.ViewModels.Base;
using MvvmHelpers;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.AppModel;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxshilaMobile.ViewModels.BaseViewModels
{
    public class BaseHariKrishnaViewModel : ExtendedBindableObject, INavigationAware, IDestructible, IPageLifecycleAware, IDisposable, IInitialize
    {
        #region Properties

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        private string loadingText = "Loading...";
        public bool IsTakingPhoto { get; set; }
        public string LoadingText
        {
            get { return loadingText; }
            set { SetProperty(ref loadingText, value); }
        }
        private bool isEmpty;
        public bool IsEmpty
        {
            get { return isEmpty; }
            set { SetProperty(ref isEmpty, value); }
        }

        private PickerItem pickerItem;
        public PickerItem DefaultPickerItem
        {
            get { return pickerItem; }
            set { SetProperty(ref pickerItem, value); }
        }
        private string _emptyStateTitle = AlertMessages.EmptyState.DefaultTitle;
        public string EmptyStateTitle
        {
            get { return _emptyStateTitle; }
            set { SetProperty(ref _emptyStateTitle, value); }
        }
        private SortOrder _setsortOrder = new SortOrder();

        public SortOrder SetSortOrder
        {
            get { return _setsortOrder; }
            set { SetProperty(ref _setsortOrder, value); }
        }

        private string _emptyStateSubtitle = AlertMessages.EmptyState.DefaultSubtitle;
        public string EmptyStateSubtitle
        {
            get { return _emptyStateSubtitle; }
            set { SetProperty(ref _emptyStateSubtitle, value); }
        }
        private string _toastText;
        public string ToastText
        {
            get => _toastText;
            set { SetProperty(ref _toastText, value); }
        }

        private DateTime _setcurrentDateTime = Functions.GetCurrentDatetime();
        public DateTime SetcurrentDateTime
        {
            get => _setcurrentDateTime;
            set { SetProperty(ref _setcurrentDateTime, value); }
        }
        protected bool IsConnected { get; private set; }
        protected bool IsReachable { get; private set; }
        public static CancellationTokenSource CancellationToken { get; set; }

        private ImageSource photoImage;
        public ImageSource PhotoImage
        {
            get { return photoImage; }
            set { SetProperty(ref photoImage, value); }
        }
        #endregion

        #region Services
        /// <summary>
        /// Navigation service
        /// </summary>
        protected INavigationService NavigationService { get; private set; }
        /// <summary>
        /// Page Dialog service
        /// </summary>
        protected IPageDialogService PageDialogService { get; set; }

        /// <summary>
        /// Call Platform Specific Call for Document Attachement
        /// </summary>
        protected ILocalFileProvider LocalFileProvider { get; set; }

        protected IUserDialogs UserDialogsService { get; private set; }

        protected IEventAggregator EventAggregator { get; private set; }

        private readonly IConnectivity Connectivity = CrossConnectivity.Current;
        protected readonly IAppSettings _settings;
        #endregion

        #region Constructor
        public BaseHariKrishnaViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator)
        {
            _settings = settings;
            EventAggregator = eventAggregator;
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            UserDialogsService = UserDialogs.Instance;
            IsConnected = Connectivity.IsConnected;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            CancellationToken = new CancellationTokenSource();
            //RegisterEvents();

        }

        public BaseHariKrishnaViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            UserDialogsService = UserDialogs.Instance;
            IsConnected = Connectivity.IsConnected;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            CancellationToken = new CancellationTokenSource();
            //RegisterEvents();

        }
        public BaseHariKrishnaViewModel() : base() { }

        #endregion

        #region Methods
        /// <summary>
        /// OnNavigatedFrom method
        /// </summary>
        /// <param name="parameters">Navigation parameters</param>
        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        /// <summary>
        /// OnNavigatedTo method
        /// </summary>
        /// <param name="parameters">Navigation parameters</param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            ToastText = "";
            OnPropertyChanged(nameof(ToastText));
        }

        /// <summary>
        /// OnNavigatingTo method
        /// </summary>
        /// <param name="parameters">Navigation parameters</param>
        public async virtual void Initialize(INavigationParameters parameters)
        {
            //await CheckLoggedInState();
        }
        protected virtual void RegisterEvents()
        {
            EventAggregator.GetEvent<SyncUpdateNotificationEvent>().Subscribe((a) => HandleSyncNotifications(a));
        }

        public void HandleSyncNotifications(SyncUpdatePayload notification)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ToastText = notification.Message;
                Debug.WriteLine(notification.Message);
            });
        }

        protected async virtual Task CheckLoggedInState()
        {
            if (_settings.LoginStatus == LoginStateTypes.LoggedIn)
                _settings.LoginStatus = _settings.TokenExpirationDate < DateTime.UtcNow ? LoginStateTypes.LoggedOut : LoginStateTypes.LoggedIn;

            switch (_settings.LoginStatus)
            {
                case LoginStateTypes.LoggedIn:
                    break;
                default:
                    await Logout();
                    break;
            }
        }
        /// <summary>
        /// Destroy method
        /// </summary>
        public virtual void Destroy() { }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        public virtual void Dispose() { }

        /// <summary>
        /// Class to manage when connectivity change
        /// </summary>
        /// <param name="sender">Sender who fires the event</param>
        /// <param name="e">Connectivity Changed Event Args </param>
        private async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.IsConnected;
            //_settings.IsOnline = IsConnected;

            await NoInterNetConnection();


        }

        public async Task RunInBackgroundThread(Task action)
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                try
                {
                    CancellationToken.Token.ThrowIfCancellationRequested();
                    await Task.Run(async () =>
                    {
                        if (!CancellationToken.Token.IsCancellationRequested)
                        {
                            await action;
                        }
                    }, CancellationToken.Token);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("RunInBackground error: " + ex.Message);
                }
            }
        }

        public async Task Logout()
        {
            _settings.ResetData();
            await NavigationService.NavigateAsync(new Uri($"/{PageName.NavigationPage}/{PageName.LoginProcess.LoginPage}", UriKind.Absolute));


            // await CheckLoggedInState();
        }

        public async Task NoInterNetConnection()
        {
            if (!IsConnected)
            {
                // UserDialogsService.Toast("Disconnected from the network!", TimeSpan.FromSeconds(2));
                await NavigationService.NavigateAsync(new Uri($"/{PageName.NavigationPage}/{PageName.NoInterNetConnectionPage}", UriKind.RelativeOrAbsolute), useModalNavigation: true);
            }
            else
            {
                await IsSiteIsReachable();
            }
            
        }
        public async Task<bool> IsSiteIsReachable()
        {
            if (IsConnected)
            {
               
                var isreachable = await CrossConnectivity.Current.IsReachable(_settings.BaseUrl, 5000);
                IsReachable = isreachable;

                return isreachable;
            }
            else
            {
                IsReachable = false;
                return false;
            }
            
        }


        public async Task<string> TakePhotoAsync(string fileName)
        {
            string filePath = string.Empty;

            if (!await Functions.IsMediaPermissionGrantedAsync())
            {
                await UserDialogsService.AlertAsync("No access permission. Please allow access.", "", "Ok");
                return string.Empty;
            }

            if (!IsTakingPhoto)
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await UserDialogsService.AlertAsync("No camera available.", "No Camera", "Ok");
                    return filePath;
                }

                IsTakingPhoto = true;
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Rear,
                    PhotoSize = PhotoSize.Medium
                });
                IsTakingPhoto = false;
                fileName = Path.GetFileNameWithoutExtension("TakePhoto_" + DateTime.UtcNow.ToString("MMddyyyy_hhmmss"));
                if (file != null)
                {
                    using (file)
                    {
                        using (Stream stream = file.GetStream())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                filePath = ms.ToArray().SaveAttachmentInLocalFolder(fileName, "jpg");
                                PhotoImage = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                            }
                        }
                    }
                }
            }

            return filePath;
        }

        public async Task<string> StoreAndGetFilePathAsync(byte[] FileBlob, string FileName)
        {
            string filePath = string.Empty;
            var fileName = Path.GetFileNameWithoutExtension(FileName);
            var extension = Path.GetExtension(FileName).Substring(1);
            using (Stream stream = new MemoryStream(FileBlob))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    filePath = ms.ToArray().SaveAttachmentInLocalFolder(fileName, extension);
                }
            }
            return filePath;
        }

        public async Task<string> PickPhotoFromGalleryAsync()
        {
            string filePath = string.Empty;
            if (!await Functions.IsMediaPermissionGrantedAsync())
            {
                await UserDialogsService.AlertAsync("No access permission. Please allow access.", "", "Ok");
                return filePath;
            }

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await UserDialogsService.AlertAsync("No Gallery available.", "No Gallery", "Ok");
                return filePath;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { PhotoSize = PhotoSize.Full });

            if (file != null)
            {
                var fileFullName = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                var fileName = Path.GetFileNameWithoutExtension("PickPhoto_" + DateTime.UtcNow.ToString("MMddyyyy_hhmmss"));
                var extension = Path.GetExtension(fileFullName).Substring(1);

                using (file)
                {
                    using (Stream stream = file.GetStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            filePath = ms.ToArray().SaveAttachmentInLocalFolder(fileName, extension);
                            PhotoImage = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                        }
                    }
                }
            }

            return filePath;
        }

        public async Task<string> PickVideoFromGalleryAsync()
        {
            string filePath = string.Empty;
            if (!await Functions.IsMediaPermissionGrantedAsync())
            {
                await UserDialogsService.AlertAsync("No access permission.", "", "Ok");
                return filePath;
            }

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await UserDialogsService.AlertAsync("No access permission. Please allow access.", "", "Ok");
                return filePath;
            }

            var file = await CrossMedia.Current.PickVideoAsync();

            if (file != null)
            {
                var fileFullName = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                var fileName = Path.GetFileNameWithoutExtension("PickVideo_" + DateTime.UtcNow.ToString("MMddyyyy_hhmmss"));
                var extension = Path.GetExtension(fileFullName).Substring(1);

                using (Stream stream = file.GetStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        filePath = ms.ToArray().SaveAttachmentInLocalFolder(fileName, extension);
                    }
                }
            }

            return filePath;
        }

        public async Task<string> PickFileAsync()
        {
            string filePath = string.Empty;
            if (!await Functions.IsMediaPermissionGrantedAsync())
            {
                await UserDialogsService.AlertAsync("No access permission. Please allow access.", "", "Ok");
                return filePath;
            }

            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                var fileFullName = file.FilePath.Substring(file.FilePath.LastIndexOf('\\') + 1);
                var extension = Path.GetExtension(file.FileName).Substring(1);
                var fileName = Path.GetFileNameWithoutExtension("Pick_" + extension + "_File_" + DateTime.UtcNow.ToString("MMddyyyy_hhmmss"));


                using (Stream stream = file.GetStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        filePath = ms.ToArray().SaveAttachmentInLocalFolder(fileName, extension);
                    }
                }
            }

            return filePath;
        }
        public async Task ShowPopup(List<PickerItem> items)
        {
            var navigationParams = new NavigationParameters
            {
                { "PickerItems", items }
            };

            await NavigationService.NavigateAsync(PageName.Popup.DefaultPickerPopup, navigationParams);
        }
        #endregion

        /// <summary>
        /// Create a ThinViewModel for the Model collection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public IEnumerable GetThinListForCollection(System.Collections.IEnumerable source, Type targetType)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(targetType);
            IList list = (IList)Activator.CreateInstance(constructedListType);
            if (source != null)
            {
                foreach (var item in source)
                {
                    var newObject = Activator.CreateInstance(targetType);

                    PopulateViewModel(newObject, item);

                    list.Add(newObject);
                }
            }

            return list;
        }

        public BaseThinViewModel GetThinViewModel(Type targetType, object model)
        {
            //try
            //{
            BaseThinViewModel newObject = Activator.CreateInstance(targetType) as BaseThinViewModel;


            PopulateViewModel(newObject, model);
            return newObject;
            //} catch ( Exception ex)
            //{

            //}
        }

        //   /// <summary>
        //   /// Create a ThinViewModel for the Model collection
        //   /// </summary>
        //   /// <param name="source"></param>
        //   /// <param name="targetType"></param>
        //   /// <returns></returns>
        //   public IEnumerable GetThinListForModelCollection(System.Collections.IEnumerable source, Type targetType)
        //   {
        //       var listType = typeof(List<>);
        //       var constructedListType = listType.MakeGenericType(targetType);
        //       IList list = (IList)Activator.CreateInstance(constructedListType);
        //       foreach (var item in source)
        //       {
        //           var newObject = Activator.CreateInstance(targetType);

        //           PopulateModel(item, newObject);

        //           list.Add(newObject);
        //       }
        //       return list;
        //   }

        /// <summary>
        /// Get all properties of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<PropertyInfo> DehydrateObject<T>(object obj)
        {
            List<PropertyInfo> values =
                (from property in obj.GetType().GetProperties()
                 where property.GetCustomAttributes(typeof(T), false).Length > 0
                 select property).ToList();

            return values;
        }

        /// <summary>
        /// Copies View Model data to current View Model
        /// </summary>
        /// <param name="valueObject"></param>
        public void CopyToViewModel(object valueObject)
        {
            var props = DehydrateObject<ModelPropertyAttribute>(this);
            try
            {
                foreach (PropertyInfo info in props)
                {
                    string propertyName = info.Name;
                    var currentVal = this.GetType().GetProperty(propertyName).GetValue(this);
                    var targetVal = valueObject?.GetType().GetProperty(propertyName)?.GetValue(valueObject);
                    //Debug.WriteLine("Property: " + propertyName);
                    if (currentVal != targetVal)
                    {
                        // if it's an observable collection
                        if (info.PropertyType.Name.Contains("ObservableCollection") || info.PropertyType.Name.Contains("ObservableRangeCollection"))
                        {
                            var genericArg = info.PropertyType.GenericTypeArguments.FirstOrDefault(); ;

                            // create ReplaceThimListForCollection
                            var list = GetThinListForCollection(targetVal as IEnumerable, genericArg);
                            info.PropertyType.GetMethod("ReplaceRange").Invoke(currentVal, new[] { targetVal });

                        }
                        else if (info.PropertyType.IsSubclassOf(typeof(BaseThinViewModel)))
                        {
                            // if type is a thin model
                            var item = GetThinViewModel(info.PropertyType, targetVal);
                            info.SetValue(this, item);
                        }
                        else // set object normally
                        {
                            info.SetValue(this, targetVal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error CopyToViewModel ex: " + ex);
            }
        }

        /// <summary>
        /// Upate the view model from a model
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="currentModel"></param>
        public void PopulateViewModel(object parentObject, object currentModel)
        {
            bool isModel = true;

            try
            {
                if (currentModel.GetType().IsSubclassOf(typeof(ModelBase)))
                {
                    isModel = true;
                }
                else
                {
                    isModel = false;
                }
            }
            catch (Exception ex)
            {

            }

            var props = DehydrateObject<ModelPropertyAttribute>(parentObject);
            foreach (PropertyInfo info in props)
            {
                var targetPropertyName = info.GetCustomAttribute(typeof(ModelPropertyAttribute)) as ModelPropertyAttribute;

                // if (info.Name == "Notes") Debugger.Break();// use this to debug a collection
                try
                {
                    var currentVal = info.GetValue(parentObject);
                    var targetVal =
                        currentModel
                            .GetType()
                                .GetProperty(isModel ? targetPropertyName.PropertyName : info.Name)
                                    .GetValue(currentModel);

                    if (currentVal != targetVal)
                    {

                        // if it's an observable collection
                        if (info.PropertyType.Name.Contains("ObservableCollection") || info.PropertyType.Name.Contains("ObservableRangeCollection"))
                        {
                            var genericArg = info.PropertyType.GenericTypeArguments.FirstOrDefault(); ;

                            var list = GetThinListForCollection(targetVal as IEnumerable, genericArg);
                            info.PropertyType.GetMethod("ReplaceRange").Invoke(currentVal, new[] { list });

                        }
                        else if (info.PropertyType.IsSubclassOf(typeof(BaseThinViewModel)))
                        {
                            // if type is a thin model
                            var item = GetThinViewModel(info.PropertyType, targetVal);
                            info.SetValue(parentObject, item);
                        }
                        else // set object normally
                        {
                            info.SetValue(parentObject, targetVal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ConvertFromModelToViewModelException(ex.Message, info.Name, targetPropertyName.PropertyName, null);
                }
            }
        }
    }
}
