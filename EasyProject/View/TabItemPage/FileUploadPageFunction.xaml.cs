using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls;
using System.Windows.Data;
using EasyProject.ViewModel;
using EasyProject.Model;
using System.Collections.Generic;
using log4net;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// FileUploadPageFunction.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FileUploadPageFunction : PageFunction<String>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        private OpenFileDialog openFileDialog;

        public FileUploadPageFunction(OpenFileDialog openFileDialog)
        {
            log.Info("Constructor FileUploadPageFunction() invoked.");
            InitializeComponent();
            //this.DataContext = new ProductViewModel();

            fileUploadBtn.Click += fileUploadBtn_Click;

            this.openFileDialog = openFileDialog;

            ((ProductViewModel)(this.DataContext)).OpenFileDialog = openFileDialog.FileName;
            SetFileNameTxtBlock();
        }//FileUploadPageFunction

        private string GetFileName(OpenFileDialog openFileDialog)
        {
            log.Info(" GetFileName(OpenFileDialog) invoked.");
            try
            {
                return System.IO.Path.GetFileName(openFileDialog.FileName);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            
        }
        private void SetFileNameTxtBlock()
        {
            log.Info("SetFileNameTxtBlock() invoked.");

            try
            {
                fileNameTxtbox.Text = GetFileName(this.openFileDialog);
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//SetFileNameTxtBlock

        private void fileUploadBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("fileUploadBtn_Click(object, RoutedEventArgs) invoked.");

            try
            {
                //이전 페이지로 돌아가기 (PageFunction 객체 생성한 페이지)
                OnReturn(new ReturnEventArgs<string>());
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//fileUploadBtn_Click
    }//class
}//namespace
