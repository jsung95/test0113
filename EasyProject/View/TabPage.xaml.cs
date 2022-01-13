using EasyProject.Model;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Media.Animation; using EasyProject.Dao;
using log4net;

namespace EasyProject
{
    /// <summary>
    /// TabPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TabPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        private string CurrentButtonName = "StatusPageTabButton";
        DeptDao dept_dao = new DeptDao();
        public TabPage()
        {
            log.Info("Constructor TapPage() invoked.");
            InitializeComponent();
            userNameTxtBox.Text = App.nurse_dto.Nurse_name;
            user_name.Text = App.nurse_dto.Nurse_name + "님";
            user_no.Text = App.nurse_dto.Nurse_no;
            user_auth.Text = App.nurse_dto.Nurse_auth;
            user_dept.Text = dept_dao.GetDeptName_Return_String((int)App.nurse_dto.Dept_id);
            this.Loaded += PageLoaded;
            TabFrame.NavigationService.Navigated += new NavigatedEventHandler(NavigationService_Navigated);
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            log.Info("NavigationService_Navigated(object, NavigationEventArgs) invoked.");
            try
            {
                TabFrame.NavigationService.RemoveBackEntry();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//NavigationService_Navigated

        /*private void StatusBtn_Click(object sender, RoutedEventArgs e)
{
   NavigationService.Navigate
        (
        new Uri("/View/TabItemPage/StatusPage.xaml", UriKind.Relative) //재고현황화면 --테스트
        );

}*/
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            log.Info("PageLoaded(object, RoutedEventArgs) invoked.");

            try
            {
                if (App.nurse_dto.Nurse_auth.Equals("NORMAL"))
                {
                    InsertPageTabButton.Width = 0;
                    AuthorityPageTabButton.Width = 0;
                    GraphPageTabButton.Width = 0;
                    OrderPageTabButton.Width = 0;
                    LogPageTabButton.Width= 0;
                }
                else if (App.nurse_dto.Nurse_auth.Equals("ADMIN"))
                {
                    AuthorityPageTabButton.Width = 0;
                    GraphPageTabButton.Width = 0;
                    LogPageTabButton.Width = 0;
                }
                else if (App.nurse_dto.Nurse_auth.Equals("SUPER"))
                {
                    StatusPageTabButton.Width = 0;
                    GraphPageTabButton.Width = 0;
                    InsertPageTabButton.Width = 0;
                    IncomingOutgoingPageTabButton.Width = 0;
                    InsertPageTabButton.Width = 0;
                    OrderPageTabButton.Width = 0;

                    TabFrame.Source = new Uri("/View/TabItemPage/AuthorityPage.xaml", UriKind.Relative);
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//PageLoaded

        private void btn_close(object sender, RoutedEventArgs e)       //버튼 창닫기
        {
            log.Info("btn_close(object, RoutedEventArgs) invoked.");

            try
            {

                Window.GetWindow(this).Close();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//btn_close

        private void TabButtonClick(object sender, RoutedEventArgs e)       //버튼 창닫기
        {
            log.Info("TabButtonClick(object, RoutedEventArgs) invoked.");

            try
            {
                string buttonName = ((Button)e.Source).Name;
                Button currentButton = (Button)this.FindName(buttonName);

                //GridCursor.Margin = new Thickness((150 * index), 0, 0, 0);
                if (!buttonName.Equals(CurrentButtonName))
                {
                    var previousButton = (Button)this.FindName(CurrentButtonName);
                    previousButton.Background = null;
                }

                BrushConverter bc = new BrushConverter();

                switch (buttonName)
                {
                    case "StatusPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/StatusPage.xaml", UriKind.Relative);
                        break;
                    case "GraphPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/GraphTabPage.xaml", UriKind.Relative);
                        break;
                    case "InsertPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/InsertPage.xaml", UriKind.Relative);
                        break;
                    case "IncomingOutgoingPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/IncomingOutgoingPageBtn.xaml", UriKind.Relative);
                        break;
                    case "OrderPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/OrderPage.xaml", UriKind.Relative);
                        break;
                    case "AuthorityPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/AuthorityPage.xaml", UriKind.Relative);
                        break;
                    case "LogPageTabButton":
                        TabFrame.Source = new Uri("TabItemPage/LogStatusPageBtn.xaml", UriKind.Relative);
                        break;

                }

                currentButton.Background = (Brush)bc.ConvertFrom("#F0EBE9");
                CurrentButtonName = buttonName;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//TabButtonClick

        private async void ButtonError_Click(object sender, RoutedEventArgs e)
        {
            log.Info("ButtonError_Click(object, RoutedEventArgs)");

            try
            {
                ErrorNotificationMessage msg = new ErrorNotificationMessage();
                msg.Message = "로그아웃을 하시겠습니까?";

                await DialogHost.Show(msg, "RootDialog");
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//ButtonError_Click


        private void pw_Change_Btn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("pw_Change_Btn_Click(object, RoutedEventArgs) invoked.");

            try
            {

                NavigationService.Navigate(new Uri("/View/PasswordChangePage.xaml", UriKind.Relative));
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//pw_Change_Btn_Click

    }//class
}//namespace



