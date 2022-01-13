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
    /// InsertPage_Category.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertPage_Category : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public InsertPage_Category()
        {
            InitializeComponent();
            CategoryAddBtn.Click += CategoryAddBtnClick;
        }

        private void CategoryAddBtnClick(object sender, RoutedEventArgs e)
        {
            log.Info("CategoryAddBtnClick(object, RoutedEventArgs) invoked.");
            

        }
    }
}
