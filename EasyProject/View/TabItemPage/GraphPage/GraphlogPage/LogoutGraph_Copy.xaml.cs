using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasyProject.Model;
using EasyProject.ViewModel;
using log4net;

namespace EasyProject.View.TabItemPage.GraphPage.GraphlogPage
{
    /// <summary>
    /// LogoutGraph_Copy.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogoutGraph_Copy : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public LogoutGraph_Copy()
        {
            log.Info("Constructor LogoutGraph_Copy() invoked.");
            InitializeComponent();
            var dash = Ioc.Default.GetService<LogViewModel>();
            dash.TodayLoginPrint();
            dash.TodayLogoutPrint();
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("RightBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                NavigationService.Navigate
                               (
                               new Uri("/View/TabItemPage/GraphPage/GraphlogPage/LoginGraph_Copy.xaml", UriKind.Relative) //재고현황화면 --테스트
                               );
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
    }
}
