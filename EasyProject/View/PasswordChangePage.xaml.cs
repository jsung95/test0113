using EasyProject.ViewModel;
using log4net;
using MaterialDesignThemes.Wpf;
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

namespace EasyProject.View
{
    /// <summary>
    /// PasswordChangePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PasswordChangePage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public PasswordChangePage()
        {
            log.Info("Constructor PasswordChangePage() invoked.");
            InitializeComponent();
            backBtn.Click += backBtn_Click;
            //pwChangeBtn.Click += pwChangeBtn_Click;

            id_TxtBox.Text = App.nurse_dto.Nurse_no;
            id_TxtBox.Focus();
        }

        private async void pwChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("pwChangeBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                //다이얼로그 창 먼저 끄기 
                PasswordDialogHost.IsOpen = false;

                var temp = Ioc.Default.GetService<PasswordChangeViewModel>();
                var pwChangeTask = Task.Run(() => temp.PasswordChange());
                bool pwChangeResult = await pwChangeTask; // loginTask가 끝나면 결과를 loginResult에 할당

                if (pwChangeResult == true)
                {
                    if (this.NavigationService.CanGoBack)
                    {
                        this.NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("No entries in back navigation history.");
                    }
                    //NavigationService.Navigate( new Uri("/View/LoginPage.xaml", UriKind.Relative) ); //로그인 화면
                }
                else
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//pwChangeBtn_Click

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("backBtn_Click(object, RoutedEventArgs) invoked");
            try
            {
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("No entries in back navigation history.");
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        
        }


    }
}
