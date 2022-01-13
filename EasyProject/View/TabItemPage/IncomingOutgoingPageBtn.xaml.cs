using EasyProject.Model;
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
    /// IncomingOutgoingPageBtn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IncomingOutgoingPageBtn : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public String userDept00 = null;
        public bool isComboBoxDropDownOpened = false;
        public IncomingOutgoingPageBtn()
        {
            log.Info("Constructor IncomingOutgoingPageBtn() invoked.");
            InitializeComponent();
            OutcomingBtn.Click += Incoming_Click;
            OutgoingBtn.Click += Outgoing_Click;
        }

        private void Incoming_Click(object sender, RoutedEventArgs e)
        {
            log.Info("Incoming_Click(object, RoutedEventArgs) invoked.");
            try
            {
                OutcomingBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"));
                OutgoingBtn.Background = System.Windows.Media.Brushes.LightGray;
                OutcomingBtn.Foreground = System.Windows.Media.Brushes.White;
                OutgoingBtn.Foreground = System.Windows.Media.Brushes.Black;

                ListFrame.Source = new Uri("IncomingOutgoingList1Page.xaml", UriKind.Relative);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
        private void Outgoing_Click(object sender, RoutedEventArgs e)
        {
            log.Info("Outgoing_Click(object, RoutedEventArgs) invoked.");
            try
            {
                OutcomingBtn.Background = System.Windows.Media.Brushes.LightGray;
                OutgoingBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"));
                OutcomingBtn.Foreground = System.Windows.Media.Brushes.Black;
                OutgoingBtn.Foreground = System.Windows.Media.Brushes.White;
                ListFrame.Source = new Uri("IncomingOutgoingList2Page.xaml", UriKind.Relative);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
    }
}
