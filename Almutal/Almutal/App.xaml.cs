using Almutal.Services;
using Almutal.Services.DialogService;
using Almutal.Services.MessagingService;
using Almutal.Services.NavigationService;
using Almutal.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("fa-solid-900.ttf", Alias = "SolidAwesome")]
namespace Almutal
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDialogService, ShellDialogService>();
            DependencyService.Register<INavigationService, ShellRoutingService>();
            DependencyService.Register<IMessagingService, MessagingService>();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
            //MainPage = new DataEntryView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
