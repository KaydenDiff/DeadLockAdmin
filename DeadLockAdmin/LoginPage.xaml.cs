namespace DeadLockAdmin;
public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new DeadLockAdmin.ViewModels.LoginViewModel();

    }
}