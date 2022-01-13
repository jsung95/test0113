using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasyProject.Model;
using EasyProject.ViewModel;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace EasyProject
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        
        protected override void OnStartup(StartupEventArgs e)
        {
            log.Info("OnStartup(StartupEventArgs) invoked.");
            base.OnStartup(e);
        }

        public static NurseModel nurse_dto = null;
        public static CategoryModel category_dto = null;
        public App()
        {
            log.Info("==== START UP EASY_PROJECT ====");
            log.Info("Constructor App() invoked.");
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<ProductViewModel>()
                .AddSingleton<LoginViewModel>()
                .AddSingleton<ProductShowViewModel>()
                .AddSingleton<SignupViewModel>()
                .AddSingleton<PasswordChangeViewModel>()
                .AddSingleton<ProductInOutViewModel>()
                .AddSingleton<UserAuthViewModel>()
                .AddSingleton<LogViewModel>()
                .BuildServiceProvider());

            this.InitializeComponent();

            nurse_dto = new NurseModel();



        }//App Constructor
    }
}
