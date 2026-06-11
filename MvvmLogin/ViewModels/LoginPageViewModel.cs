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
    /// <summary>
    /// ה-ViewModel של דף ההתחברות.
    /// יורש מ-ViewModelBase המאפשר עדכון דינמי של ממשק המשתמש (UI) בעזרת מנגנון PropertyChanged.
    /// </summary>
	public class LoginPageViewModel : ViewModelBase
	{
		// שדות פרטיים השומרים את המצב הפנימי של התצוגה
		private string error;
		private string userName;
		private string password;
		private bool isNotLoggedIn;
		private MockAuthService service;

		/// <summary>
		/// פקודה המקושרת לכפתור ההתחברות ב-View.
		/// </summary>
		public ICommand LoginCommand
		{
			get; private set;
		}

		/// <summary>
		/// הודעת השגיאה המוצגת למשתמש במקרה של כישלון בהתחברות.
		/// </summary>
		public string Error
		{
			get => error;
			set
			{
				error = value;
				OnPropertyChanged(); // עדכון ה-UI על שינוי המאפיין
			}
		}

		/// <summary>
		/// שם המשתמש שהוזן על ידי המשתמש ב-Entry.
		/// </summary>
		public string UserName
		{
			get => userName;
			set
			{
				userName = value;
				OnPropertyChanged(); // עדכון ה-UI על שינוי שם המשתמש
				
				// הודעה לפקודת ההתחברות לבדוק מחדש האם ניתן לבצע את הפעולה (CanLogin)
				((Command)LoginCommand).ChangeCanExecute();
			}
		}

		/// <summary>
		/// הסיסמה שהוזנה על ידי המשתמש ב-Entry.
		/// </summary>
		public string Password
		{
			get => password;
			set
			{
				password = value;
				OnPropertyChanged(); // עדכון ה-UI על שינוי הסיסמה
				
				// הודעה לפקודת ההתחברות לבדוק מחדש האם ניתן לבצע את הפעולה (CanLogin)
				((Command)LoginCommand).ChangeCanExecute();
			}
		}

		/// <summary>
		/// מאפיין בוליאני המציין האם המשתמש אינו מחובר (נשמר כ-true כאשר יש שגיאה).
		/// מקושר למאפיין IsVisible של תיבת השגיאה ב-XAML.
		/// </summary>
		public bool IsNotLoggedIn
		{
			get => isNotLoggedIn;
			set
			{
				isNotLoggedIn = value;
				OnPropertyChanged(); // עדכון ה-UI על שינוי הראות של השגיאה
			}
		}

		/// <summary>
		/// בנאי ברירת מחדל של ה-ViewModel.
		/// מאתחל את שירות האימות ומגדיר את פקודת ההתחברות יחד עם התנאי להפעלתה.
		/// </summary>
		public LoginPageViewModel()
		{
			service = new MockAuthService();
			
			// אתחול ה-Command עם הפונקציה לביצוע (Login) והפונקציה לבדיקת זמינות הלחיצה (CanLogin)
			LoginCommand = new Command(Login, CanLogin);
		}

		/// <summary>
		/// תנאי הקובע האם כפתור ההתחברות יהיה פעיל (Enabled).
		/// הכפתור יהיה פעיל רק כאשר גם שם המשתמש וגם הסיסמה אינם ריקים.
		/// </summary>
		/// <returns>True אם השדות מלאים, אחרת False</returns>
		private bool CanLogin()
		{
			return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
		}

		/// <summary>
		/// מתודה המבוצעת כאשר המשתמש לוחץ על כפתור ההתחברות.
		/// פונה לשירות המדומה לאימות הפרטים ומעדכנת את ממשק המשתמש בהתאם לתוצאה.
		/// </summary>
		private void Login()
		{
			// יצירת אובייקט משתמש עם הפרטים שהוקלדו במסך
			User u = new User()
			{
				Username = this.UserName,
				Password = this.Password
			};
			
			// קריאה לשירות האימות ועדכון משתנה השגיאה במידה וההתחברות נכשלה
			IsNotLoggedIn = !service.Login(u);
			
			if (IsNotLoggedIn)
			{
				Error = "שם משתמש או סיסמה לא תקינים";
			}
			else
			{
				Error = string.Empty;
				// הקפצת חלון הודעה על הצלחה בחיבור
				Shell.Current.DisplayAlert("הצלחה", "התחברת בהצלחה!", "אישור");
			}
			
			// ניקוי שדות הקלט לאחר ניסיון ההתחברות
			UserName = string.Empty;
			Password = string.Empty;
		}
	}
}
