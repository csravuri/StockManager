using System;
using StockManager.Models;
using StockManager.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StockManager
{
    public partial class App : Application
    {
        public StockCreateDB StockCreateDB;
        public App()
        {
            InitializeComponent();

            if (DateTime.Now > DateTime.Parse("2020-02-15"))
            {
                MainPage = new ExpiryPage();
            }
            else
            {
                MainPage = new NavigationPage(new StockViewPage());
            }            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
