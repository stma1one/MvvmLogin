
using MvvmLogin.Models;
using MvvmLogin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmLogin.ViewModels
{
	public class LoginPageViewModel:ViewModelBase
	{
		private string error;
		private string userName;
		private string password;
		private bool isNotLoggedIn;
		private MockAuthService service;

		public ICommand LoginCommand
		{
			get; private set;
		}
		public string Error
		{
			get => error;
			set
			{
				error = value;
				OnPropertyChanged();
			}
		}
		public string UserName
		{
			get => userName;
			set
			{
				userName = value;
				OnPropertyChanged();
				((Command)LoginCommand).ChangeCanExecute();

			}
		}
		public string Password
		{
			get => password;
			set
			{
				password = value;
				OnPropertyChanged();
				((Command)LoginCommand).ChangeCanExecute();
			}
		}
		public bool IsNotLoggedIn
		{
			get => isNotLoggedIn;
			set
			{
				isNotLoggedIn = value;
				OnPropertyChanged();
			}
		}
		public LoginPageViewModel()
		{
			service = new MockAuthService();
			LoginCommand = new Command(Login,CanLogin);
		}

		private bool CanLogin()
		{
			return !string.IsNullOrEmpty(UserName)&&!string.IsNullOrEmpty(Password);
		}

		private void Login()
		{
			User u = new User()
			{
				Username = this.UserName,
				Password = this.Password
			};
			 IsNotLoggedIn = !service.Login(u);
			if (IsNotLoggedIn)
				Error = "שם משתמש או סיסמה לא תקינים";
			else
			{
				Error = string.Empty;
				//הקפצת חלון הודעה על הצלחה
				Shell.Current.DisplayAlert("הצלחה", "התחברת בהצלחה!", "אישור");
			}
				//ניקוי שדות שם משתמש וסיסמה
				UserName = string.Empty;
				Password = string.Empty;

		}
	
	}
}
