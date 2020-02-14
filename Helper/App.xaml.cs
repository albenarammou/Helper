using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Helper.Services;
using Helper.Views;

namespace Helper
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<DbDataStore>();
            MainPage = new MainPage();
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
