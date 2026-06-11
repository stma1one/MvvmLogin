using System;
using Microsoft.Maui.Controls;

namespace MvvmLogin.Pages;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage()
	{
		InitializeComponent();

		// תלמידים: כאן יש לקשר את ה-ViewModel של דף זה ל-BindingContext.
		// דוגמה למימוש:
		// BindingContext = new ViewModels.CreateAccountPageViewModel();
		
		// דוגמה לטיפול בניווט מתוך ה-Code-Behind (אם לא משתמשים ב-Command ב-ViewModel):
		/*
		LoginLinkGesture.Tapped += async (s, e) =>
		{
			await Shell.Current.GoToAsync(".."); // חוזר חזרה לעמוד הקודם (LoginPage)
		};
		*/
	}
}