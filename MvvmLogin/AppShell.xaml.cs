using MvvmLogin.Pages;

namespace MvvmLogin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // רישום נתיבי הניווט של האפליקציה לצורך מעבר בין הדפים בעזרת Shell Navigation
            Routing.RegisterRoute("CreateAccountPage", typeof(CreateAccountPage));
        }
    }
}
