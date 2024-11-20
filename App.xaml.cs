
using CryptoTradingDesktopApp.Api.Services;

namespace CryptoTradingDesktopApp.maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ApiService>();
            DependencyService.Register<AuthService>();
            DependencyService.Register<SignalRService>();

            MainPage = new AppShell();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
}