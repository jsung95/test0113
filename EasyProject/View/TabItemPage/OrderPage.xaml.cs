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
    /// OrderPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderPage : Page
    {
        private int index = 1;
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public OrderPage()
        {
            log.Info("Constructor OrderPage() invoked.");
            InitializeComponent();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("resetBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                memo_TxtBox.Text = null;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            prodcode_TxtBox.Text = null;
            prodname_TxtBox.Text = null;
            categoryname_TxtBox.Text = null;
            prodprice_TxtBox.Text = null;
            prodcount_TxtBox.Text = null;



        }

        //인쇄 버튼
        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("printBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                try
                {
                    printBtn.Visibility = Visibility.Hidden;
                    pdfBtn.Visibility = Visibility.Hidden;

                    PrintDialog printDialog = new PrintDialog();
                    if (printDialog.ShowDialog().GetValueOrDefault(false))
                    {
                        printDialog.PrintVisual(NewPlaceOrder, "PlaceOrder");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    printBtn.Visibility = Visibility.Visible;
                    pdfBtn.Visibility = Visibility.Visible;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            

        }

        //pdf 버튼
        private void pdfBtn_Click(object sender, RoutedEventArgs e)
        {
            log.Info("pdfBtn_Click(object, RoutedEventArgs) invoked.");
            try
            {
                try
                {
                    printBtn.Visibility = Visibility.Hidden;
                    pdfBtn.Visibility = Visibility.Hidden;

                    //이미지로 저장(스크린 샷)
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)NewPlaceOrder.ActualWidth, (int)NewPlaceOrder.ActualHeight, 74, 74, PixelFormats.Pbgra32);
                    rtb.Render(NewPlaceOrder);
                    PngBitmapEncoder png = new PngBitmapEncoder();
                    png.Frames.Add(BitmapFrame.Create(rtb));
                    MemoryStream stream = new MemoryStream();
                    png.Save(stream);

                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    string stampFileName = @"C:\Users\user\Desktop\" + $"신규발주신청서{index}.png";
                    image.Save(stampFileName);




                    //sharpPDF이용해서 넣기
                    PdfDocument document = new PdfDocument();

                    // Create an empty page
                    PdfPage page = document.AddPage();

                    // Get an XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Create a font
                    XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

                    XImage im = XImage.FromFile(@"C:\Users\user\Desktop\" + $"신규발주신청서{index}.png");

                    gfx.DrawImage(im, -150, 100, 700, 450);





                    // Save the document...
                    string filename = @"C:\Users\user\Desktop\" + $"신규발주신청서{index}.pdf";
                    document.Save(filename);


                    var temp = Ioc.Default.GetService<ProductShowViewModel>();
                    temp.IsInOutEnabled = false;
                    temp.MessageQueue.Enqueue($"신규발주신청서{index}.pdf 생성", "닫기", (x) => { temp.IsInOutEnabled = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                    
                    index++;
                    Process.Start(filename);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    printBtn.Visibility = Visibility.Visible;
                    pdfBtn.Visibility = Visibility.Visible;


                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
            
       

        }


    }


}
