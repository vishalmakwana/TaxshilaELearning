using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxshilaMobile.Controls
{

    public partial class ExtendedNavigationPage : Plugin.SharedTransitions.SharedTransitionNavigationPage
    {
        public ExtendedNavigationPage(Page page) : base(page)
        {
            InitializeComponent();
        }

        public bool IgnoreLayoutChange { get; set; } = false;

        protected override void OnSizeAllocated(double width, double height)
        {
            if (!IgnoreLayoutChange)
                base.OnSizeAllocated(width, height);
        }
    }
}