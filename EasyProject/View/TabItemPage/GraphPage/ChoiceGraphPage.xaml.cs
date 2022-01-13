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

namespace EasyProject.View.TabItemPage.GraphPage
{
    /// <summary>
    /// ChoiceGraphPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChoiceGraphPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public ChoiceGraphPage()
        {
            log.Info("Constructor ChoiceGraphPage() invoked.");
            InitializeComponent();
        }
    }
}
