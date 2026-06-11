using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLogin.Models
{
	public partial class User : ObservableObject
	{
		private string email;
		private string profileImageUrl;


		private string _username;
		private string _password;

		public string Username
		{
			get => _username;
			set
			{
				_username = value;
				OnPropertyChanged();
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				OnPropertyChanged();
			}
		}
		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
				OnPropertyChanged();
			}
		}
		public string ProfileImageUrl
		{
			get
			{
				return profileImageUrl;
			}
			set
			{
				profileImageUrl = value;
				OnPropertyChanged();
			}
		}

	}
}