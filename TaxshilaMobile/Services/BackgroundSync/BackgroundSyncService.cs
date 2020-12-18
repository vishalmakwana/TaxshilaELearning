using JdVyaparApp.DataTypesApp.Default;
using JdVyaparApp.Helpers;
using JdVyaparApp.ServiceBus.Services;
using JdVyaparApp.Services.Interfaces;
using Matcha.BackgroundService;
using Plugin.Connectivity;
using Prism.DryIoc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JdVyaparApp.Services.BackgroundSync
{
    public class BackgroundSyncService : IPeriodicTask
    {
        #region Properties
        public TimeSpan Interval { get; set; }
        public static CancellationTokenSource CancellationToken { get; set; }
        #endregion

        #region Services
        private readonly RestApiHelper _restApiHelper;
        private readonly IBaseUrl _baseUrl;
        private readonly IAppSettings _settings;
        private readonly ISyncService _syncService;
        private readonly IQueuedSyncService _queuedSyncService;



        #endregion
        #region Constructor
        public BackgroundSyncService(int minutes)
        {
            _syncService = (ISyncService)((PrismApplication)Xamarin.Forms.Application.Current).Container.Resolve(typeof(ISyncService));
            _queuedSyncService = (IQueuedSyncService)((PrismApplication)Xamarin.Forms.Application.Current).Container.Resolve(typeof(IQueuedSyncService));

            _baseUrl = Xamarin.Forms.DependencyService.Get<IBaseUrl>();
            _settings = Xamarin.Forms.DependencyService.Get<IAppSettings>();
            _restApiHelper = new RestApiHelper();

            Interval = TimeSpan.FromMinutes(minutes);
            CancellationToken = new CancellationTokenSource();
        }
        #endregion
        #region Job
        public async Task<bool> StartJob()
        {
            Debug.WriteLine("Started Sync");

            if (CrossConnectivity.Current.IsConnected && _settings.LoginStatus == LoginStateTypes.LoggedIn)
            {
                Task.Run(async () => RunInBackgroundThread(StartSync()));
            }

            return true; //return false when you want to stop or trigger only once
        }
        #endregion

        public async Task StartSync()
        {
            Debug.WriteLine("Starting Sync...");
            Debug.WriteLine("Sync begins...");
            try
            {
                CancellationToken?.Token.ThrowIfCancellationRequested();
                await _queuedSyncService.GetUnitQueuedSync(_syncService.GetStatus(SyncCategoryTypes.Units));
            }
            catch (Exception)
            {

                throw;
            }
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

    }
}
