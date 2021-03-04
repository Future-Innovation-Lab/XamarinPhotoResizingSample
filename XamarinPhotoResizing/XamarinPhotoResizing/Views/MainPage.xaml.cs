using Xamarin.Forms;
using XamarinPhotoResizing.ViewModels;

namespace XamarinPhotoResizing
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel(Navigation);
        }
    }
}
