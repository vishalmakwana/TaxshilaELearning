using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxshilaMobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncToast : ContentView
    {
        public SyncToast()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
         propertyName: "Text",
         returnType: typeof(string),
         declaringType: typeof(SyncToast),
         defaultValue: default(string),
         propertyChanged: TextPropertyChanged
         );
        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SyncToast)bindable;
            if (control != null)
            {
                control.DescriptionText.Text = (newValue ?? String.Empty).ToString();
            }
        }
        public string Text
        {
            get { return DescriptionText.Text; }
            set { DescriptionText.Text = value; }
        }
    }
}