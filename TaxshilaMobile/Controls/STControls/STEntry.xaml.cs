using Prism.DryIoc;
using Prism.Events;
using TaxshilaMobile.PrismEvents;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxshilaMobile.Controls.STControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class STEntry : Entry
    {
        private readonly IEventAggregator _eventAggregator;
        public STEntry()
        {
            InitializeComponent();
          
        }

       
    }
}