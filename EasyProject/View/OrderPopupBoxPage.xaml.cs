using EasyProject.ViewModel;
using log4net;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyProject.View
{
    /// <summary>
    /// OrderPopupBoxPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderPopupBoxPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        private int index = 1;

        public OrderPopupBoxPage()
        {
            InitializeComponent();
            log.Info("Constructor OrderPopupBoxPage() invoked.");

        }
        public void pdfBtn_Click(object e, RoutedEventArgs arg)
        {
            log.Info("pdfBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                printBtn.Visibility = Visibility.Hidden;
                pdfBtn.Visibility = Visibility.Hidden;
                cancel_Btn2.Visibility = Visibility.Hidden;
                resetBtn.Visibility = Visibility.Hidden;

                //이미지로 저장(스크린 샷)
                RenderTargetBitmap rtb = new RenderTargetBitmap(900, 1000, 186, 186, PixelFormats.Pbgra32);
                rtb.Render(PlaceOrder);
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                MemoryStream stream = new MemoryStream();
                png.Save(stream);

                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                string stampFileName = @"C:\Users\user\Desktop\" + $"발주신청서{index}.png";
                image.Save(stampFileName);




                //sharpPDF이용해서 넣기
                PdfDocument document = new PdfDocument();

                // Create an empty page
                PdfPage page = document.AddPage();

                // Get an XGraphics object for drawing
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Create a font
                XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

                XImage im = XImage.FromFile(@"C:\Users\user\Desktop\" + $"발주신청서{index}.png");

                gfx.DrawImage(im, 0, 100, 700, 800);




                // Save the document...
                string filename = @"C:\Users\user\Desktop\" + $"발주신청서{index}.pdf";
                document.Save(filename);

                var temp = Ioc.Default.GetService<ProductShowViewModel>();
                temp.IsInOutEnabled = false;
                temp.MessageQueue.Enqueue($"발주신청서{index}.pdf 생성", "닫기", (x) => { temp.IsInOutEnabled = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                index++;
                Process.Start(filename);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                printBtn.Visibility = Visibility.Visible;
                pdfBtn.Visibility = Visibility.Visible;
                cancel_Btn2.Visibility = Visibility.Visible;
                resetBtn.Visibility = Visibility.Visible;
            }

        }

        //초기화버튼
        public void resetBtn_Click(object e, RoutedEventArgs arg)
        {
            log.Info("resetBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {     
                prodprice_Textbox.Text = null;
                memo_TxtBox.Text = null;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
          
        }

        //인쇄버튼
        private void PrintBtn(object sender, RoutedEventArgs e)
        {
            log.Info("PrintBtn(object, RoutedEventArgs) invoked.");
            try
            {
                printBtn.Visibility = Visibility.Hidden;
                pdfBtn.Visibility = Visibility.Hidden;
                cancel_Btn2.Visibility = Visibility.Hidden;
                resetBtn.Visibility = Visibility.Hidden;

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog().GetValueOrDefault(false))
                {
                    printDialog.PrintVisual(this, this.Title);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                printBtn.Visibility = Visibility.Visible;
                pdfBtn.Visibility = Visibility.Visible;
                cancel_Btn2.Visibility = Visibility.Visible;
                resetBtn.Visibility = Visibility.Visible;
            }


        }

      
    }

}
