using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using EasyProject.Model;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Excel = Microsoft.Office.Interop.Excel;
using EasyProject.ViewModel;
using log4net;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// IncomingOutgoingList1Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IncomingOutgoingList1Page : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public String userDept00 = null;
        public bool isComboBoxDropDownOpened = false;
        //Boolean headerflag = false;

        public IncomingOutgoingList1Page()
        {
            log.Info("Constructor IncomingOutgoingList1Page() invoked.");
            InitializeComponent();
           
            export_btn.Click += Export_btn_Click;
            export_btn2.Click += Export_btn2_Click;
            //userDept00 = (deptName_ComboBox1.SelectedValue as DeptModel).Dept_name;
            /*var dash = Ioc.Default.GetService<ProductInOutViewModel>();
            //temp.DashboardPrint();
            dash.DashboardPrint2(dash.SelectedStartDate_In, dash.SelectedEndDate_In);
            dash.DashboardPrint3(dash.SelectedStartDate_In, dash.SelectedEndDate_In);   //입고
            //temp.DashboardPrint4(temp.SelectedCategory1);
            dash.DashboardPrint_Pie();*/

            var temp = Ioc.Default.GetService<ProductShowViewModel>();
            if (temp.Nurse.Nurse_auth == "NORMAL")
            {
                deptName_ComboBox1.Visibility = Visibility.Hidden;
                dept_TextBox.Visibility = Visibility.Visible;
                dept_TextBox.Text = temp.SelectedDept.Dept_name;
            }
        }      
       
        //private void OnDropDownOpened(object sender, EventArgs e)
        //{
        //    isComboBoxDropDownOpened = true;

        //    var deptModelObject = deptName_ComboBox1.SelectedValue as DeptModel;
        //    var deptNameText = deptModelObject.Dept_name;
        //    userDept00= deptNameText.ToString();
        //}

        private void OnDropDownOpened(object sender, EventArgs e)
        {
            log.Info("OnDropDownOpened(object, EventArgs) invoked.");
            try
            {
                isComboBoxDropDownOpened = true;

                var deptModelObject = deptName_ComboBox1.SelectedValue as DeptModel;
                var deptNameText = deptModelObject.Dept_name;
                userDept00 = deptNameText.ToString();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        }


        private void Export_btn_Click(object sender, RoutedEventArgs e)
        {
            log.Info(" Export_btn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                var temp = Ioc.Default.GetService<ProductShowViewModel>();
                userDept00 = temp.SelectedDept.Dept_name;

                dataGrid1.SelectAllCells();
                dataGrid1.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, dataGrid1);

                String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                Clipboard.Clear();

                dataGrid1.UnselectAllCells();

                // DateTime now = DateTime.Now;
                string today = String.Format(DateTime.Now.ToString("yyyy/MM/dd_HHmmss"));

                Console.WriteLine(result);
                string f_path = @"c:\temp\[" + userDept00 + "]" + "입고현황_" + today + ".csv";
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
  
                var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                var datas = temp.InLstOfRecords;
                userDept00 = temp.SelectedDept.Dept_name;
                string result = "제품코드, 제품명, 품목/종류, 유통기한, 입고일, 입고유형, 관리자\n";
                foreach(var data in datas)
                {
                    result = result + data.Prod_code + ", " + data.Prod_name + ", " + data.Category_name + ", " + data.Prod_expire + ", " + data.Prod_in_date + ", " + data.Prod_in_type + ", " + data.Nurse_name;
                    if (data.Prod_in_type == "신규" || data.Prod_in_type == "추가")
                    {
                        result += "(" + data.Prod_in_to + ")\n";
                    }
                    else // 이관
                    {
                        result += "(" + data.Prod_in_from + ")\n";
                    }                                            
                }
                //Clipboard.Clear();

                // DateTime now = DateTime.Now;
                string today = String.Format(DateTime.Now.ToString("yyyy/MM/dd_HHmmss"));

                //Console.WriteLine(result);
                string f_path = @"c:\temp\[" + userDept00 + "]" + "입고현황_" + today + ".csv";
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

    }//class
    public class MultipleTextFormatConverterKey : IMultiValueConverter  
    {
       
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format((string)parameter, values);
        
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
             return null;
        }
    }
}
