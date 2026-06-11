using System;
using Microsoft.Maui.Controls;

namespace MvvmLogin.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();

		// תלמידים: כאן יש לקשר את ה-ViewModel של דף זה ל-BindingContext.
		// דוגמה למימוש:
		// BindingContext = new ViewModels.LoginPageViewModel();
		
		// דוגמה לטיפול בניווט מתוך ה-Code-Behind (אם לא משתמשים ב-Command ב-ViewModel):
		/*
		RegisterLinkGesture.Tapped += async (s, e) =>
		{
			await Shell.Current.GoToAsync("CreateAccountPage");
		};
		*/
	}
}