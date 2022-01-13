using log4net;
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

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// LogStatusPageBtn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogStatusPageBtn : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public String userDept00 = null;
        public bool isComboBoxDropDownOpened = false;

        public LogStatusPageBtn()
        {
            log.Info("Constructor LogStatusPageBtn() invoked.");
            InitializeComponent();
            EventLogBtn.Click += EventLog_Click;
            LoginBtn.Click += Login_Click;
            LogoutBtn.Click += Logout_Click;
            InitializeComponent();
        }
        private void EventLog_Click(object sender, RoutedEventArgs e)
        {
            log.Info("EventLog_Click(object, RoutedEventArgs) invoked.");
            try
            {
                ListFrame.Source = new Uri("LogStatusList1Page.xaml", UriKind.Relative);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            log.Info("Login_Click(object, RoutedEventArgs) invoked.");
            try
            {
                ListFrame.Source = new Uri("LogStatusList2Page.xaml", UriKind.Relative);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            log.Info("Logout_Click(object, RoutedEventArgs) invoked.");
            try
            {
                ListFrame.Source = new Uri("LogStatusList3Page.xaml", UriKind.Relative);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }
    }
}
