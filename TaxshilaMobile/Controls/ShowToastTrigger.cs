using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxshilaMobile.Controls
{
    public class ShowToastTrigger : TriggerAction<View>
    {
        public string Text { get; set; }
        private bool _isTemporary = true;
        public bool IsTemporary
        {
            get => _isTemporary;
            set { _isTemporary = value; }
        }
        private bool _isVisible = false;
        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; }
        }
        protected override void Invoke(View sender)
        {
            if (IsTemporary)
            {
                var toaster = sender as SyncToast;
                Action<double> callback = input => toaster.TranslateTo(toaster.X, 0);
                Action<double> callbackReturn = input => toaster.TranslateTo(toaster.X, 100);

                uint rate = 16;
                uint length = 5000;
                Easing easing = Easing.CubicOut;

                toaster.Text = Text;

                toaster.Animate("show", new Animation(callback, 0, 1, easing, () => { }), rate, 500, easing, async (v, c) =>
                {
                    await Task.Delay(3000);
                    toaster.Animate("hide", new Animation(callbackReturn, 0, 1, easing));
                });
            }

        }
    }
}
