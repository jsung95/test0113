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
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace EasyProject.View.TabItemPage.GraphPage
{
    /// <summary>
    /// Outgoing_GraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Outgoing_GraphPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public Outgoing_GraphPage()
        {
            log.Info("Constructor Outgoing_GraphPage() invoked.");
            InitializeComponent();
            /*var temp = Ioc.Default.GetService<ProductInOutViewModel>();
            temp.DashboardPrint_Pie();*/
        }
        

      

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("RightBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                NavigationService.Navigate
                (
                new Uri("/View/TabItemPage/GraphPage/DiscardProdPrice_GraphPage.xaml", UriKind.Relative) //재고현황화면 --테스트
                );
                /*var dash = Ioc.Default.GetService<ProductInOutViewModel>();
                dash.DashboardPrint2(dash.SelectedStartDate_Out, dash.SelectedEndDate_Out);
                dash.DashboardPrint_Pie();*/
                
            }
            catch (Exception ex) 
            {
                log.Error(ex.Message);
            }
            
        }
    }
}
