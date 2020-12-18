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
    public partial class EntryLayout : StackLayout
    {

        public static readonly BindableProperty TitleProperty =
       BindableProperty.Create(nameof(Title), typeof(string), typeof(EntryLayout), default(string), BindingMode.TwoWay);

        public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryLayout), default(string), BindingMode.TwoWay);

        public static readonly BindableProperty PlaceholderProperty =
      BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryLayout), default(string), BindingMode.TwoWay);      

        public static readonly BindableProperty KeyboardProperty =
          BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryLayout), Keyboard.Default);

        public static readonly BindableProperty IsReadOnlyProperty =
           BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(EntryLayout), false);

        public static readonly BindableProperty RequiredColorProperty =
            BindableProperty.Create(nameof(RequiredColor), typeof(Color), typeof(EntryLayout), Color.Transparent, BindingMode.TwoWay);

        public static readonly BindableProperty TextChangeCommandProperty =
           BindableProperty.Create(nameof(TextChangeCommand), typeof(ICommand), typeof(HkFileLayout), null, BindingMode.TwoWay);


        public ICommand TextChangeCommand
        {
            get { return (ICommand)GetValue(TextChangeCommandProperty); }
            set { SetValue(TextChangeCommandProperty, value); }
        }



        public event EventHandler<FocusEventArgs> EntryFocused;
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public Color RequiredColor
        {
            get { return (Color)GetValue(RequiredColorProperty); }
            set { SetValue(RequiredColorProperty, value); }
        }


        public EntryLayout()
        {
            InitializeComponent();
        }

        private void BorderlessEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangeCommand?.Execute(null);
        }
    }
}