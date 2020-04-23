using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FinAccount.Models;
using FinAccount.Views;
using FinAccount.ViewModels;

namespace FinAccount {
    public partial class App : Application {
        private const string DATABASE_NAME = "finacc.db";
        private static RecordsRepository database;

        public static RecordsRepository DataBase {
            get {
                if(database == null) {
                    database = new RecordsRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }

        public App() {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage()) {
                BarBackgroundColor = Color.FromHex("4CAF50")
            };
            MainPage.BindingContext = new MainPageViewModel {
                Navigation = MainPage.Navigation
            };
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
