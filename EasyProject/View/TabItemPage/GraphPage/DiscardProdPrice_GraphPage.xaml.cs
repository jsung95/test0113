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
using EasyProject.ViewModel;
using EasyProject.Model;
using log4net;

namespace EasyProject.View.TabItemPage.GraphPage
{
    /// <summary>
    /// DiscardProdPrice_GraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DiscardProdPrice_GraphPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public DiscardProdPrice_GraphPage()
        {
            log.Info("Constructor DiscardProdPrice_GraphPage() invoked.");
            InitializeComponent();
            var dash = Ioc.Default.GetService<ProductInOutViewModel>();
            //temp.DashboardPrint();
            /*dash.DashboardPrint_Pie();
            dash.DashboardPrint2(dash.SelectedStartDate_Out, dash.SelectedEndDate_Out);
            dash.DashboardPrint3(dash.SelectedStartDate_Out, dash.SelectedEndDate_Out);*/
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("LeftBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                NavigationService.Navigate
               (
               new Uri("/View/TabItemPage/GraphPage/Outgoing_GraphPage.xaml", UriKind.Relative) //재고현황화면 --테스트
               );
                /*var dash = Ioc.Default.GetService<ProductInOutViewModel>();
                dash.DashboardPrint_Pie();
                dash.DashboardPrint2(dash.SelectedStartDate_Out, dash.SelectedEndDate_Out);
                dash.DashboardPrint3(dash.SelectedStartDate_Out, dash.SelectedEndDate_Out);*/
                
            }
            catch (Exception ex) 
            {
                log.Error(ex.Message);
            }
           
        }
    }
}
