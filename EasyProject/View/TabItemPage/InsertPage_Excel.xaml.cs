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
using System.IO;
using Microsoft.Win32;
using EasyProject.ViewModel;
using Excel = Microsoft.Office.Interop.Excel;
using log4net;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// InsertPage_Excel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertPage_Excel : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public InsertPage_Excel()
        {
            log.Info("Constructor InsertPage_Excel() invoked.");
            InitializeComponent();

            //fileUploadBtn.Click += fileUploadBtn_Click;
            //fileDownLoadBtn.Click += fileDownLoadBtn_Click;
        }

        private void fileDownLoadBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("fileDownLoadBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {    
                string f_path = @"C:\Users\user\Documents\GitHub\easy_project\EasyProject\ExcelFile\재고입력폼.xlsx";

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
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }

        private void fileUploadBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("fileUploadBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "csv 파일 (*.csv)|*.csv|엑셀 파일 (*.xls)|*.xls|엑셀 파일 (*.xlsx)|*.xlsx";
                openFileDialog.Filter = "csv 파일 (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == true)
                {

                    //MessageBox.Show(System.IO.Path.GetFullPath(openFileDialog.FileName));
                    FileUploadPageFunction uploadPFunction = new FileUploadPageFunction(openFileDialog);
                    NavigationService.Navigate(uploadPFunction);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
    }
}
