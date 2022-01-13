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

namespace EasyProject.View.TabItemPage.GraphPage.GraphlogPage
{
    /// <summary>
    /// LogoutGraph.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogoutGraph : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public LogoutGraph()
        {
            log.Info("Constructor LogoutGraph() invoked.");
            InitializeComponent();
            var dash = Ioc.Default.GetService<LogViewModel>();
            dash.TodayLoginPrint();
            dash.TodayLogoutPrint();
        }
    }
}
