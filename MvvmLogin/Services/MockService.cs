using System;
using System.Collections.Generic;
using System.Linq;
using MvvmLogin.Models;

namespace MvvmLogin.Services
{
    // שירות אימות מדומה (Mock Auth Service) המשמש לתרגול למידה של MVVM
    // השירות משתמש ברשימת משתמשים סטטית השמורה בזיכרון
    public class MockAuthService
    {
        // רשימה סטטית של משתמשים במערכת
        private static readonly List<User> _users = new()
        {
            new User 
            { 
                Username = "admin", 
                Email = "admin@oranim.co.il", 
                Password = "123", 
                ProfileImageUrl = "https://picsum.photos/id/1025/200" 
            },
            new User 
            { 
                Username = "israel", 
                Email = "israel@oranim.co.il", 
                Password = "456", 
                ProfileImageUrl = "https://picsum.photos/id/64/200" 
            }
        };

        public MockAuthService() { }

        /// <summary>
        /// מחזיר את כל המשתמשים הרשומים במערכת
        /// </summary>
        public static List<User> Users => _users;

        /// <summary>
        /// פונקציית התחברות למערכת.
        /// בודקת האם קיים משתמש עם שם המשתמש או האימייל המבוקש והסיסמה התואמת.
        /// </summary>
        /// <param name="u">אובייקט יוזר </param>
      
        /// <returns>ערך בוליאני המציין אם ההתחברות הצליחה</returns>
        public bool Login(User u)
        {
            if (string.IsNullOrWhiteSpace(u.Username) || string.IsNullOrWhiteSpace(u.Password))
                return false;

            return _users.Any(us => 
                (us.Username.Equals(u.Username)  && 
                us.Password == u.Password));
        }

        /// <summary>
        /// פונקציה ליצירת חשבון חדש.
        /// מוסיפה משתמש חדש לאוסף הסטטי במידה והשדות תקינים ואינם כפולים.
        /// </summary>
        /// <param name="username">שם משתמש ייחודי</param>
        /// <param name="email">כתובת אימייל ייחודית</param>
        /// <param name="password">סיסמה</param>
        /// <param name="profileImageUrl">כתובת לתמונת פרופיל (אופציונלי)</param>
        /// <returns>אמת במידה והחשבון נוצר בהצלחה</returns>
        /// <exception cref="ArgumentException">נזרק כאשר אחד משדות החובה ריק</exception>
        /// <exception cref="InvalidOperationException">נזרק כאשר שם המשתמש או האימייל כבר קיימים</exception>
        public User CreateAccount(string username, string email, string password, string profileImageUrl)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("כל שדות החובה (שם משתמש, אימייל וסיסמה) חייבים להיות מלאים.");
            }

            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("שם משתמש זה כבר קיים במערכת. אנא בחר שם אחר.");
            }

            if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("כתובת אימייל זו כבר רשומה במערכת.");
            }

            User u = new User
            {
                Username = username,
                Email = email,
                Password = password,
                ProfileImageUrl = string.IsNullOrWhiteSpace(profileImageUrl) ? "https://picsum.photos/id/1025/200" : profileImageUrl
            };

			_users.Add(u);

            return u;
        }
    }
}
