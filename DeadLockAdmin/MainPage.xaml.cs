using DeadLockAdmin.ViewModels;
namespace DeadLockAdmin
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ItemsViewModel();

        }
    }

}
