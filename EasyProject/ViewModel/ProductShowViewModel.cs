using EasyProject.Dao;
using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using LiveCharts.Configurations;
using System.Windows.Media;
using SolidColorBrush = Xamarin.Forms.SolidColorBrush;
using Color = Xamarin.Forms.Color;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using log4net;

namespace EasyProject.ViewModel
{
    public class ProductShowViewModel : Notifier
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        DeptDao dept_dao = new DeptDao();
        ProductDao product_dao = new ProductDao();
        CategoryDao category_dao = new CategoryDao();
        UsersDao user_dao = new UsersDao();

        //로그인한 간호자(사용자) 정보를 담을 프로퍼티
        public NurseModel Nurse { get; set; }

        private CategoryModel selectedModifyCategory;
        public CategoryModel SelectedModifyCategory
        {
            get { return selectedModifyCategory; }
            set
            {
                selectedModifyCategory = value;
                OnPropertyChanged("SelectedModifyCategory");
            }
        }

        private int selectedProductIndex;
        public int SelectedProductIndex
        {
            get { return selectedProductIndex; }
            set
            {
                selectedProductIndex = value;
                OnPropertyChanged("SelectedProductIndex");
            }
        }

        private string comboBoxCategoryName;
        public string ComboBoxCategoryName
        {
            get { return comboBoxCategoryName; }
            set
            {
                comboBoxCategoryName = value;
                OnPropertyChanged("ComboBoxCategoryName");
            }
        }

        private bool isToolTipChecked = true;
        public bool IsToolTipChecked
        {
            get { return isToolTipChecked; }
            set
            {
                isToolTipChecked = value;
                OnPropertyChanged("IsToolTipChecked");
            }
        }





        public ICollectionView EmployeeCollection { get; private set; }

        public ProductShowViewModel()
        {
            log.Info("Constructor ProductShowViewModel() invoked.");
            messagequeue = new SnackbarMessageQueue();

            Products = new ObservableCollection<ProductShowModel>();
            SearchTypeList = new[] { "제품코드", "제품명", "품목/종류" };
            SelectedSearchType = SearchTypeList[0];
            TextForSearch = ""; // 검색어 초기값을 null이 아닌 ""으로 설정

            Depts = new ObservableCollection<DeptModel>(dept_dao.GetDepts()); // 우측 상단 제품 현황 목록의 부서 선택 콤보박스
            SelectedDept = Depts[(int)App.nurse_dto.Dept_id - 1]; // 위 콤보박스의 초기값 = 현재 사용자의 부서로 설정
            
            
            getListbyDept();
            //updateSearchedProducts(true);

            DeptsForPopupBox = new ObservableCollection<DeptModel>(dept_dao.GetDepts()); // 출고 팝업의 부서 선택 콤보박스
            DeptsForPopupBox.RemoveAt((int)App.nurse_dto.Dept_id - 1); // 현재 사용자의 부서는 목록에서 제거
            Categories = new ObservableCollection<CategoryModel>(category_dao.GetCategories());                                                       

            //App.xaml.cs 에 로그인할 때 바인딩 된 로그인 정보 객체
            Nurse = App.nurse_dto;
            //SelectedProductList = new List<ProductShowModel>();
         
            SelectedUser = user_dao.GetUserInfoWithDept(Nurse);

            //EmployeeCollection = CollectionViewSource.GetDefaultView(LstEmPloyeeDetail);
            //employeeEntities = new EmployeeEntities();
            //employeeContext = new EmployeeContext();

            //LoadEmployee();
            //UpdateRecordCount();
            //EmployeeCollection.Filter = FilterByName;


            //11대시보드
            Category1 = new ObservableCollection<CategoryModel>(category_dao.GetCategoriesvalues());
            SelectedCategory1 = Category1[0];
            DecidedNumber = new[] { 10, 20, 30 };
            SelectedNumber = DecidedNumber[0];
            DashboardPrint1(SelectedDept, SelectedCategory1, SelectedNumber);

            //파이차트
            Depts_Pie = new ObservableCollection<DeptModel>(dept_dao.GetDepts());   //dept_od를 가져온다
            SelectedDept_Pie = Depts_Pie[(int)App.nurse_dto.Dept_id - 1];  // 
            /*ProductInout_Pie = new ObservableCollection<ProductInOutModel>(product_dao.GetProdOutType());
            
            SelectedOutType_Pie = ProductInout_Pie[0];
*/
            //오늘날짜 생성
            testdate = DateTime.Now.Year.ToString() + "년 " + DateTime.Now.Month.ToString() + "월 " + DateTime.Now.Day.ToString() + "일";

            

        }//Constructor

        #region 대시보드
        //대시보드 목록 콤보박스
        public ObservableCollection<CategoryModel> Category1 { get; set; }

        //대시보드 목록 카테고리 프로퍼티
        private CategoryModel selectedCategory1;
        public CategoryModel SelectedCategory1
        {
            get { return selectedCategory1; }
            set
            {
                selectedCategory1 = value;
                OnPropertyChanged("SelectedCategory1");
                //DashboardPrint11(selectedDept11, selectedCategory11);

            }
        }
        // DashboardPrint() 그래프
        private SeriesCollection seriesCollection1;
        public SeriesCollection SeriesCollection1               //그래프 큰 틀 만드는거
        {
            get { return seriesCollection1; }
            set
            {
                seriesCollection1 = value;
                OnPropertyChanged("SeriesCollection1");
            }
        }

        private SeriesCollection seriesCollection2;
        public SeriesCollection SeriesCollection2               //부서별 카테고리//제품총수량 그래프 큰 틀 
        {
            get { return seriesCollection2; }
            set
            {
                seriesCollection2 = value;
                OnPropertyChanged("SeriesCollection2");
            }
        }

