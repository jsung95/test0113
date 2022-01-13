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
using Oracle.ManagedDataAccess.Client;
using EasyProject.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Expression.Interactivity.Core;
using log4net;

namespace EasyProject
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        double orginalWidth, originalHeight;
        ScaleTransform scale = new ScaleTransform();

        public MainWindow()
        {
            log.Info("Constructor MainWindow() invoked.");
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;  //화면버튼
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;    //드래그 무브

            this.DataContext = this;
        }

        void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)       //드래그 무브
        {
            log.Info("MainWindow_MouseLeftButtonDown(object, SizeChangedEventArgs) invoked.");
            try
            {
                this.DragMove();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }//MainWindow_MouseLeftButtonDown

        void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.Info("Window1_SizeChanged(object, SizeChangedEventArgs) invoked.");
            try
            {
                ChangeSize(e.NewSize.Width, e.NewSize.Height);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//Window1_SizeChanged

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            log.Info("Window1_Loaded(object, RoutedEventArgs) invoked.");
            try
            {
                orginalWidth = this.Width;
                originalHeight = this.Height;

                if (this.WindowState == WindowState.Maximized)
                {
                    ChangeSize(this.ActualWidth, this.ActualHeight);
                }
                this.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }

        private void ChangeSize(double width, double height)
        {
            log.Info("ChangeSize(double, double) invoked.");
            try
            {
                scale.ScaleX = width / orginalWidth;
                scale.ScaleY = height / originalHeight;

                FrameworkElement rootElement = this.Content as FrameworkElement;
                rootElement.LayoutTransform = scale;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }

        private void btn_close(object sender, RoutedEventArgs e)       //버튼 창닫기
        {
            log.Info("btn_close(object, RoutedEventArgs) invoked.");
            try
            {
                var temp = Ioc.Default.GetService<LoginViewModel>();

                if (temp.idCheckResult) //만약 로그인에 성공했던 상태라면
                {
                    log.Info("==== Terminated Program ====");
                    temp.Logout_For_QuitProgram(); //로그아웃 로깅 데이터를 추가하고
                    Window.GetWindow(this).Close(); //프로그램 종료
                }
                else
                {
                    log.Info("==== Terminated Program ====");
                    Window.GetWindow(this).Close();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            

        }

        private void btn_minimize(object sender, RoutedEventArgs e)       //화면 내리기
        {
            log.Info("btn_minimize(object, RoutedEventArgs) invoked.");
            try
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Minimized;
                }
                else if (this.WindowState == WindowState.Minimized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Minimized;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }

        private void btn_extend(object sender, RoutedEventArgs e)       //화면 확대 축소
        {
            log.Info("btn_extend(object, RoutedEventArgs) invoked.");
            try
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            

        }


        private ActionCommand exit_Command;
        public ICommand Exit_Command
        {
            get
            {
                if (exit_Command == null)
                {
                    exit_Command = new ActionCommand(exit_Program);
                }
                return exit_Command;
            }
        }

        private void exit_Program()       //버튼 창닫기
        {
            log.Info("exit_Program() invoked.");
            try
            {
                var temp = Ioc.Default.GetService<LoginViewModel>();

                if (temp.idCheckResult) //만약 로그인에 성공했던 상태라면
                {
                    log.Info("==== Terminated Program ====");
                    temp.Logout_For_QuitProgram(); //로그아웃 로깅 데이터를 추가하고
                    Window.GetWindow(this).Close(); //프로그램 종료
                }
                else
                {
                    log.Info("==== Terminated Program ====");
                    Window.GetWindow(this).Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            

        }

    }
}
