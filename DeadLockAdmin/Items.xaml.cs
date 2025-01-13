using DeadLockAdmin.ViewModels;
namespace DeadLockAdmin;

public partial class Items : ContentPage
{
	public Items()
	{
		InitializeComponent();
        BindingContext = new ItemsViewModel();
    }
}