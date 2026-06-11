# מדריך למשתמש ומפתח: מימוש תבנית MVVM ב- .NET MAUI

מדריך זה מלווה אותך שלב-אחר-שלב מהשלבים הראשוניים של יצירת פרויקט פשוט ועד למימוש המלא של תבנית העיצוב **MVVM (Model-View-ViewModel)** עבור דף ההתחברות.

---

## 📖 מבוקש ומבוא ל-MVVM
תבנית MVVM נועדה להפריד בצורה מוחלטת בין הממשק הגרפי (העיצוב ב-XAML) לבין הלוגיקה העסקית והנתונים (בקוד C#).
*   **Model (המודל):** מייצג את הנתונים והלוגיקה העסקית הבסיסית (לדוגמה: מחלקת `User`).
*   **View (התצוגה):** ממשק המשתמש הגדרתי ב-XAML (לדוגמה: `LoginPage.xaml`).
*   **ViewModel (מודל התצוגה):** הגשר המתווך בין ה-View ל-Model. הוא מחזיק את המצב (State) של הדף, מגיב לפעולות המשתמש באמצעות פקודות (Commands) ומעדכן את המסך על שינויים בנתונים.

---

## 🛠️ שלב 1: תשתית הודעה על שינויים (INotifyPropertyChanged)
על מנת שה-View (המסך) ידע להתעדכן באופן אוטומטי בכל פעם שמשתנה כלשהו ב-ViewModel משנה את ערכו, עלינו לממש את ממשק `INotifyPropertyChanged`.

בפרויקט זה הגדרנו את המחלקה הבאה בתיקיית `Models`:

```csharp
// Models/ObservableObject.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmLogin.Models
{
    public partial class ObservableObject : INotifyPropertyChanged
    {
        // האירוע שנורה בכל פעם שמאפיין כלשהו משתנה
        public event PropertyChangedEventHandler PropertyChanged;

        // מתודת עזר השולחת הודעה לממשק המשתמש לעדכן את השדה
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

לאחר מכן, יצרנו את מחלקת הבסיס לכל ה-ViewModels שנקראת `ViewModelBase.cs` ויורשת מ-`ObservableObject`:
```csharp
// ViewModels/ViewModelBase.cs
using MvvmLogin.Models;

namespace MvvmLogin.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
    }
}
```

---

## ⚙️ שלב 2: בניית ה-ViewModel והגדרת Properties
בתוך ה-ViewModel (לדוגמה `LoginPageViewModel.cs`) אנו מגדירים מאפיינים (Properties) שיחזיקו את המידע שהמשתמש מקליד או רואה במסך.

בכל פעם שערך של מאפיין משתנה (בתוך ה-`set`), נקרא למתודה `OnPropertyChanged()` כדי לעדכן את המסך:

```csharp
// שדה פרטי (Backing Field)
private string userName;

// מאפיין ציבורי החשוף למסך
public string UserName
{
    get => userName;
    set
    {
        userName = value;
        OnPropertyChanged(); // קריאה לעדכון ה-View
    }
}
```

---

## ⚡ שלב 3: הגדרת פעולות בעזרת פקודות (ICommand)
ב-MVVM אנו נמנעים משימוש באירועים קלאסיים (כמו `Clicked` ב-Code-Behind). במקום זאת, אנו משתמשים בממשק `ICommand`.

### 1. הגדרת ה-Command ב-ViewModel:
מגדירים Property מסוג `ICommand` עם הרשאת כתיבה פרטית:
```csharp
using System.Windows.Input;

public ICommand LoginCommand { get; private set; }
```

### 2. אתחול ה-Command בבנאי (Constructor):
מאתחלים את ה-Command ומקשרים אותו למתודה שמבצעת את הפעולה (`Login`) ולמתודה שבודקת האם הפעולה אפשרית (`CanLogin`):
```csharp
public LoginPageViewModel()
{
    service = new MockAuthService();
    
    // יצירת Command חדש
    LoginCommand = new Command(Login, CanLogin);
}
```

### 3. מתודת הראשי והבדיקה (Execute & CanExecute):
```csharp
// מחזיר True רק אם שדות הקלט אינם ריקים
private bool CanLogin()
{
    return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
}

