using System;
using Microsoft.Maui.Controls;
using MvvmLogin.ViewModels;

namespace MvvmLogin.Pages;

public partial class LoginPage : ContentPage
{

	public LoginPage()
	{
		InitializeComponent();

		// תלמידים: כאן יש לקשר את ה-ViewModel של דף זה ל-BindingContext.
		// דוגמה למימוש:
		// BindingContext = new ViewModels.LoginPageViewModel();

		BindingContext = new LoginPageViewModel();
	}
}