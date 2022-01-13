﻿using System;
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
using System.Text.RegularExpressions;
using log4net;

namespace EasyProject.View.TabItemPage
{
    /// <summary>
    /// InsertPage_Form.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertPage_Form : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public InsertPage_Form()
        {
            log.Info("Constructor InsertPage_Excel() invoked.");
            InitializeComponent();
        }

        // 가격 텍스트박스의 입력값이 변경되었을 때 값이 반영되기전에 들어오는 이벤트 
        // 따라서 정규식 검사 클래스인 Regex를 이용하여 숫자일 때에만 수정이 가능하게 구현.
        private void productPrice_TxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            log.Info("productPrice_TxtBox_PreviewTextInput(object, TextCompositionEventArgs) invoked.");
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//productPrice_TxtBox_PreviewTextInput()

        // 수량 텍스트박스의 입력값이 변경되었을 때 값이 반영되기전에 들어오는 이벤트 
        // 따라서 정규식 검사 클래스인 Regex를 이용하여 숫자일 때에만 수정이 가능하게 구현.
        private void productQuantity_TxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            log.Info("productQuantity_TxtBox_PreviewTextInput(object, TextCompositionEventArgs) invoked.");
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//productQuantity_TxtBox_PreviewTextInput
    }
}