        // 대시보드 프로퍼티
        public ChartValues<int> Values { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> BarLabels { get; set; }       //string[]
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> Formatter1 { get; set; }

        // 대시보드에 표시할 갯수 프로퍼티
        public int[] DecidedNumber { get; set; }

        //대시보드 동적 프로퍼티
        private ChartValues<int> values1;
        public ChartValues<int> Values1
        {
            get { return values1; }
            set
            {
                values1 = value;
                OnPropertyChanged("values1");

            }
        }
        private List<string> barLabels1;
        public List<string> BarLabels1
        {
            get { return barLabels1; }
            set
            {
                barLabels1 = value;
                OnPropertyChanged("barLabels1");

            }
        }
        public List<string> barLabels2 { get; set; }
        public List<string> BarLabels2
        {
            get { return barLabels2; }
            set
            {
                barLabels2 = value;
                OnPropertyChanged("barLabels2");

            }
        }
        //대시보드 출력 갯수
        private int selectedNumber;
        public int SelectedNumber
        {
            get { return selectedNumber; }
            set
            {
                selectedNumber = value;
                OnPropertyChanged("selectedNumber");
            }
        }

        public void DashboardPrint1(DeptModel selected_dept, CategoryModel selected_category, int selected_number)                       //대시보드 출력(x축:제품code, y축:수량) 
        {
            log.Info("DashboardPrint1(DeptModel, CategoryModel, int) invoked.");
            try
            {
                var Mapper = Mappers.Xy<int>()
          .X((value, index) => value)
          .Y((value, index) => index)

          .Fill((value, index) => {
              Console.WriteLine("dfassdfa");

              if (value < 0)
              {
                  return Brushes.Red;
              }
              else if (((value > 0) && (value < 3))
               || (value == 0))
              {
                  return Brushes.Yellow;
              }
              else
              {
                  return Brushes.Green;
              }

          });

                //Console.WriteLine("dsdfsdffsdf");
                //Console.WriteLine(value.Prod_remainexpire);

                var seriesCollection = new SeriesCollection(Mapper);
                Console.WriteLine(Mapper);

                ChartValues<int> name = new ChartValues<int>();   //y축들어갈 임시 값

                Console.WriteLine("DashboardPrint11");
                SeriesCollection1 = seriesCollection;   //대시보드 틀
                                                        //Console.WriteLine(selected.Dept_id); 
                List<ProductShowModel> list_xyz = product_dao.Get_Dept_Category_Number_RemainExpire(selected_dept, selected_category, selected_number);
                Console.WriteLine(selected_dept.Dept_name);
                Console.WriteLine(selected_category.Category_name);
                foreach (var item in list_xyz)
                {
                    name.Add((int)item.Prod_remainexpire);
                    Console.WriteLine("PROD_REMAINEXPIRE" + (int)item.Prod_remainexpire);
                }

                Values = new ChartValues<int> { };

                SeriesCollection1.Add(new RowSeries
                {
                    Title = "남은 유통기한",   //+ i
                    Values = name,
                    DataLabels = true,
                    LabelPoint = point => point.X + "일 ",
                    Configuration = Mapper,
                });

                BarLabels1 = new List<string>() { };                           //x축출력
                foreach (var item in list_xyz)
                {
                    BarLabels1.Add(item.Prod_code);
                    Console.WriteLine("Prod_code" + item.Prod_code);
                }
                Formatter1 = value => value.ToString("N0");   //문자열 10진수 변환
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
           
        }//dashboardprint1



        private ActionCommand command;
        public ICommand Command
        {
            get
            {
                if (command == null)
                {
                    command = new ActionCommand(DeptComboBoxChanged);
                }
                return command;
            }//get

        }//Command

        public void DeptComboBoxChanged()
        {
            log.Info("DeptComboBoxChanged() invoked.");
            try
            {
                getListbyDept();
                DashboardPrint1(SelectedDept, SelectedCategory1, SelectedNumber);
                DashboardPrint2(selectedDept);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
         
        }

        //부서별   카테고리//제품총수량 그래프
        public void DashboardPrint2(DeptModel selected)                       //대시보드 출력(x축:제품code, y축:수량) 
        {
            log.Info("DashboardPrint2(DeptModel) invoked.");
            try
            {
                ChartValues<int> mount = new ChartValues<int>();   //y축들어갈 임시 값
                Console.WriteLine("DashboardPrint2");
                SeriesCollection2 = new SeriesCollection();   //대시보드 틀
                                                              //Console.WriteLine(selected.Dept_id); 
                List<ImpDeptModel> list_xy = product_dao.Dept_Category_Mount(selected);
                Console.WriteLine(selected);
                //부서id별 제품code와 수량리스트
                //List<string> list_x = new List<string>();                                    //x축리스트
                //ChartValues<int> list_y = new ChartValues<int>();                          //y축리스트
                //foreach (var item in list_xy)
                //{
                //    list_x.Add((string)item.Prod_code);
                //    list_y.Add((int)item.Prod_total);
                //}
                //name을 2개선언 리스트

                //List<ProductShowModel> list1 = list_y;      //y축출력
                //List<ProductShowModel> list1 = product_dao.Prodtotal_Info();     
                foreach (var item in list_xy)
                {
                    mount.Add((int)item.Imp_dept_count);
                }
                //for (int i = 0; i < 8; i++)
                //{
                //    name.Add((int)list_xy[i].Prod_total);
                //}
                Values = new ChartValues<int> { };

                SeriesCollection2.Add(new ColumnSeries
                {
                    Title = "총 수량",   //+ i
                    Values = mount,
                });
                BarLabels2 = new List<string>() { };                           //x축출력
                foreach (var item in list_xy)
                {
                    BarLabels2.Add(item.Category_name);
                }
                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//dashboardprint2
        #endregion

        

        #region 팝업 - 발주신청
        // 발주 신청 페이지 바인딩
        private UserModel selectedUser;
        public UserModel SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        //재고현황페이지에서 발주팝업박스 텍스트초기화 커맨드
        private ActionCommand orderPopupReset;
        public ICommand OrderPopupReset
        {
            get
            {
                if (orderPopupReset == null)
                {
                    Console.WriteLine("리셋!");
                    orderPopupReset = new ActionCommand(OrderFormReset);
                }
                return orderPopupReset;
            }//get
        }

        public void OrderFormReset()
        {
            log.Info("OrderFormReset() invoked.");
            try
            {
                //SelectedUser.Nurse_name = null;
                //SelectedUser.Dept_name = null;
                //SelectedUser.Dept_phone = null;
                //SelectedProduct.Prod_name = null;
                SelectedProduct.Mount = null;
                SelectedProduct.Volume = null;
                SelectedProduct.Manufacturer = null;
                SelectedProduct.OrderMemo = null;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//OrderFormReset


        #endregion

        #region 팝업 - 출고
        private ActionCommand outProductCommand; //출고확인 버튼 커맨드
        public ICommand OutProductCommand
        {
            get
            {
                if (outProductCommand == null)
                {
                    outProductCommand = new ActionCommand(OutProduct);
                }
                return outProductCommand;
            }//get
        }//OutProductCommand

        public void OutProduct()
        {
            log.Info("OutProduct() invoked.");
            try
            {
                //Console.WriteLine("OutProduct() 실행!");
                if (SelectedProduct.SelectedOutType == "사용")  // 출고타입이 사용일 때  *출고 타입을 선택하지 않은 경우는 확인 버튼이 비활성화되어 있음
                {
                    if (SelectedProduct.InputOutCount == null) // null 입력할 경우
                    {
                        MessageQueue.Enqueue("제품 사용 수량을 올바르게 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount <= 0) //0 or 음수를 입력할 경우
                    {
                        MessageQueue.Enqueue("제품 사용 수량에는 0 보다 큰 수량을 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount > SelectedProduct.Imp_dept_count) // 현재 재고 수량보다 많은 숫자를 입력할 경우
                    {
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}의 현재 수량이 {SelectedProduct.InputOutCount}보다 적습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount == SelectedProduct.Imp_dept_count) // 현재 재고 수량을 모두 사용할 경우
                    {
                        product_dao.OutProduct(SelectedProduct, Nurse);
                        product_dao.ChangeProductInfo_IMP_DEPT_ForOut(SelectedProduct);
                        product_dao.ChangeProductInfo_ForOut(SelectedProduct);
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) 모두 사용하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;

                        LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                        updateSearchedProducts(false);
                        UpdateRecordCount();

                        var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                        temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                    }
                    else
                    {
                        product_dao.OutProduct(SelectedProduct, Nurse);
                        product_dao.ChangeProductInfo_IMP_DEPT_ForOut(SelectedProduct);
                        product_dao.ChangeProductInfo_ForOut(SelectedProduct);
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) {SelectedProduct.InputOutCount}개 사용하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;

                        LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                        updateSearchedProducts(false);
                        UpdateRecordCount();

                        var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                        temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                    }

                }//else if
                else if (SelectedProduct.SelectedOutType == "이관") // 출고타입이 이관일 때  *출고 타입을 선택하지 않은 경우는 확인 버튼이 비활성화되어 있음
                {
                    if (SelectedProduct.InputOutCount == null) // null 입력할 경우
                    {
                        MessageQueue.Enqueue("제품 이관 수량을 올바르게 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.SelectedOutDept == null) // 부서 선택하지 않을 경우
                    {
                        MessageQueue.Enqueue("제품을 이관할 부서를 선택해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount <= 0) //0 or 음수를 입력할 경우
                    {
                        MessageQueue.Enqueue("제품 이관 수량에는 0 보다 큰 수량을 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount > SelectedProduct.Imp_dept_count) // 현재 재고 수량보다 많은 숫자를 입력할 경우
                    {
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}의 현재 수량이 {SelectedProduct.InputOutCount}보다 적습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount == SelectedProduct.Imp_dept_count) // 현재 재고 수량을 모두 이관할 경우
                    {
                        product_dao.OutProduct(SelectedProduct, Nurse);
                        product_dao.ChangeProductInfo_IMP_DEPT_ForOut(SelectedProduct);
                        product_dao.ChangeProductInfo_ForOut(SelectedProduct);
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) 모두 {SelectedProduct.SelectedOutDept.Dept_name} 부서로 이관하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;

                        LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                        updateSearchedProducts(false);
                        UpdateRecordCount();

                        var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                        temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                    }
                    else
                    {
                        product_dao.OutProduct(SelectedProduct, Nurse);
                        product_dao.ChangeProductInfo_IMP_DEPT_ForOut(SelectedProduct);
                        product_dao.ChangeProductInfo_ForOut(SelectedProduct);
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) {SelectedProduct.InputOutCount}개 {SelectedProduct.SelectedOutDept.Dept_name} 부서로 이관하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;

                        LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                        updateSearchedProducts(false);
                        UpdateRecordCount();

                        var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                        temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                    }

                }//else if
                else if (SelectedProduct.SelectedOutType == "폐기")  // 출고타입이 폐기일 때  *출고 타입을 선택하지 않은 경우는 확인 버튼이 비활성화되어 있음
                {

                    if (SelectedProduct.InputOutCount == null) // null 입력할 경우
                    {
                        MessageQueue.Enqueue("제품 폐기 수량을 올바르게 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount <= 0) //0 or 음수를 입력할 경우
                    {
                        MessageQueue.Enqueue("제품 폐기 수량에는 0 보다 큰 수량을 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount > SelectedProduct.Imp_dept_count) // 현재 재고 수량보다 많은 숫자를 입력할 경우
                    {
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}의 현재 수량이 {SelectedProduct.InputOutCount}보다 적습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;
                    }
                    else if (SelectedProduct.InputOutCount == SelectedProduct.Imp_dept_count) // 현재 재고 수량을 모두 폐기할 경우
                    {
                        product_dao.OutProduct(SelectedProduct, Nurse);
                        product_dao.ChangeProductInfo_IMP_DEPT_ForOut(SelectedProduct);
                        product_dao.ChangeProductInfo_ForOut(SelectedProduct);
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) 모두 폐기하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;

                        LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                        updateSearchedProducts(false);
                        UpdateRecordCount();

                        var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                        temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                    }
                    else
                    {
                        product_dao.OutProduct(SelectedProduct, Nurse);
                        product_dao.ChangeProductInfo_IMP_DEPT_ForOut(SelectedProduct);
                        product_dao.ChangeProductInfo_ForOut(SelectedProduct);
                        MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) {SelectedProduct.InputOutCount}개 폐기하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                        IsInOutEnabled = true;

                        LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                        updateSearchedProducts(false);
                        UpdateRecordCount();

                        var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                        temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                        temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                    }

                }//else if

                if (SelectedProduct != null)
                {
                    SelectedProduct.SelectedOutType = null;
                    SelectedProduct.SelectedOutDept = null;
                    SelectedProduct.InputOutCount = null;
                }

                DashboardPrint2(selectedDept);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
           
        }//OutProduct

        private ActionCommand outProductReset;
        public ICommand OutProductReset
        {
            get
            {
                if (outProductReset == null)
                {
                    outProductReset = new ActionCommand(OutProductFormReset);
                }
                return outProductReset;
            }//get
        }

        public void OutProductFormReset()
        {
            log.Info("OutProductFormReset() invoked.");
            try
            {
                SelectedProduct.SelectedOutType = null;
                SelectedProduct.SelectedOutDept = null;
                SelectedProduct.InputOutCount = null;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//OutProductFormReset
        #endregion

        #region 팝업 -입고
        private ActionCommand inProductCommand; //입고확인 버튼 커맨드
        public ICommand InProductCommand
        {
            get
            {
                if (inProductCommand == null)
                {
                    inProductCommand = new ActionCommand(InProduct);
                }
                return inProductCommand;
            }//get
        }

        public void InProduct() // 팝업박스 - 추가입고
        {
            log.Info("InProduct() invoked.");
            try
            {
                //Console.WriteLine("InProduct() 실행!");
                if (SelectedProduct.InputInCount == null)
                {
                    MessageQueue.Enqueue("제품 추가 입고 수량을 제대로 입력해주세요.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                    IsInOutEnabled = true;
                }
                else if (SelectedProduct.InputInCount <= 0)
                {
                    MessageQueue.Enqueue("제품 추가 입고 수량은 0보다 커야합니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                    IsInOutEnabled = true;
                }
                else // 수량 추가 성공
                {
                    product_dao.InProduct(SelectedProduct, Nurse);
                    product_dao.ChangeProductInfo_IMP_DEPT_ForIn(SelectedProduct);
                    product_dao.ChangeProductInfo_ForIn(SelectedProduct);
                    MessageQueue.Enqueue($"{SelectedProduct.Prod_name}을(를) {SelectedProduct.InputInCount}개 추가 입고하였습니다.", "닫기", (x) => { IsEmptyProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                    IsInOutEnabled = true;

                    LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                    updateSearchedProducts(false);
                    UpdateRecordCount();
                    var temp = Ioc.Default.GetService<ProductInOutViewModel>();
                    temp.getOutListByDept(); // 입출고현황 페이지 출고목록 갱신
                    temp.getInListByDept(); // 입출고현황 페이지 출고목록 갱신
                }

                if (SelectedProduct != null)
                {
                    SelectedProduct.InputInCount = null;
                }

                DashboardPrint2(selectedDept);          //입고변경에 따른 그래프적용(다시 그려줌);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }//InProduct

        private ActionCommand inProductReset;
        public ICommand InProductReset
        {
            get
            {
                if (inProductReset == null)
                {
                    inProductReset = new ActionCommand(InProductFormReset);
                }
                return inProductReset;
            }//get
        }

        public void InProductFormReset()
        {
            log.Info("InProductFormReset() invoked.");
            try
            {
                SelectedProduct.InputInCount = null;
            }

            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//InProductFormReset
        #endregion

        #region 제품수정
        private ActionCommand productEditCommand;
        public ICommand ProductEditCommand
        {
            get
            {
                if (productEditCommand == null)
                {
                    productEditCommand = new ActionCommand(EditProduct);
                }
                return productEditCommand;
            }//get
        }

        public void EditProduct()
        {
            log.Info("EditProduct() invoked.");
            try
            {
                product_dao.ChangeProductInfo(SelectedProduct);
                product_dao.ChangeProductInfo_IMP_DEPT(SelectedProduct);
                //ObservableCollection<CategoryModel> list = new ObservableCollection<CategoryModel>(category_dao.GetCategories());
                //Categories.Clear();
                //foreach (var item in list)
                //{
                //    Categories.Add(item);
                //}

                //CategoryModel newCategoryDao = new CategoryModel();
                //var cateGoryId = category_dao.GetCategoryID(SelectedProduct.Category_name);
                //newCategoryDao.Category_id = cateGoryId;
                //newCategoryDao.Category_name = SelectedProduct.Category_name;
                //Categories.Insert(0,newCategoryDao);

                //첫페이지 말고 다음 페이지에서 재고수정할때 성공은 되는데 첫페이지로 돌아감 해결하기
                LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                updateSearchedProducts(false);
                UpdateRecordCount();

                DashboardPrint2(selectedDept);

                //DialogHost.ShowDialog(null);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }//EditProduct
        #endregion

        #region  Snackbar
        // ==================== 스넥바 snackbar =======================
        //============================================================

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

        private bool isEmptyProduct = false;
        public bool IsEmptyProduct
        {
            get { return isEmptyProduct; }
            set
            {
                isEmptyProduct = value;
                OnPropertyChanged("IsEmptyProduct");
            }
        }
        private bool isInOutEnabled = false;          // 재고출입고
        public bool IsInOutEnabled
        {
            get { return isInOutEnabled; }
            set
            {
                isInOutEnabled = value;
                OnPropertyChanged("IsInOutEnabled");
            }
        }

        private bool isEditButtonClicked = false;
        public bool IsEditButtonClicked
        {
            get { return isEditButtonClicked; }
            set
            {
                isEditButtonClicked = value;
                OnPropertyChanged("IsEditButtonClicked");
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

        }//SnackBarCommand

        private void CloseSnackBar()
        {
            log.Info("CloseSnackBar() invoked.");
            try
            {
                //IsEmptyProduct = false;
                IsInOutEnabled = false;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//CloseSnackBar

        //============================================================
        //============================================================
        #endregion

        #region 검색/콤보박스/리스트
        //화면에 보여줄 재고목록에 해당하는 옵저버블컬렉션 프로퍼티
        private ObservableCollection<ProductShowModel> products;
        public ObservableCollection<ProductShowModel> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged("Products");
            }
        }

        public IEnumerable<ProductShowModel> searchedProducts { get; set; } // 검색결과에 해당하는 객체들을 임시로 담아놓을 프로퍼티

        //부서 목록 콤보박스, 부서 리스트 출력
        public ObservableCollection<DeptModel> Depts { get; set; }
        public ObservableCollection<DeptModel> DeptsForPopupBox { get; set; }

        private bool comboboxChanged = false;
        public bool ComboboxChanged
        {
            get { return comboboxChanged; }
            set
            {
                comboboxChanged = value;
                OnPropertyChanged("ComboboxChanged");
            }
        }

        //카테고리 목록 콤보박스, 카테고리 목록 출력
        public ObservableCollection<CategoryModel> Categories { get; set; }



        //선택한 부서를 담을 프로퍼티
        private DeptModel selectedDept;
        public DeptModel SelectedDept
        {
            get { return selectedDept; }
            set
            {
                selectedDept = value;
                OnPropertyChanged("SelectedDept");
                //DashboardPrint2(selectedDept);
            }
        }

        //선택한 카테고리명을 담을 프로퍼티
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

        //검색 유형 프로퍼티
        public string[] SearchTypeList { get; set; }
        //선택한 검색 유형 콤보박스를 담을 프로퍼티
        public string SelectedSearchType { get; set; }


        //입력한 검색내용을 담을 프로퍼티
        private string textForSearch;
        public string TextForSearch
        {
            get { return textForSearch; }
            set
            {
                textForSearch = value;
                OnPropertyChanged("TextForSearch");
            }
        }

        //선택한 1개의 제품 정보를 담을 객체
        private ProductShowModel selectedProduct;
        public ProductShowModel SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                Console.WriteLine("selectedProduct set!!");
                //SelectedProductList.Clear(); // 이전에 담은 SelectedProduct를 리스트에서 지운다.
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
                //Message.Send(SelectedProducts);
                /*                Console.WriteLine("==선택한 재고 정보==");
                                Console.WriteLine($"  Prod_code : {SelectedProduct.Prod_code}");
                                Console.WriteLine($"  Prod_name : {SelectedProduct.Prod_name}");
                                Console.WriteLine($"  Category_name : {SelectedProduct.Category_name}");
                                Console.WriteLine($"  Prod_price : {SelectedProduct.Prod_price}");
                                Console.WriteLine($"  Imp_dept_count : {SelectedProduct.Imp_dept_count}");
                                Console.WriteLine($"  Prod_expire : {SelectedProduct.Prod_expire}");
                                Console.WriteLine($"  Prod_id : {SelectedProduct.Prod_id}");
                                Console.WriteLine($"  Imp_dept_id : {SelectedProduct.Imp_dept_id}");*/
                //SelectedProductList.Add(selectedProduct);
                //Console.WriteLine(SelectedProductList[0].Prod_code);
            }
        }

        

        private ActionCommand searchKeywordCommand;
        public ICommand SearchKeywordCommand
        {
            get
            {
                if (searchKeywordCommand == null)
                {
                    searchKeywordCommand = new ActionCommand(searchKeywordChanged);
                }
                return searchKeywordCommand;
            }//get
        }
        public void searchKeywordChanged()
        {
            log.Info("searchKeywordChanged() invoked.");
            try
            {
                updateSearchedProducts(true);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//searchKeywordChanged
        public void updateSearchedProducts(bool isNotUpdated)
       {
            log.Info("updateSearchedProducts(bool) invoked.");
            try
            {
                //LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.SearchProducts(SelectedDept, SelectedSearchType, TextForSearch));
                //UpdateCollection(LstOfRecords.Take(SelectedRecord));
                //UpdateRecordCount();

                Console.WriteLine("updateSearchedProducts() 검색어 : " + TextForSearch);
                if (isNotUpdated)
                {
                    CurrentPage = 1; // 재고 수정/입고/출고등이 일어나지 않은 단순 검색어 변경일 경우 검색할 때마다 1페이지로 이동
                }

                if (TextForSearch != null) // 키워드 있을 때
                {
                    if (SelectedSearchType == "제품명")
                    {
                        searchedProducts = LstOfRecords.Where(model => model.Prod_name.Contains(TextForSearch) || model.Prod_name.Contains(TextForSearch.ToUpper()) || model.Prod_name.Contains(TextForSearch.ToLower()));
                        Console.WriteLine("제품명 : " + searchedProducts.Count() + TextForSearch.ToUpper());

                        UpdateRecordCount();

                    }
                    else if (SelectedSearchType == "제품코드")
                    {
                        searchedProducts = LstOfRecords.Where(model => model.Prod_code.Contains(TextForSearch) || model.Prod_code.Contains(TextForSearch.ToUpper()) || model.Prod_code.Contains(TextForSearch.ToLower()));
                        Console.WriteLine("제품코드 : " + searchedProducts.Count());

                        UpdateRecordCount();
                    }
                    else // 품목/종류
                    {
                        searchedProducts = LstOfRecords.Where(model => model.Category_name.Contains(TextForSearch) || model.Category_name.Contains(TextForSearch.ToUpper()) || model.Category_name.Contains(TextForSearch.ToLower()));
                        Console.WriteLine("품목/종류 : " + searchedProducts.Count());

                        UpdateRecordCount();
                    }

                }//if
                else // 키워드 없을 때
                {
                    searchedProducts = LstOfRecords;
                    Console.WriteLine("검색어없음: " + searchedProducts.Count());

                    UpdateRecordCount();

                }//else
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }


        }//updateSearchedProducts


        #endregion

        #region Pagination
        private ActionCommand nextCommand;
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new ActionCommand(NextPage);
                }
                return nextCommand;
            }//get
        }



        private ActionCommand firstCommand;
        public ICommand FirstCommand
        {
            get
            {
                if (firstCommand == null)
                {
                    firstCommand = new ActionCommand(FirstPage);
                }
                return firstCommand;
            }//get
        }

        private ActionCommand lastCommand;
        public ICommand LastCommand
        {
            get
            {
                if (lastCommand == null)
                {
                    lastCommand = new ActionCommand(LastPage);
                }
                return lastCommand;
            }//get
        }

        private ActionCommand previouCommand;
        public ICommand PreviousCommand
        {
            get
            {
                if (previouCommand == null)
                {
                    previouCommand = new ActionCommand(PreviousPage);
                }
                return previouCommand;
            }//get
        }

        //*****************************************************************************
        //*****************************************************************************
        //여기서부터 paginaion 추가한 코드 내용
        public ObservableCollection<ProductShowModel> LstOfRecords { get; set; }
        public void getListbyDept()
        {
            log.Info("getListbyDept() invoked.");
            try
            {
                //LoadEmployee();
                //LstOfRecords.Add(empDetails);

                LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));
                updateSearchedProducts(true);
                UpdateRecordCount();
                //searchKeywordChanged();
                //RecordStartFrom = (CurrentPage - 1) * SelectedRecord;
                //var recordsToShow = LstOfRecords.Skip(RecordStartFrom).Take(SelectedRecord);

                //UpdateCollection(recordsToShow); // SelectedRecord만큼 잘라서 UpdateCollection에 넣음
                //UpdateRecordCount();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }//getListbyDept

        int RecordStartFrom = 0;
        private void PreviousPage(object obj)
        {
            log.Info("PreviousPage(object) invoked.");
            try
            {
                CurrentPage--;
                //RecordStartFrom = searchedProducts.Count() - SelectedRecord * (NumberOfPages - (CurrentPage - 1));
                RecordStartFrom = (CurrentPage - 1) * SelectedRecord;
                var recorsToShow = searchedProducts.Skip(RecordStartFrom).Take(SelectedRecord);
                UpdateCollection(recorsToShow);
                UpdateEnableState();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//PreviousPage

        private void LastPage(object obj)
        {
            log.Info("LastPage(object) invoked.");
            try
            {
                //30 1->20, 2->10
                //last page - 21 -30
                //11-30=>30-20=10 -> 11-30

                //fix
                //20*(2-1)=20
                //skip = 20
                var recordsToskip = SelectedRecord * (NumberOfPages - 1);
                UpdateCollection(searchedProducts.Skip(recordsToskip));
                CurrentPage = NumberOfPages;
                //MessageBox.Show(CurrentPage + "페이지");
                UpdateEnableState();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//LastPage

        private void FirstPage(object obj)
        {
            log.Info("FirstPage(object) invoked.");
            try
            {
                UpdateCollection(searchedProducts.Take(SelectedRecord));
                CurrentPage = 1;
                UpdateEnableState();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//FirstPage

        private void NextPage(object obj)
        {
            log.Info("NextPage(object) invoked.");
            try
            {
                RecordStartFrom = CurrentPage * SelectedRecord;
                var recordsToShow = searchedProducts.Skip(RecordStartFrom).Take(SelectedRecord);
                UpdateCollection(recordsToShow);
                CurrentPage++;
                UpdateEnableState();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
           
        }//NextPage

        private void UpdateCollection(IEnumerable<ProductShowModel> enumerable)
        {
            log.Info("UpdateCollection(IEnumerable<ProductShowModel>) invoked.");
            try
            {
                if (Products != null)
                {
                    Products.Clear();
                }

                foreach (var item in enumerable)
                {
                    Products.Add(item);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//UpdateCollection

        private int _currentPage = 1;

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdateEnableState();
            }
        }
        private void UpdateEnableState()
        {
            log.Info("UpdateEnableState() invoked.");
            try
            {
                IsFirstEnabled = CurrentPage > 1;
                IsPreviousEnabled = CurrentPage > 1;
                IsNextEnabled = CurrentPage < NumberOfPages;
                IsLastEnabled = CurrentPage < NumberOfPages;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//UpdateEnableState

        private int _numberOfPages = 10;

        public int NumberOfPages
        {
            get { return _numberOfPages; }
            set
            {
                _numberOfPages = value;
                OnPropertyChanged(nameof(NumberOfPages));
                UpdateEnableState();
            }
        }
        private bool _isFirstEnabled;

        public bool IsFirstEnabled
        {
            get { return _isFirstEnabled; }
            set
            {
                _isFirstEnabled = value;
                OnPropertyChanged(nameof(IsFirstEnabled));
            }
        }

        private bool _isPreviousEnabled;

        public bool IsPreviousEnabled
        {
            get { return _isPreviousEnabled; }
            set
            {
                _isPreviousEnabled = value;
                OnPropertyChanged(nameof(IsPreviousEnabled));
            }
        }
        private bool _isLastEnabled;

        public bool IsLastEnabled
        {
            get { return _isLastEnabled; }
            set
            {
                _isLastEnabled = value;
                OnPropertyChanged(nameof(IsLastEnabled));
            }
        }

        private bool _isNextEnabled;

        public bool IsNextEnabled
        {
            get { return _isNextEnabled; }
            set
            {
                _isNextEnabled = value;
                OnPropertyChanged(nameof(IsNextEnabled));
            }
        }

        private int _selectedRecord = 10;

        public int SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
                UpdateRecordCount();
            }
        }
        private void UpdateRecordCount()
        {
            log.Info("UpdateRecordCount() invoked.");
            try
            {
                NumberOfPages = (int)Math.Ceiling((double)searchedProducts.Count() / SelectedRecord);
                NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;

                RecordStartFrom = (CurrentPage - 1) * SelectedRecord;
                var recordsToShow = searchedProducts.Skip(RecordStartFrom).Take(SelectedRecord);

                UpdateCollection(recordsToShow);
                //CurrentPage = 1;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }//UpdateRecordCount
        #endregion

        #region Pagination (현재미사용)
        //private ActionCommand nextCommand;
        //public ICommand NextCommand
        //{
        //    get
        //    {
        //        if (nextCommand == null)
        //        {
        //            nextCommand = new ActionCommand(NextPage);
        //        }
        //        return nextCommand;
        //    }//get
        //}



        //private ActionCommand firstCommand;
        //public ICommand FirstCommand
        //{
        //    get
        //    {
        //        if (firstCommand == null)
        //        {
        //            firstCommand = new ActionCommand(FirstPage);
        //        }
        //        return firstCommand;
        //    }//get
        //}

        //private ActionCommand lastCommand;
        //public ICommand LastCommand
        //{
        //    get
        //    {
        //        if (lastCommand == null)
        //        {
        //            lastCommand = new ActionCommand(LastPage);
        //        }
        //        return lastCommand;
        //    }//get
        //}

        //private ActionCommand previouCommand;
        //public ICommand PreviousCommand
        //{
        //    get
        //    {
        //        if (previouCommand == null)
        //        {
        //            previouCommand = new ActionCommand(PreviousPage);
        //        }
        //        return previouCommand;
        //    }//get
        //}

        ////*****************************************************************************
        ////*****************************************************************************
        ////여기서부터 paginaion 추가한 코드 내용
        //private ObservableCollection<ProductShowModel> LstOfRecords;
        //public void showListbyDept()
        //{
        //    //LoadEmployee();
        //    //LstOfRecords.Add(empDetails);

        //    LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

        //    RecordStartFrom = (CurrentPage - 1) * SelectedRecord;
        //    var recordsToShow = LstOfRecords.Skip(RecordStartFrom).Take(SelectedRecord);

        //    UpdateCollection(recordsToShow); // SelectedRecord만큼 잘라서 UpdateCollection에 넣음
        //    UpdateRecordCount();
        //}

        //int RecordStartFrom = 0;
        //private void PreviousPage(object obj)
        //{
        //    CurrentPage--;
        //    RecordStartFrom = LstOfRecords.Count - SelectedRecord * (NumberOfPages - (CurrentPage - 1));
        //    var recorsToShow = LstOfRecords.Skip(RecordStartFrom).Take(SelectedRecord);
        //    UpdateCollection(recorsToShow);
        //    UpdateEnableState();

        //}
        //private void LastPage(object obj)
        //{
        //    //30 1->20, 2->10
        //    //last page - 21 -30
        //    //11-30=>30-20=10 -> 11-30

        //    //fix
        //    //20*(2-1)=20
        //    //skip = 20
        //    var recordsToskip = SelectedRecord * (NumberOfPages - 1);
        //    UpdateCollection(LstOfRecords.Skip(recordsToskip));
        //    CurrentPage = NumberOfPages;
        //    //MessageBox.Show(CurrentPage + "페이지");
        //    UpdateEnableState();
        //}
        //private void FirstPage(object obj)
        //{
        //    UpdateCollection(LstOfRecords.Take(SelectedRecord));
        //    CurrentPage = 1;
        //    UpdateEnableState();
        //}
        //private void NextPage(object obj)
        //{
        //    RecordStartFrom = CurrentPage * SelectedRecord;
        //    var recordsToShow = LstOfRecords.Skip(RecordStartFrom).Take(SelectedRecord);
        //    UpdateCollection(recordsToShow);
        //    CurrentPage++;
        //    UpdateEnableState();
        //}

        //private void UpdateCollection(IEnumerable<ProductShowModel> enumerable)
        //{
        //    if (Products != null)
        //    {
        //        Products.Clear();
        //    }

        //    foreach (var item in enumerable)
        //    {
        //        Products.Add(item);
        //    }
        //}
        //private int _currentPage = 1;

        //public int CurrentPage
        //{
        //    get { return _currentPage; }
        //    set
        //    {
        //        _currentPage = value;
        //        OnPropertyChanged(nameof(CurrentPage));
        //        UpdateEnableState();
        //    }
        //}
        //private void UpdateEnableState()
        //{
        //    IsFirstEnabled = CurrentPage > 1;
        //    IsPreviousEnabled = CurrentPage > 1;
        //    IsNextEnabled = CurrentPage < NumberOfPages;
        //    IsLastEnabled = CurrentPage < NumberOfPages;
        //}

        //private int _numberOfPages = 10;

        //public int NumberOfPages
        //{
        //    get { return _numberOfPages; }
        //    set
        //    {
        //        _numberOfPages = value;
        //        OnPropertyChanged(nameof(NumberOfPages));
        //        UpdateEnableState();
        //    }
        //}
        //private bool _isFirstEnabled;

        //public bool IsFirstEnabled
        //{
        //    get { return _isFirstEnabled; }
        //    set
        //    {
        //        _isFirstEnabled = value;
        //        OnPropertyChanged(nameof(IsFirstEnabled));
        //    }
        //}

        //private bool _isPreviousEnabled;

        //public bool IsPreviousEnabled
        //{
        //    get { return _isPreviousEnabled; }
        //    set
        //    {
        //        _isPreviousEnabled = value;
        //        OnPropertyChanged(nameof(IsPreviousEnabled));
        //    }
        //}
        //private bool _isLastEnabled;

        //public bool IsLastEnabled
        //{
        //    get { return _isLastEnabled; }
        //    set
        //    {
        //        _isLastEnabled = value;
        //        OnPropertyChanged(nameof(IsLastEnabled));
        //    }
        //}

        //private bool _isNextEnabled;

        //public bool IsNextEnabled
        //{
        //    get { return _isNextEnabled; }
        //    set
        //    {
        //        _isNextEnabled = value;
        //        OnPropertyChanged(nameof(IsNextEnabled));
        //    }
        //}

        //private int _selectedRecord = 10;

        //public int SelectedRecord
        //{
        //    get { return _selectedRecord; }
        //    set
        //    {
        //        _selectedRecord = value;
        //        OnPropertyChanged(nameof(SelectedRecord));
        //        UpdateRecordCount();
        //    }
        //}
        //private void UpdateRecordCount()
        //{
        //    NumberOfPages = (int)Math.Ceiling((double)LstOfRecords.Count / SelectedRecord);
        //    NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;

        //    RecordStartFrom = (CurrentPage - 1) * SelectedRecord;
        //    var recordsToShow = LstOfRecords.Skip(RecordStartFrom).Take(SelectedRecord);

        //    UpdateCollection(recordsToShow);
        //    //CurrentPage = 1;
        //}
        #endregion
        #region ExportPage (현재 사용X)
        //public List<ProductShowModel> SelectedProductList { get; set; } // SelectedProduct를 DataGrid에서 사용하기 위한 List

        /*private ActionCommand command;
        public ICommand Command
        {
            get
            {
                if (command == null)
                {
                    command = new ActionCommand(GetProductsByDept);
                }
                return command;
            }//get
        }

        public void GetProductsByDept()
        {
            Products = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

            LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

            UpdateCollection(LstOfRecords.Take(SelectedRecord));
            UpdateRecordCount();

            ComboboxChanged = true;
        }*/



        /*        private ActionCommand changeProductCommand;
                public ICommand ChangeProductCommand
                {
                    get
                    {
                        if (changeProductCommand == null)
                        {
                            changeProductCommand = new ActionCommand(ChangeProductInfo);
                        }
                        return changeProductCommand;
                    }//get
                }*/

        public void ChangeProductInfo() //재고수정 확인 버튼 클릭 시에 동작하는 메소드
        {
            product_dao.ChangeProductInfo(SelectedProduct);
            product_dao.ChangeProductInfo_IMP_DEPT(SelectedProduct);
            Products = new ObservableCollection<ProductShowModel>(product_dao.GetProducts());
            //LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.SearchProducts(SelectedDept, SelectedSearchType, TextForSearch));
            //UpdateCollection(LstOfRecords.Take(SelectedRecord));
            //UpdateRecordCount();

        }

        public void AddNewCategory(string AddCategoryName)
        {
            if (category_dao.IsExistsCategory(AddCategoryName)) //만약 기존 카테고리에 이미 존재한다면
            {
                //MessageQueue.Enqueue("이미 존재하는 카테고리명 입니다.", "닫기", (x) => { IsDuplicatedProduct = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
                //IsDuplicatedProduct = true;
            }
            else
            {
                category_dao.AddCategory(AddCategoryName);
            }
        }

        private ActionCommand modifyProductReset;
        public ICommand ModifyProductReset
        {
            get
            {
                if (modifyProductReset == null)
                {
                    modifyProductReset = new ActionCommand(modifyProductResetClick);
                }
                return modifyProductReset;
            }//get
        }

        public void modifyProductResetClick()
        {
            SelectedCategory = null;
        }
        #endregion
        #region 검색버튼(현재 미사용)
        private ActionCommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new ActionCommand(SearchProducts);
                }
                return searchCommand;
            }//get
        }

        public void SearchProducts()
        {
            LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.SearchProducts(SelectedDept, SelectedSearchType, TextForSearch));
            UpdateCollection(LstOfRecords.Take(SelectedRecord));
            UpdateRecordCount();
        }
        #endregion


        public ObservableCollection<DeptModel> Depts_Pie { get; set; }

        //선택한 부서를 담을 프로퍼티
        private DeptModel selectedDept_Pie;
        public DeptModel SelectedDept_Pie
        {
            get { return selectedDept_Pie; }
            set
            {
                selectedDept_Pie = value;
                //OnPropertyChanged("SelectedDept_Pie");
                //DashboardPrint_Pie();
            }
        }

        public ObservableCollection<ProductInOutModel> ProductInout_Pie { get; set; }

        //선택한 출고유형(사용/이관/폐기)을 담을 프로퍼티
        private ProductInOutModel selectedOutType_Pie;

        public ProductInOutModel SelectedOutType_Pie
        {
            get { return selectedOutType_Pie; }
            set
            {
                selectedOutType_Pie = value;
                //OnPropertyChanged("SelectedOutType_Pie");
                //DashboardPrint_Pie();
            }
        }

        // 도넛그래프 SerieCollection(그래프틀)
        private SeriesCollection seriesCollection_Pie;
        public SeriesCollection SeriesCollection_Pie
        {
            get { return seriesCollection_Pie; }
            set
            {
                seriesCollection_Pie = value;
                OnPropertyChanged("SeriesCollection_Pie");
            }
        }

        //도넛그래프 출력메소드
        public void DashboardPrint_Pie()
        {
            log.Info("DashboardPrint_Pie() invoked.");
            try
            {
                List<ProductInOutModel> list = product_dao.GetDiscardTotalCount(SelectedDept_Pie, SelectedOutType_Pie);

                SeriesCollection_Pie = new SeriesCollection();

                foreach (var item in list)
                {
                    //Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:C})", item.Prod_name, chartPoint.Participation);
                    Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0:#,0}개 ({1:#,0}￦)", item.Prod_out_count, item.Prod_price);
                    SeriesCollection_Pie.Add(new PieSeries
                    {
                        Title = item.Prod_name,
                        Values = new ChartValues<int> { (int)item.Prod_out_count },
                        DataLabels = true,
                        LabelPoint = labelPoint
                    });

                }//foreache
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
           
        }//DashboardPrint_Pie


        //dashboardPrint_Pie command
        private ActionCommand command45;
        public ICommand Command45
        {
            get
            {
                if (command45 == null)
                {
                    command45 = new ActionCommand(dashboardPrint_Pie);
                }
                return command45;
            }//get

        }//Command

        public void dashboardPrint_Pie()
        {
            log.Info("dashboardPrint_Pie() invoked.");
            try
            {
                //DashboardPrint_Pie();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//dashboardPrint_Pie

        private ActionCommand dataGridRefreshCommand;
        public ICommand DataGridRefreshCommand
        {
            get
            {
                if (dataGridRefreshCommand == null)
                {
                    dataGridRefreshCommand = new ActionCommand(DataGridRefresh);
                }
                return dataGridRefreshCommand;
            }//get

        }//Command

        private void DataGridRefresh()
        {
            log.Info("DataGridRefresh() invoked.");
            try
            {
                TextForSearch = null;
                SelectedDept = Depts[(int)App.nurse_dto.Dept_id - 1];
                getListbyDept();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

        }//DataGridRefresh


        //오늘날짜 담을 프로퍼티
        public string testdate { get; set; }

        private ActionCommand categoryModifyCommand; //카테고리 수정 커맨드
        public ICommand CategoryModifyCommand
        {
            get
            {
                if (categoryModifyCommand == null)
                {
                    categoryModifyCommand = new ActionCommand(EditCategory);
                }
                return categoryModifyCommand;
            }//get
        }//CategoryModifyCommand

        private void EditCategory()
        {
            log.Info("EditCategory() invoked.");
            try
            {
                Console.WriteLine(SelectedModifyCategory.Category_name+"바꾸려는 카테명 ");
                category_dao.ChangeCategoryName(SelectedModifyCategory);

                //product_dao.ChangeProductInfo(SelectedModifyCategory);
                //product_dao.ChangeProductInfo_IMP_DEPT(SelectedProduct);

                //ObservableCollection<CategoryModel> list = new ObservableCollection<CategoryModel>(category_dao.GetCategories());
                //Categories.Clear();
                //foreach (var item in list)
                //{
                //    Categories.Add(item);
                //}

                //CategoryModel newCategoryDao = new CategoryModel();
                //var cateGoryId = category_dao.GetCategoryID(SelectedProduct.Category_name);
                //newCategoryDao.Category_id = cateGoryId;
                //newCategoryDao.Category_name = SelectedProduct.Category_name;
                //Categories.Insert(0,newCategoryDao);

                //첫페이지 말고 다음 페이지에서 재고수정할때 성공은 되는데 첫페이지로 돌아감 해결하기
                //LstOfRecords = new ObservableCollection<ProductShowModel>(product_dao.GetProductsByDept(SelectedDept));

                //updateSearchedProducts(false);
                //UpdateRecordCount();

                //DashboardPrint2(selectedDept);

                //DialogHost.ShowDialog(null);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

    }//class

   
}//namespace