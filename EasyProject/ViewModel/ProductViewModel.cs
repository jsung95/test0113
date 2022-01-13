using EasyProject.Dao;
using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.IO;
using System.Text;
using log4net;
using MaterialDesignThemes.Wpf;

namespace EasyProject.ViewModel
{
    public class ProductViewModel : ObservableObject
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        ProductDao dao = new ProductDao();
        CategoryDao categoryDao = new CategoryDao();


        //카테고리 목록을 담을 프로퍼티
        public ObservableCollection<CategoryModel> Categories { get; set; }

        //재고 입력 데이터를 담을 프로퍼티
        public ProductModel Product { get; set; }
       

        //로그인한 간호자(사용자) 정보를 담을 프로퍼티
        public NurseModel Nurse { get; set; }

        //입력한 재고 데이터를 담은 객체를 담아줄 옵저버블컬렉션 리스트
        public ObservableCollection<ProductInOutModel> Add_list { get; set; }  
        //public ObservableCollection<ProductInOutModel> Add_list { 
        //    get
        //    {
        //        return add_list;    
        //    }
        //    set
        //    {
        //        SetProperty(ref add_list, value);
        //    }
        //         }

        public List<ProductInOutModel> productDtoList { get; set; }

        private List<ProductShowModel> excelProductList;

