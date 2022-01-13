using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.Linq;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Collections;
using EasyProject.Model;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using EasyProject.ViewModel;
using System.Windows.Data;
using System.Globalization;
using log4net;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Text;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;


namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// StatusPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatusPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public ChartValues<float> Values { get; set; }

        public bool isComboBoxDropDownOpened = false;

        public string categoryComboBoxFirstSelected = null;
        public String userDept00 = null;

        public StatusPage()
        {
            log.Info("Constructor StatusPage() invoked.");
            InitializeComponent();
            //dept_Label.Visibility = Visibility.Hidden;
            //Dept_comboBox.Visibility = Visibility.Hidden;
            
            export_btn.Click += Export_btn_Click;
            export_btn2.Click += Export_btn2_Click;
            this.Loaded += MainWindow_Loaded;

        }

        private void Export_btn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("Export_btn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                var temp = Ioc.Default.GetService<ProductShowViewModel>();
                userDept00 = temp.SelectedDept.Dept_name;

                var datas = temp.Products;
                userDept00 = temp.SelectedDept.Dept_name;
                string result = "제품코드, 제품명, 품목/종류, 제품가격, 수량, 유통기한\n";
                foreach (var data in datas)
                {
                    result = result + data.Prod_code + ", " + data.Prod_name + ", " + data.Category_name + ", " + data.Prod_price + ", " + data.Imp_dept_count + ", " + data.Prod_expire + "\n"; // data.Prod_expire.ToString("yyyy-MM-dd")

                }
                //Clipboard.Clear();

                // DateTime now = DateTime.Now;
                string today = String.Format(DateTime.Now.ToString("yyyy/MM/dd_HHmmss"));

                //Console.WriteLine(result);
                string f_path = @"c:\temp\[" + userDept00 + "]" + "재고현황_" + today + ".csv";
                File.AppendAllText(f_path, result, UnicodeEncoding.UTF8);

                // Get the Excel application object.
                Excel.Application excel_app = new Excel.Application();

                // Make Excel visible (optional).
                excel_app.Visible = true;

                // Open the file.
                excel_app.Workbooks.Open(
                    f_path,               // Filename
                    Type.Missing,
                    Type.Missing,

                       Excel.XlFileFormat.xlCSV,   // Format
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,

                       ",",          // Delimiter
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing
                );
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //로그 Level : error
            }
        }

        private void Export_btn2_Click(object sender, RoutedEventArgs e)
        {
            log.Info(" Export_btn2_Click(object, RoutedEventArgs) invoked.");
            try
            {

                var temp = Ioc.Default.GetService<ProductShowViewModel>();
                var datas = temp.LstOfRecords;
                userDept00 = temp.SelectedDept.Dept_name;
                string result = "제품코드, 제품명, 품목/종류, 제품가격, 수량, 유통기한\n";
                foreach (var data in datas) 
                {
                    result = result + data.Prod_code + ", " + data.Prod_name + ", " + data.Category_name + ", " + data.Prod_price + ", " + data.Imp_dept_count + ", " + data.Prod_expire + "\n"; // data.Prod_expire.ToString("yyyy-MM-dd")

                }
                //Clipboard.Clear();

                // DateTime now = DateTime.Now;
                string today = String.Format(DateTime.Now.ToString("yyyy/MM/dd_HHmmss"));

                //Console.WriteLine(result);
                string f_path = @"c:\temp\[" + userDept00 + "]" + "재고현황_" + today + ".csv";
                File.AppendAllText(f_path, result, UnicodeEncoding.UTF8);

                // Get the Excel application object.
                Excel.Application excel_app = new Excel.Application();

                // Make Excel visible (optional).
                excel_app.Visible = true;

                // Open the file.
                excel_app.Workbooks.Open(
                    f_path,               // Filename
                    Type.Missing,
                    Type.Missing,

                       Excel.XlFileFormat.xlCSV,   // Format
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,

                       ",",          // Delimiter
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing,
                       Type.Missing
                );
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //로그 Level : error
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
    where T : DependencyObject
        {
            log.Info("FindChild<T>(DependencyObject, string) invoked.");
            T foundChild = null;
            try
            {
                // Confirm parent and childName are valid. 
                if (parent == null) return null;

                

                int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    // If the child is not of the request child type child
                    T childType = child as T;
                    if (childType == null)
                    {
                        // recursively drill down the tree
                        foundChild = FindChild<T>(child, childName);

                        // If the child is found, break so we do not overwrite the found child. 
                        if (foundChild != null) break;
                    }
                    else if (!string.IsNullOrEmpty(childName))
                    {
                        var frameworkElement = child as FrameworkElement;
                        // If the child's name is set for search
                        if (frameworkElement != null && frameworkElement.Name == childName)
                        {
                            // if the child's name is of the request name
                            foundChild = (T)child;
                            break;
                        }
                    }
                    else
                    {
                        // child element found.
                        foundChild = (T)child;
                        break;
                    }
                }

                return foundChild;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return foundChild;
            }//catch

        }//FindChild

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            log.Info("MainWindow_Loaded(object, RoutedEventArgs) invoked.");

            try
            {
                var deptModelObject = deptName_ComboBox1.SelectedValue as DeptModel;
                var deptNameText = deptModelObject.Dept_name; // 콤보박스에서 선택한 부서명
                var temp = Ioc.Default.GetService<ProductShowViewModel>();
                var userDept = temp.Depts[(int)App.nurse_dto.Dept_id - 1];  // 현재 사용자 소속 부서 객체
                var userDeptName = userDept.Dept_name;
                if(temp.Nurse.Nurse_auth == "NORMAL")
                {
                    deptName_ComboBox1.Visibility = Visibility.Hidden;
                    dept_TextBox.Visibility = Visibility.Visible;
                    dept_TextBox.Text = temp.SelectedDept.Dept_name;
                }
                var dash = Ioc.Default.GetService<ProductShowViewModel>();
                dash.DashboardPrint1(dash.SelectedDept, dash.SelectedCategory1, dash.SelectedNumber);
                dash.DashboardPrint2(dash.SelectedDept);

                if (App.nurse_dto.Nurse_auth.Equals("ADMIN"))
                {

                    if (deptNameText.Equals(userDeptName) || userDeptName == null)
                    {
                        Console.WriteLine(userDeptName + "같은 부서일때");
                        buttonColumn.Visibility = Visibility.Visible;
                        ModifyToggleButtonPanel.Visibility = Visibility.Visible;
                        //ModifyToggleButtonPanel.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Console.WriteLine(userDeptName + "다른 부서일때");
                        buttonColumn.Visibility = Visibility.Hidden;
                        ModifyToggleButtonPanel.Visibility = Visibility.Hidden;
                    }
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//MainWindow_Loaded

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            log.Info("SomeSelectionChanged(object, SelectionChangedEventArgs)");

            try
            {
                var comboBox = sender as ComboBox;

                if (comboBox.SelectedItem != null)
                {
                    ((ProductShowViewModel)(this.DataContext)).ComboBoxCategoryName = (comboBox.SelectedItem as CategoryModel).Category_name;
                    Console.WriteLine(((ProductShowViewModel)(this.DataContext)).ComboBoxCategoryName);

                    if ((comboBox.SelectedItem as CategoryModel).Category_name !=
                    ((ProductShowViewModel)(this.DataContext)).SelectedProduct.Category_name &&
                    (comboBox.SelectedItem as CategoryModel).Category_name != "직접입력")
                    {
                        //Console.WriteLine("다르다");

                        ((ProductShowViewModel)(this.DataContext)).SelectedProduct.Category_name = (comboBox.SelectedItem as CategoryModel).Category_name;
                        //Console.WriteLine(((ProductShowViewModel)(this.DataContext)).SelectedProduct.Category_name);
                    }
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//SomeSelectionChanged

        private void DataGridKeyDown(object sender, KeyEventArgs e)
        {
            log.Info("DataGridKeyDown(object, KeyEventArgs) invoked.");

            try
            {
                if (e.Key == Key.Enter)
                {


                    //dataGrid.SelectedCells[0].Column.GetCellContent;
                    dataGrid.Focus();


                    //ErrorNotificationMessage msg = new ErrorNotificationMessage();
                    //msg.Message = "재고수정을 하시겠습니까?";
                    //await DialogHost.Show(msg, "RootDialog");
                    //DialogHost.ShowDialog(DialogHost.DialogContent);

                    ((ProductShowViewModel)(this.DataContext)).EditProduct();
                    //DialogHost.CloseDialogCommand.Execute(null, null);
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//DataGridKeyDown

        //private void KeyDown(object sender, KeyEventArgs e)
        //{
        //    var cmb = sender as ComboBox;

        //    if(e.Key == Key.Enter || e.Key == Key.Tab)
        //    {
        //        ((ProductShowViewModel)(this.DataContext)).AddNewCategory(cmb.Text);
        //        ((ProductShowViewModel)(this.DataContext)).SelectedProduct.Category_name = cmb.Text;
        //        ((ProductShowViewModel)(this.DataContext)).EditProduct();
        //    } 
        //}

        private void OnDropDownOpened(object sender, EventArgs e)
        {
            log.Info("OnDropDownOpened(object, EventArgs) invoked.");

            try
            {
                isComboBoxDropDownOpened = true;

                var deptModelObject = deptName_ComboBox1.SelectedValue as DeptModel;
                var deptNameText = deptModelObject.Dept_name; // 콤보박스에서 선택한 부서명
                var temp = Ioc.Default.GetService<ProductShowViewModel>();
                var userDept = temp.Depts[(int)App.nurse_dto.Dept_id - 1];  // 현재 사용자 소속 부서 객체
                var userDeptName = userDept.Dept_name;

                if (App.nurse_dto.Nurse_auth.Equals("ADMIN"))
                {
                    if (isComboBoxDropDownOpened)
                    {

                        if (deptNameText.Equals(userDeptName) || userDeptName == null)
                        {
                            Console.WriteLine(userDeptName + "같은 부서일때");
                            buttonColumn.Visibility = Visibility.Visible;
                            ModifyToggleButtonPanel.Visibility = Visibility.Visible;
                            //ModifyToggleButtonPanel.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            Console.WriteLine(userDeptName + "다른 부서일때");
                            buttonColumn.Visibility = Visibility.Hidden;
                            ModifyToggleButtonPanel.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//OnDropDownOpened

        private void DataGridCheckboxClick(object sender, RoutedEventArgs e)
        {
            log.Info("DataGridCheckboxClick(object, RoutedEventArgs) invoked.");

            try
            {
                if (DataGridCheckbox.IsChecked == true)
                {
                    //DataAndGraphGrid.ColumnDefinitions.Add(DataGridColumn);
                    DataGridColumn.Width = new GridLength(1.8, GridUnitType.Star);
                }
                else
                {
                    DataGridColumn.Width = new GridLength(0);
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//DataGridCheckboxClick

        private void GraphCheckboxClick(object sender, RoutedEventArgs e)
        {
            log.Info("GraphCheckboxClick(object, RoutedEventArgs) invoked.");

            try
            {
                if (GraphCheckbox.IsChecked == true)
                {
                    GraphColumn.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    GraphColumn.Width = new GridLength(0);
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//GraphCheckboxClick

        private void GraphCheckboxUnChecked(object sender, RoutedEventArgs e)
        {
            log.Info("GraphCheckboxUnChecked(object, RoutedEventArgs) invoked.");

            try
            {
                GraphCard.Visibility = Visibility.Visible;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//GraphCheckboxUnChecked



    }//class

    public class IsLesserThanConverter : IValueConverter //Red
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public static readonly IValueConverter Instance = new IsLesserThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.Info("Convert(object, Type, object, CultureInfo) invoked.");
            bool checkBool = false;
            try
            {
                
                if (value != null && targetType != null)
                {
                    int intValue = (int)value;//남은 일수
                    int compareToValue = Int32.Parse(parameter.ToString());

                    checkBool = intValue < compareToValue;
                }

                return checkBool;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return checkBool;
            }//catch

        }//Convert

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.Info("ConvertBack(object, Type, object, CultureInfo) invoked.");

            try
            {
                throw new NotImplementedException();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new NotImplementedException();
            }//catch

        }//ConvertBack
    }//class

    public class IsEqualOrLessGreaterThanConverter : IValueConverter
    {//Yellow
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public static readonly IValueConverter Instance = new IsEqualOrLessGreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            bool checkBool = false;
            log.Info("Convert(object, Type, object, CultureInfo) invoked.");

            try
            {
                if (value != null && targetType != null)
                {
                    int intValue = (int)value;//남은 일수
                    int compareToValue = Int32.Parse(parameter.ToString());

                    checkBool = ((intValue > compareToValue) && (intValue - compareToValue < 3))
                    || (intValue == compareToValue);
                }

                return checkBool;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return checkBool;
            }//catch

        }//Convert

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.Info("ConvertBack(object, Type, object, CultureInfo) invoked.");

            try
            {
                throw new NotImplementedException();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new NotImplementedException();
            }//catch
        }//ConvertBack
    }//class

    public class IsGreaterThanConverter : IValueConverter
    {//Green
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public static readonly IValueConverter Instance = new IsGreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool checkBool = false;
            log.Info("Convert(object, Type, object, CultureInfo) invoked.");

            try
            {
                if (value != null && targetType != null)
                {
                    int intValue = (int)value;//남은 일수
                    int compareToValue = Int32.Parse(parameter.ToString());

                    checkBool = (intValue > compareToValue) && (intValue - compareToValue > 3);
                }

                return checkBool;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return checkBool;
            }//catch

        }//Convert

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.Info("ConvertBack(object, Type, object, CultureInfo) invoked.");

            try
            {

                throw new NotImplementedException();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new NotImplementedException();
            }//catch
        }
    }//class



}//namespace
