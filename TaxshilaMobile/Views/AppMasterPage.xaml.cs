using Prism.Navigation;
using Xamarin.Forms;

namespace TaxshilaMobile.Views
{
    public partial class AppMasterPage : MasterDetailPage, IMasterDetailPageOptions
    {
        public AppMasterPage()
        {
            InitializeComponent();
        }
        public bool IsPresentedAfterNavigation { get { return false; } }

    }
}