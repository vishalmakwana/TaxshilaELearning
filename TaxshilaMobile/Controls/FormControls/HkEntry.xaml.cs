using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxshilaMobile.Controls.FormControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HkEntry : StackLayout
    {

        public static readonly BindableProperty EnteryTitleProperty =
    BindableProperty.Create(nameof(Title), typeof(string), typeof(EntryLayout), default(string), BindingMode.TwoWay);

        public static readonly BindableProperty EnteryIconProperty =
        BindableProperty.Create(nameof(EnteryIcon), typeof(string), typeof(HkEntry), default(string), BindingMode.TwoWay);

        
        public static readonly BindableProperty EnteryPlaceHolderProperty =
       BindableProperty.Create(nameof(EnteryPlaceholder), typeof(string), typeof(HkEntry), default(string), BindingMode.TwoWay);

        public static readonly BindableProperty KeyboardProperty =
           BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryLayout), Keyboard.Default);

        public static readonly BindableProperty EnteryTextProperty =
       BindableProperty.Create(nameof(EnteryPlaceholder), typeof(string), typeof(HkEntry), default(string), BindingMode.TwoWay);
        
        public static readonly BindableProperty ValidateEntryCommandProperty =
           BindableProperty.Create(nameof(ValidateEntryCommand), typeof(ICommand), typeof(HkEntry), null, BindingMode.TwoWay);
       
        public static readonly BindableProperty EnteryKeyboardProperty =
       BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryLayout), Keyboard.Default);

        public static readonly BindableProperty RequiredColorProperty =
           BindableProperty.Create(nameof(RequiredColor), typeof(Color), typeof(EntryLayout), Color.Transparent, BindingMode.TwoWay);
        public string Title
        {
            get { return (string)GetValue(EnteryTitleProperty); }
            set { SetValue(EnteryTitleProperty, value); }
        }
        public Color RequiredColor
        {
            get { return (Color)GetValue(RequiredColorProperty); }
            set { SetValue(RequiredColorProperty, value); }
        }
        public string EnteryIcon
        {
            get { return (string)GetValue(EnteryIconProperty); }
            set { SetValue(EnteryIconProperty, value); }
        }

        public string EnteryPlaceholder
        {
            get { return (string)GetValue(EnteryPlaceHolderProperty); }
            set { SetValue(EnteryPlaceHolderProperty, value); }
        }

        public string EnteryText
        {
            get { return (string)GetValue(EnteryTextProperty); }
            set { SetValue(EnteryTextProperty, value); }
        }

        public ICommand ValidateEntryCommand
        {
            get { return (ICommand)GetValue(ValidateEntryCommandProperty); }
            set { SetValue(ValidateEntryCommandProperty, value); }
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }
        public HkEntry()
        {
            InitializeComponent();
        }

        private void BorderlessEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateEntryCommand?.Execute(null);
        }
    }
}