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
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using EasyProject.ViewModel;
using EasyProject.Model;
using log4net;

namespace EasyProject.View.TabItemPage.GraphPage
{
    /// <summary>
    /// DeptCate_GraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeptCate_GraphPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public DeptCate_GraphPage()
        {
            log.Info("Constructor DeptCate_GraphPage() invoked.");
            InitializeComponent();
            var dash = Ioc.Default.GetService<ProductShowViewModel>();
            //temp.DashboardPrint();
            dash.DashboardPrint1(dash.SelectedDept, dash.SelectedCategory1, dash.SelectedNumber);
        }

        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("rightBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                NavigationService.Navigate
                               (
                               new Uri("/View/TabItemPage/GraphPage/RemainExpire_GraphPage.xaml", UriKind.Relative) //재고현황화면 --테스트
                               );

                var dash = Ioc.Default.GetService<ProductShowViewModel>();
                dash.DashboardPrint2(dash.SelectedDept);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
    }
}