        private DateTime? startDate;
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                if (StartDate > EndDate)
                {
                    StartDate = EndDate.Value.AddDays(-1);
                }
                dateChanged();
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime? endDate;
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                if (StartDate > EndDate)
                {
                    StartDate = EndDate.Value.AddDays(-1);
                }
                dateChanged();
                OnPropertyChanged("EndDate");
            }
        }
        public void dateChanged()
        {
            log.Info("dateChanged() invoked.");
            try
            {
                if (Add_list != null)
                {
                    Add_list.Clear();
                    var temp = dao.GetProductInByNurse(Nurse, StartDate, EndDate);
                    foreach (var item in temp)
                    {
                        Add_list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }                      
        }


        public ProductViewModel()
        {
            log.Info("Constructor ProductViewModel() invoked.");

            //스넥바 Duration 3초로 설정 TimeSpan.FromMilliseconds(3000)
            messagequeue = new SnackbarMessageQueue();

            Product = new ProductModel()
            {
                Prod_expire = DateTime.Now
            };
            List<CategoryModel> list = dao.GetCategoryModels();
            Categories = new ObservableCollection<CategoryModel>(list); // List타입 객체 list를 OC 타입 Products에 넣음                                                           
            CategoryModel categoryDao = new CategoryModel();
            categoryDao.Category_id = null;
            categoryDao.Category_name = "추가(직접입력)";
            Categories.Add(categoryDao);

            //App.xaml.cs 에 로그인할 때 바인딩 된 로그인 정보 객체
            Nurse = App.nurse_dto;
            //재고등록페이지 날짜
            //날짜 컨트롤 부서별 해당 최소 날짜 및 최대 날짜로 초기화
            StartDate = Convert.ToDateTime(dao.GetProductIn_MinDate(Nurse));
            if (StartDate.Value.Year == 1)
            {
                StartDate = DateTime.Today.AddDays(-1); // MinDate가 0001 01 01 인 경우 전날로 설정
            }
            EndDate = DateTime.Today;
            //현재 로그인 사용자의 입고 목록을 가져옴
            Add_list = dao.GetProductInByNurse(Nurse, StartDate, EndDate);
            
            excelProductList = new List<ProductShowModel>();
           
        }
        
        //재고추가 다이얼로그
        private bool isInsertDialogHostOpen;
        public bool IsInsertDialogHostOpen
        {
            get
            {
                return isInsertDialogHostOpen;
            }

            set
            {
                this.isInsertDialogHostOpen = value;
                OnPropertyChanged("IsInsertDialogHostOpen");
            }
        }


        #region 스넥바

        //스넥바 메세지큐
        private SnackbarMessageQueue messagequeue;
        public SnackbarMessageQueue MessageQueue
        {
            get { return messagequeue; }
            set
            {
                messagequeue = value;
                OnPropertyChanged("MessageQueue");
            }
        }


        private bool isDuplicatedProduct = false;
        public bool IsDuplicatedProduct
        {
            get { return isDuplicatedProduct; }
            set
            {
                isDuplicatedProduct = value;
                OnPropertyChanged("IsDuplicatedProduct");
            }
        }


        private ActionCommand snackBarCommand;
        public ICommand SnackBarCommand
        {
            get
            {
                if (snackBarCommand == null)
                {
                    snackBarCommand = new ActionCommand(CloseSnackBar);
                }
                return snackBarCommand;
            }//get

        }//ListCommand

        private void CloseSnackBar()
        {
            log.Info("CloseSnackBar() invoked.");
            try
            {
                IsDuplicatedProduct = false;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
        #endregion

        #region CSV/엑셀

        private string openFileDialog;
        public string OpenFileDialog
        {
            get
            {
                return openFileDialog;
            }

            set
            {
                this.openFileDialog = value;
                OnPropertyChanged("OpenFileDialog");
            }
        }

        private ActionCommand listCommand;
        public ICommand ListCommand
        {
            get
            {
                if (listCommand == null)
                {
                    listCommand = new ActionCommand(FileReader);
                }
                return listCommand;
            }//get

        }//ListCommand

        private void FileReader()
        {
            log.Info("FileReader() invoked.");
            try
            {
                string fileExtension = Path.GetExtension(openFileDialog);

                if (fileExtension == ".csv")
                {
                    CsvReader();
                }
                else
                {
                    ExcelReader();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        //csv로 입력받은 여러개의 제품들에 대한 처리 
        private void CsvReader()
        {
            log.Info("CsvReader() invoked.");
            try
            {
                //StreamReader sr = new StreamReader(openFileDialog, Encoding.GetEncoding("euc-kr"));
                StreamReader sr = new StreamReader(openFileDialog, UnicodeEncoding.UTF8);
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] temp = s.Split(',');

                    var product = new ProductShowModel();
                    var productModel = new ProductModel();

                    for (int cCnt = 0; cCnt <= 5; cCnt++)
                    {
                        if(temp[cCnt] != null)
                        {
                            product = SetProductObjectForCsv(ref product, cCnt, temp[cCnt]);


                            Console.WriteLine(product.Category_name + "////카테고리명////");
                        }
                        
                    }//for


                    productModel.Prod_code = product.Prod_code;
                    productModel.Prod_name = product.Prod_name;
                    productModel.Category_id = categoryDao.GetCategoryID(product.Category_name);
                    productModel.Prod_expire = product.Prod_expire;
                    productModel.Prod_price = product.Prod_price;


                    if (!dao.IsProductDuplicateCheck(productModel, (int)App.nurse_dto.Dept_id))
                    {

                        Console.WriteLine("중복이 아니다. ");

                    }//if
                    else
                    {
                        IsDuplicatedProduct = true;
                        Console.WriteLine("중복이다.");

                        excelProductList = new List<ProductShowModel>();//reset
                    }//else

                    if (!IsDuplicatedProduct)
                    {
                        excelProductList.Add(product);
                    }//if

                }//while

                if (isDuplicatedProduct)
                {
                    IsInsertDialogHostOpen = false;
                    MessageQueue.Enqueue("이미 존재하는 재고 입력은 불가합니다.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                }

                if (excelProductList.Count > 0)
                {
                    foreach (ProductShowModel elem in excelProductList)
                    {
                        IsDuplicatedProduct = false;

                        //MessageBox.Show(product.Prod_code+".."+product.Prod_name+
                        //   ".." + product.Category_name+".."+product.Prod_expire
                        //   + ".." + product.Prod_price + ".." + product.Prod_total);

                        dao.AddProductForExcel(elem, elem.Category_name);
                        dao.StoredProductForExcel(elem, Nurse);
                        dao.AddImpDeptForExcel(elem, Nurse);

                        // 현재 사용자가 추가 입고 내역을 담을 임시 객체
                        ProductInOutModel productDto = new ProductInOutModel();

                        // 새로 입고 시 Add_list(사용자의 입고 내역 목록) 업데이트
                        productDto.Prod_in_date = DateTime.Now;
                        productDto.Prod_code = elem.Prod_code;
                        productDto.Prod_name = elem.Prod_name;
                        productDto.Category_name = elem.Category_name;
                        productDto.Prod_expire = elem.Prod_expire;
                        productDto.Prod_price = elem.Prod_price;
                        productDto.Prod_in_count = elem.Prod_total;
                        productDto.Nurse_name = Nurse.Nurse_name;

                        //productDtoList.Insert(0, productDto);
                        Add_list.Insert(0, productDto);
                        //Add_list.Add(productDto);
                        Console.WriteLine(Add_list.Count + "개");

                        var temp1 = Ioc.Default.GetService<ProductShowViewModel>();
                        temp1.getListbyDept();  // 재고현황 리스트 갱신

                        var temp2 = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp2.getInListByDept(); // 입고 목록 갱신
                    }//foreach

                    IsInsertDialogHostOpen = false;
                    MessageQueue.Enqueue("신규 재고가 추가되었습니다.", "닫기", (x) => { IsDuplicatedProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                }//if

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);

                IsDuplicatedProduct = true;
                IsInsertDialogHostOpen = false;
                MessageQueue.Enqueue("업로드 할 파일을 닫고 다시 시도하십시오.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
            }//catch

        }//CsvReader



        //excel로 입력받은 여러개의 제품들에 대한 처리 
        private void ExcelReader()
        {
            log.Info("ExcelReader() invoked.");
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                try
                {
                    int rCnt = 0; // 열 갯수
                    int cCnt = 0; // 행 갯수

                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(openFileDialog);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1); // 첫번째 시트를 가져 옴.

                    range = xlWorkSheet.UsedRange; // 가져 온 시트의 데이터 범위 값

                    for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
                    {
                        var product = new ProductShowModel();
                        for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                        {
                            product = SetProductObject(ref product, ref range, rCnt, cCnt);

                            ProductModel productModel = new ProductModel();
                            productModel.Prod_code = product.Prod_code;
                            productModel.Prod_name = product.Prod_name;
                            productModel.Category_id = categoryDao.GetCategoryID(product.Category_name);
                            productModel.Prod_expire = product.Prod_expire;
                            productModel.Prod_price = product.Prod_price;

                            if (!dao.IsProductDuplicateCheck(productModel, (int)App.nurse_dto.Dept_id))
                            {

                                Console.WriteLine("중복이 아니다. ");

                            }
                            else
                            {
                                IsDuplicatedProduct = true;
                                Console.WriteLine("중복이다.");

                                excelProductList.Clear();
                            }
                        }
                        if (!IsDuplicatedProduct)
                        {
                            excelProductList.Add(product);
                        }
                    }

                    if (isDuplicatedProduct)
                    {
                        IsInsertDialogHostOpen = false;
                        MessageQueue.Enqueue("이미 존재하는 재고 입력은 불가합니다.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                    }

                    if (excelProductList.Count > 0)
                    {
                        foreach (ProductShowModel elem in excelProductList)
                        {
                            IsDuplicatedProduct = false;
                            IsInsertDialogHostOpen = false;
                            MessageQueue.Enqueue("신규 재고가 추가되었습니다.", "닫기", (x) => { IsDuplicatedProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                            //MessageBox.Show(product.Prod_code+".."+product.Prod_name+
                            //   ".." + product.Category_name+".."+product.Prod_expire
                            //   + ".." + product.Prod_price + ".." + product.Prod_total);

                            dao.AddProductForExcel(elem, elem.Category_name);
                            dao.StoredProductForExcel(elem, Nurse);
                            dao.AddImpDeptForExcel(elem, Nurse);

                            // 현재 사용자가 추가 입고 내역을 담을 임시 객체
                            ProductInOutModel productDto = new ProductInOutModel();

                            // 새로 입고 시 Add_list(사용자의 입고 내역 목록) 업데이트
                            productDto.Prod_in_date = DateTime.Now;
                            productDto.Prod_code = elem.Prod_code;
                            productDto.Prod_name = elem.Prod_name;
                            productDto.Category_name = elem.Category_name;
                            productDto.Prod_expire = elem.Prod_expire;
                            productDto.Prod_price = elem.Prod_price;
                            productDto.Prod_in_count = elem.Prod_total;
                            productDto.Nurse_name = Nurse.Nurse_name;

                            //productDtoList.Insert(0, productDto);
                            Add_list.Insert(0, productDto);
                            //Add_list.Add(productDto);
                            Console.WriteLine(Add_list.Count + "개");

                            var temp1 = Ioc.Default.GetService<ProductShowViewModel>();
                            temp1.getListbyDept();  // 재고현황 리스트 갱신

                            var temp2 = Ioc.Default.GetService<ProductInOutViewModel>();
                            temp2.getInListByDept(); // 입고 목록 갱신
                        }

                        //var ob2list = Add_list.ToList();
                        //ob2list.AddRange(productDtoList);
                        //Add_list = new ObservableCollection<ProductInOutModel>(ob2list);

                        xlWorkBook.Close(true, null, null);
                        xlApp.Quit();

                        releaseObject(xlWorkSheet);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);


                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    IsDuplicatedProduct = true;
                    IsInsertDialogHostOpen = false;
                    MessageQueue.Enqueue("업로드 할 파일을 닫고 다시 시도하십시오.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                }
            }
            catch(Exception ex1)
            {
                log.Error(ex1.Message);
            }
            


        }
        private ProductShowModel SetProductObjectForCsv(ref ProductShowModel Product, int columnNum, string columnText)
        {
            log.Info("SetProductObjectForCsv(ref ProductShowModel, int, string) invoked.");
            try
            {
                switch (columnNum)
                {
                    case 0:
                        Product.Prod_code = (string)(columnText);
                        break;

                    case 1:
                        Product.Prod_name = (string)(columnText);
                        break;

                    case 2:
                        Product.Category_name = (string)(columnText);
                        break;

                    case 3:
                        Product.Prod_expire = Convert.ToDateTime(columnText.Substring(0, 10));
                        break;

                    case 4:
                        Product.Prod_price = Int32.Parse(columnText);
                        break;

                    case 5:
                        Product.Prod_total = Int32.Parse(columnText);
                        break;

                }
                return Product;

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            
        }

        private ProductShowModel SetProductObject(ref ProductShowModel Product, ref Excel.Range range, int rCnt, int cCnt)
        {
            log.Info("SetProductObject(ref ProductShowModel, ref Excel.Range, int, int) invoked.");
            try
            {
                string headerText = range.Cells[1, cCnt].Text.ToString();

                switch (headerText)
                {
                    case "제품코드":
                        Product.Prod_code = (string)GetCellText(range, rCnt, 1);
                        break;

                    case "제품명":
                        Product.Prod_name = (string)GetCellText(range, rCnt, 2);
                        break;

                    case "품목/종류":
                        Product.Category_name = (string)GetCellText(range, rCnt, 3);
                        break;

                    case "유통기한":
                        Product.Prod_expire = Convert.ToDateTime(GetCellText(range, rCnt, 4).Substring(0, 10));
                        break;

                    case "가격":
                        Product.Prod_price = Int32.Parse(GetCellText(range, rCnt, 5));
                        break;

                    case "수량":
                        Product.Prod_total = Int32.Parse(GetCellText(range, rCnt, 6));
                        break;

                }
                return Product;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            
        }
        private string GetCellText(Excel.Range range, int rCnt, int cCnt)
        {
            log.Info("GetCellText(Excel.Range, int, int) invoked.");
            try
            {
                string cellText = range.Cells[rCnt, cCnt].Text.ToString();
                Console.WriteLine(cellText + "엑셀에서 읽어온 데이터");
                return cellText;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            
        }

        private void releaseObject(object obj)
        {
            log.Info("releaseObject(object) invoked.");
            try
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
                catch (Exception ex)
                {
                    obj = null;
                    Console.WriteLine("releaseObject : " + ex);
                }
                finally
                {
                    GC.Collect();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
        #endregion

        #region 재고입력

        //제품 가격, 수량을 임시로 담을 프로퍼티
        private string input_Prod_price;
        public string Input_Prod_price
        {
            get { return input_Prod_price; }
            set { input_Prod_price = value; OnPropertyChanged("Input_Prod_price"); }
        }
        private string input_Prod_total;
        public string Input_Prod_total
        {
            get { return input_Prod_total; }
            set { input_Prod_total = value; OnPropertyChanged("Input_Prod_total"); }
        }


        //선택한 카테고리를 담을 프로퍼티
        private CategoryModel selectedCategory;
        public CategoryModel SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        //카테고리 추가 - 직접입력한 카테고리 TextBox.Text 값
        private string addCategoryName;
        public string AddCategoryName
        {
            get { return addCategoryName; }
            set
            {
                addCategoryName = value;
                OnPropertyChanged("AddCategoryName");
            }
        }

        private ActionCommand command;
        public ICommand Command
        {
            get
            {
                if (command == null)
                {
                    command = new ActionCommand(ProductInsert);
                }
                return command;
            }//get

        }//Command

        public void ProductInsert()
        {
            log.Info("ProductInsert() invoked.");
            try
            {
                // 만약 제품입력이 하나라도 안되었다면
                if (Product.Prod_code == null || Product.Prod_code.Equals("") || Product.Prod_name == null || Product.Prod_name.Equals("") || SelectedCategory == null || Product.Prod_expire == null || Product.Prod_expire.Equals("") || Input_Prod_price == null || Input_Prod_price.Equals("") || Input_Prod_total == null || Input_Prod_total.Equals(""))
                {
                    //스넥바 메세지 출력
                    IsDuplicatedProduct = true;
                    MessageQueue.Enqueue("입력할 제품의 정보를 모두 기입해주세요.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                }//if
                else //제품입력이 모두 되었지만
                {
                    Product.Prod_price = Int32.Parse(Input_Prod_price);
                    Product.Prod_total = Int32.Parse(Input_Prod_total);

                    if (SelectedCategory.Category_name.Equals("추가(직접입력)")) //만약 카테고리 선택을 직접입력으로 선택했다면
                    {
                        if (AddCategoryName == null || AddCategoryName.Equals("")) //만약 직접입력란이 비어있다면
                        {
                            IsDuplicatedProduct = true;
                            MessageQueue.Enqueue("추가할 카테고리명을 입력해주세요.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        }//if
                        else //만약 직접입력라인 비어있지 않고 입력했다면
                        {
                            if (categoryDao.IsExistsCategory(AddCategoryName)) //만약 기존 카테고리에 이미 존재한다면
                            {
                                IsDuplicatedProduct = true;
                                MessageQueue.Enqueue("이미 존재하는 카테고리명 입니다.", "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                            }//if
                            else // 만약 기존 카테고리에 존재하지 않아서 추가할수 있다면
                            {
                                if (!dao.IsProductDuplicateCheck(Product, AddCategoryName)) //만약 입력하려는 정보가 기존 제품과 중복되지 않는다면
                                {
                                    categoryDao.AddCategory(AddCategoryName);
                                    IsDuplicatedProduct = false;
                                    MessageQueue.Enqueue("신규 재고가 추가되었습니다. (카테고리가 추가되었습니다.)", "닫기", (x) => { IsDuplicatedProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                                    //재고입력
                                    dao.AddProduct(Product, AddCategoryName);

                                    //입고테이블에 추가
                                    dao.StoredProduct(Product, Nurse);

                                    //IMP_DEPT 테이블에 추가
                                    dao.AddImpDept(Product, Nurse);

                                    // 현재 사용자가 추가 입고 내역을 담을 임시 객체
                                    ProductInOutModel dto = new ProductInOutModel();

                                    // 새로 입고 시 Add_list(사용자의 입고 내역 목록) 업데이트
                                    dto.Prod_in_date = DateTime.Now;
                                    dto.Prod_code = Product.Prod_code;
                                    dto.Prod_name = Product.Prod_name;
                                    dto.Category_name = AddCategoryName;
                                    dto.Prod_expire = Product.Prod_expire;
                                    dto.Prod_price = Product.Prod_price;
                                    dto.Prod_in_count = Product.Prod_total;
                                    dto.Nurse_name = Nurse.Nurse_name;

                                    Add_list.Insert(0, dto);

                                    var temp1 = Ioc.Default.GetService<ProductShowViewModel>();
                                    temp1.getListbyDept();  // 재고현황 리스트 갱신
                                    temp1.Categories = new ObservableCollection<CategoryModel>(categoryDao.GetCategories());

                                    ObservableCollection<CategoryModel> list = new ObservableCollection<CategoryModel>(categoryDao.GetCategories());
                                    Categories.Clear();
                                    foreach (var item in list)
                                    {
                                        Categories.Add(item);
                                    }

                                    CategoryModel newCategoryDao = new CategoryModel();
                                    newCategoryDao.Category_id = null;
                                    newCategoryDao.Category_name = "추가(직접입력)";
                                    Categories.Add(newCategoryDao);

                                    ObservableCollection<CategoryModel> newCategories = new ObservableCollection<CategoryModel>(categoryDao.GetCategoriesvalues());
                                    temp1.Category1.Clear();
                                    foreach (var item in list)
                                    {
                                        temp1.Category1.Add(item);
                                    }
                                    temp1.SelectedCategory1 = temp1.Category1[0];

                                    var temp2 = Ioc.Default.GetService<ProductInOutViewModel>();
                                    temp2.getInListByDept(); // 입고 목록 갱신

                                    ResetForm();
                                }//if
                                else //만약 입력하려는 정보가 기존 제품과 중복된다면
                                {
                                    IsDuplicatedProduct = true;
                                    string message = Product.Prod_code + "(" + Product.Prod_name + ") / " +
                                        AddCategoryName + "/" + Product.Prod_expire + "/" + Product.Prod_price + "는 이미 존재하여 재고 입력이 불가합니다.";
                                    MessageQueue.Enqueue(message, "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                                    Console.WriteLine(message);

                                }//else
                            }//else
                        }//else
                    }//if
                    else //콤보박스에서 기존에 있는 카테고리를 선택했을 때
                    {
                        //Product.Category_id = categoryDao.GetCategoryID(SelectedCategory.Category_name);

                        if (!dao.IsProductDuplicateCheck(Product, SelectedCategory))
                        {
                            IsDuplicatedProduct = false;
                            MessageQueue.Enqueue("신규 재고가 추가되었습니다.", "닫기", (x) => { IsDuplicatedProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                            //재고입력
                            dao.AddProduct(Product, SelectedCategory);

                            //입고테이블에 추가
                            dao.StoredProduct(Product, Nurse);

                            //IMP_DEPT 테이블에 추가
                            dao.AddImpDept(Product, Nurse);

                            // 현재 사용자가 추가 입고 내역을 담을 임시 객체
                            ProductInOutModel dto = new ProductInOutModel();

                            // 새로 입고 시 Add_list(사용자의 입고 내역 목록) 업데이트
                            dto.Prod_in_date = DateTime.Now;
                            dto.Prod_code = Product.Prod_code;
                            dto.Prod_name = Product.Prod_name;
                            if (AddCategoryName == null || AddCategoryName.Equals(""))
                            {
                                dto.Category_name = SelectedCategory.Category_name;
                            }
                            else
                            {
                                dto.Category_name = AddCategoryName;
                            }
                            dto.Prod_expire = Product.Prod_expire;
                            dto.Prod_price = Product.Prod_price;
                            dto.Prod_in_count = Product.Prod_total;
                            dto.Nurse_name = Nurse.Nurse_name;

                            Add_list.Insert(0, dto);

                            var temp1 = Ioc.Default.GetService<ProductShowViewModel>();
                            temp1.getListbyDept();  // 재고현황 리스트 갱신

                            var temp2 = Ioc.Default.GetService<ProductInOutViewModel>();
                            temp2.getInListByDept(); // 입고 목록 갱신

                            ResetForm();
                        }//if
                        else
                        {
                            IsDuplicatedProduct = true;
                            string message = Product.Prod_code + "(" + Product.Prod_name + ") / " +
                                SelectedCategory.Category_name + "/" + Product.Prod_expire + "/" + Product.Prod_price + "는 이미 존재하여 재고 입력이 불가합니다.";
                            MessageQueue.Enqueue(message, "닫기", (x) => { IsDuplicatedProduct = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                            Console.WriteLine(message);

                        }//else
                    }//else


                }//else
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
           
        }// ProductInsert
        #endregion

        #region 초기화 버튼
        private ActionCommand resetCommand; // 초기화 버튼(입력 폼 비우기)
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new ActionCommand(ResetForm);
                }
                return resetCommand;
            }//get

        }//ResetCommand
        public void ResetForm()
        {
            log.Info("ResetForm() invoked.");
            try
            {
                Product.Prod_expire = DateTime.Now;
                Product.Prod_name = null;
                Product.Prod_price = null;
                Input_Prod_price = null;
                Product.Prod_total = null;
                Input_Prod_total = null;
                Product.Prod_code = null;
                SelectedCategory = null;
                AddCategoryName = null;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
        #endregion

    }//class

}//namespace