// מבוצע בעת לחיצה על כפתור ההתחברות
private void Login()
{
    // לוגיקה לאימות מול השרת/שירות
    User u = new User { Username = UserName, Password = Password };
    IsNotLoggedIn = !service.Login(u);
    
    // ניקוי השדות
    UserName = string.Empty;
    Password = string.Empty;
}
```

### 4. שינוי מצב הכפתור בזמן אמת (`ChangeCanExecute`):
כדי שהכפתור יהפוך לפעיל או כבוי באופן אוטומטי בזמן שהמשתמש מקליד, עלינו לקרוא למתודה `ChangeCanExecute()` בתוך ה-`set` של המאפיינים המשפיעים על התנאי:
```csharp
public string UserName
{
    get => userName;
    set
    {
        userName = value;
        OnPropertyChanged();
        
        // כפיית בדיקה מחדש של CanLogin ושינוי מצב כפתור ה-Button במסך
        ((Command)LoginCommand).ChangeCanExecute();
    }
}
```

---

## 🎨 שלב 4: קישור ב-View (XAML) - xmlns ו- x:DataType
על מנת שהמסך יכיר את ה-ViewModel ונוכל להשתמש בקישורים מקומפלים (Compiled Bindings) הבטוחים יותר ומהירים יותר, עלינו לבצע את ההגדרות הבאות בתגית ה-`ContentPage` הראשית:

### 1. הגדרת מרחב השמות (`xmlns`):
מייבאים את מרחב השמות של ה-ViewModels לתוך ה-XAML ומקצים לו כינוי (למשל: `viewmodels`):
```xml
xmlns:viewmodels="clr-namespace:MvvmLogin.ViewModels"
```

### 2. הגדרת טיפוס הנתונים של הקישור (`x:DataType`):
מגדירים ל-XAML מאיזה ViewModel הוא שואב את הנתונים שלו:
```xml
x:DataType="viewmodels:LoginPageViewModel"
```

### 3. ביצוע Binding למאפיינים ולפקודות:
כעת ניתן לקשר את הפקדים בתוך המסך למאפיינים ולפקודות שהגדרנו ב-ViewModel:

*   **קישור דו-כיווני לקלט של משתמש (Entry):**
    ```xml
    <Entry Placeholder="הכנס שם משתמש"
           Text="{Binding UserName}" />
    ```
    *(בכל פעם שהמשתמש מקליד בתיבה, ה-UserName ב-ViewModel מתעדכן, ובכל פעם שה-ViewModel מנקה את השדה, הטקסט בתיבה נמחק).*

*   **קישור למאפיין ראות (IsVisible) וטקסט שגיאה:**
    ```xml
    <Border IsVisible="{Binding IsNotLoggedIn}">
        <Label Text="{Binding Error}" />
    </Border>
    ```

*   **קישור כפתור לפקודה (Command):**
    ```xml
    <Button Text="התחבר"
            Command="{Binding LoginCommand}" />
    ```
    *(הכפתור יהיה זמין או מבוטל אוטומטית בהתאם לערך המוחזר מ-CanLogin, ובלחיצה עליו תופעל המתודה Login).*

---

## 🔗 שלב 5: חיבור ה-View ל-ViewModel בקוד (BindingContext)
כדי שכל הקישורים (Bindings) שהגדרנו ב-XAML יעבדו בפועל, האפליקציה צריכה לדעת מהו מקור הנתונים של הדף בזמן ריצה. 

אנו מגדירים זאת בקובץ ה-Code-Behind של הדף (`LoginPage.xaml.cs`):

```csharp
// Pages/LoginPage.xaml.cs
using Microsoft.Maui.Controls;
using MvvmLogin.ViewModels;

namespace MvvmLogin.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // הגדרת מקור הנתונים הראשי של המסך
            BindingContext = new LoginPageViewModel();
        }
    }
}
```

---

## 💡 טיפים מנצחים לעבודה נכונה ב-MVVM
1.  **קוד נקי ב-Code-Behind:** בתוך קובץ ה-`.xaml.cs` צריך להיות אך ורק קריאה ל-`InitializeComponent()` והגדרת ה-`BindingContext`. שום לוגיקה עסקית אחרת לא צריכה להיכתב שם.
2.  **תמיד להשתמש ב-x:DataType:** הגדרת טיפוס הנתונים מאפשרת ל-Compiler להתריע בפנינו על שגיאות כתיב בשמות המאפיינים כבר בזמן הבנייה של הפרויקט, ולא בזמן ריצה.
3.  **הקפדה על OnPropertyChanged():** אם שכחת לקרוא ל-`OnPropertyChanged()` ב-set של Property כלשהו, המסך פשוט לא יתעדכן כאשר הערך של המאפיין ישתנה בקוד.
