using TaxshilaMobile.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace TaxshilaMobile.Behaviors
{
    public class ToastPopupBehavior : Behavior<SyncToast>
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(ToastPopupBehavior), null, propertyChanged: OnNumberChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public SyncToast AssociatedObject
        {
            get;
            private set;
        }

        static void OnNumberChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (ToastPopupBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            var toaster = behavior.AssociatedObject as SyncToast;

            Action<double> callback = input => toaster.TranslateTo(toaster.X, 0);
            Action<double> callbackReturn = input => toaster.TranslateTo(toaster.X, 100);

            uint rate = 16;
            uint length = 5000;
            Easing easing = Easing.CubicOut;

            toaster.Text = behavior.Text;

            if (!string.IsNullOrWhiteSpace(toaster.Text))
            {
                toaster.Animate("show", new Animation(callback, 0, 1, easing, () => { }), rate, 500, easing, async (v, c) =>
                {
                    await Task.Delay(3000);
                    toaster.Animate("hide", new Animation(callbackReturn, 0, 1, easing));
                });
            }
            else
            {
                toaster.TranslateTo(toaster.X, toaster.Y + 100);
            }
        }

        protected override void OnAttachedTo(SyncToast bindable)
        {

            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(SyncToast bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
