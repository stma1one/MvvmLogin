using System;
using Microsoft.Maui.Controls;
using MvvmLogin.ViewModels;

namespace MvvmLogin.Pages;

public partial class LoginPage : ContentPage
{

	public LoginPage()
	{
		InitializeComponent();

		// קישור ה-ViewModel ל-BindingContext של הדף הנוכחי
		// קישור זה מאפשר ל-XAML לבצע Binding למאפיינים ולפקודות המוגדרים ב-LoginPageViewModel
		BindingContext = new LoginPageViewModel();
	}
}