using EasyProject.Dao;
using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using log4net;

namespace EasyProject.ViewModel
{
    public class ProductInOutViewModel : Notifier
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        ProductDao product_dao = new ProductDao();
        DeptDao dept_dao = new DeptDao();


        public string[] SearchTypeList { get; set; }


        //부서 리스트를 담을 프로퍼티
        public ObservableCollection<DeptModel> Depts { get; set; }

        private DeptModel selectedDept;
        public DeptModel SelectedDept
        {
            get { return selectedDept; }
            set
            {
                selectedDept = value;

                //부서 변경 시에
                //SearchKeyword_In = null; //검색 텍스트 초기화
                //SelectedStartDate_In = Convert.ToDateTime(product_dao.GetProductIn_MinDate(SelectedDept)); //날짜 컨트롤 최대, 최소 날짜로 설정
                //SelectedEndDate_In = Convert.ToDateTime(product_dao.GetProductIn_MaxDate(SelectedDept));
                
                OnPropertyChanged("SelectedDept");
                //getInListByDept();
            }
        }
        private ActionCommand deptChangedCommand;
        public ICommand DeptChangedCommand
        {
            get
            {
                if (deptChangedCommand == null)
                {
                    deptChangedCommand = new ActionCommand(DeptChanged);
                }
                return deptChangedCommand;
            }//get

        }//Command

        private void DeptChanged()
        {
            log.Info("DeptChanged() invoked.");
            try
            {
                Console.WriteLine("DeptChanged!--------------------------------------------------------------------");
                SearchKeyword_In = ""; //검색 텍스트 초기화
                searchKeyword_Out = ""; //검색 텍스트 초기화

                SelectedStartDate_In = Convert.ToDateTime(product_dao.GetProductIn_MinDate(SelectedDept)); //날짜 컨트롤 최대, 최소 날짜로 설정
                                                                                                           //SelectedEndDate_In = Convert.ToDateTime(product_dao.GetProductIn_MaxDate(SelectedDept));
                SelectedEndDate_In = DateTime.Today;

                //SelectedStartDate_Out = Convert.ToDateTime(product_dao.GetProductOut_MinDate(SelectedDept));
                //SelectedEndDate_Out = Convert.ToDateTime(product_dao.GetProductOut_MaxDate(SelectedDept));
                //SelectedEndDate_Out = DateTime.Today;

                //getProductIn_By_Date(); -> 날짜 바뀌는 부분에서 실행됨
                //getProductOut_By_Date();
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//DeptChanged

        private bool isDataGridCheckBoxChecked = true;
        public bool IsDataGridCheckBoxChecked
        {
            get { return isDataGridCheckBoxChecked; }
            set
            {
                isDataGridCheckBoxChecked = value;

                OnPropertyChanged("IsDataGridCheckBoxChecked");
            }
        }

        private bool isGraphCheckBoxChecked = true;
        public bool IsGraphCheckBoxChecked
        {
            get { return isGraphCheckBoxChecked; }
            set
            {
                isGraphCheckBoxChecked = value;

                OnPropertyChanged("IsGraphCheckBoxChecked");
            }
        }

        

        public ProductInOutViewModel()
        {
            log.Info("Constructor ProductInOutViewModel() invoked.");

            messagequeue = new SnackbarMessageQueue();

            SearchTypeList = new[] { "제품코드", "제품명", "품목/종류" };
            SelectedSearchType_In = SearchTypeList[0];
            SelectedSearchType_Out = SearchTypeList[0];

            Depts = new ObservableCollection<DeptModel>(dept_dao.GetDepts());
            SelectedDept = Depts[(int)App.nurse_dto.Dept_id - 1];

            
            //Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept));
            Product_in = new ObservableCollection<ProductInOutModel>();
            Product_out = new ObservableCollection<ProductInOutModel>();


            //날짜 컨트롤 부서별 해당 최소 날짜 및 최대 날짜로 초기화
            //입고
            SelectedStartDate_In = Convert.ToDateTime(product_dao.GetProductIn_MinDate(SelectedDept));
            if(SelectedStartDate_In.Value.Year == 1)
            {
                SelectedStartDate_In = DateTime.Today.AddDays(-1); // MinDate가 0001 01 01 인 경우 전날로 설정
            }
            //SelectedEndDate_In = Convert.ToDateTime(product_dao.GetProductIn_MaxDate(SelectedDept));
            SelectedEndDate_In = DateTime.Today;

            //출고 : 날짜 컨트롤 위의 하나로 입고/출고 두 페이지 모두 사용중
            //SelectedStartDate_Out = Convert.ToDateTime(product_dao.GetProductOut_MinDate(SelectedDept));
            //SelectedEndDate_Out = Convert.ToDateTime(product_dao.GetProductOut_MaxDate(SelectedDept));
            //SelectedEndDate_Out = DateTime.Today;

            //부서별 입고 유형별 빈도 그래프 (기간 선택 가능 * 초기 설정 : 현재날짜로부터 1주일)
            SelectedStartDate2 = DateTime.Today.AddDays(-7);
            SelectedEndDate2 = DateTime.Today;

            //부서별 출고 유형별 빈도 그래프 (기간 선택 가능 * 초기 설정 : 현재날짜로부터 1주일)
            SelectedStartDate1 = DateTime.Today.AddDays(-7);
            SelectedEndDate1 = DateTime.Today;

            //입고 출고 그래프 출력
            //DashboardPrint2();
            //DashboardPrint3();

            //파이차트
            //파이차트
            Depts_Pie = new ObservableCollection<DeptModel>(dept_dao.GetDepts());   //dept_od를 가져온다
            SelectedDept_Pie = Depts_Pie[(int)App.nurse_dto.Dept_id - 1];  // 
            /*ProductInout_Pie = new ObservableCollection<ProductInOutModel>(product_dao.GetProdOutType());
            SelectedOutType_Pie = ProductInout_Pie[0];
            DashboardPrint_Pie();*/
            


            //UpdateCalendarBlackoutDates();
        }//Constructor

        public List<string> BarLabels2 { get; set; }       //string[]
        public List<string> BarLabels3 { get; set; }       //string[]
        public List<string> BarLabels4 { get; set; }       //string[]
        public List<string> BarLabels5 { get; set; }       //string[]
        public ChartValues<int> Values2 { get; set; }
        public ChartValues<int> Values3 { get; set; }
        public ChartValues<int> Values4 { get; set; }
        public ChartValues<int> Values5 { get; set; }
        public Func<double, string> Formatter { get; set; }

        // DashboardPrint() 그래프
        // 부서별 출고 유형 그래프 (기간 선택 가능) -----------------------------------
        private SeriesCollection seriesCollection2;
        public SeriesCollection SeriesCollection2
        {
            get { return seriesCollection2; }
            set
            {
                seriesCollection2 = value;
                OnPropertyChanged("SeriesCollection2");
            }
        }

        public DateTime selectedStartDate1;
        public DateTime SelectedStartDate1
        {
            get { return selectedStartDate1; }
            set
            {
                selectedStartDate1 = value;
                OnPropertyChanged("SelectedStartDate1");
                //DashboardPrint2();
                //DashboardPrint3();
                //DashboardPrint_Pie();
            }
        }
        public DateTime selectedEndDate1;
        public DateTime SelectedEndDate1
        {
            get { return selectedEndDate1; }
            set
            {
                selectedEndDate1 = value;
                OnPropertyChanged("SelectedEndDate1");
                //DashboardPrint2();
                //DashboardPrint3();
                //DashboardPrint_Pie();
            }
        }

        //부서별 입고 유형 그래프
        private SeriesCollection seriesCollection3;
        public SeriesCollection SeriesCollection3               //그래프 큰 틀 만드는거
        {
            get { return seriesCollection3; }
            set
            {
                seriesCollection3 = value;
                OnPropertyChanged("SeriesCollection3");
            }
        }
        public DateTime selectedStartDate2;
        public DateTime SelectedStartDate2
        {
            get { return selectedStartDate2; }
            set
            {
                selectedStartDate2 = value;
                OnPropertyChanged("SelectedStartDate2");
                //DashboardPrint3();
            }
        }
        public DateTime selectedEndDate2;
        public DateTime SelectedEndDate2
        {
            get { return selectedEndDate2; }
            set
            {
                selectedEndDate2 = value;
                OnPropertyChanged("SelectedEndDate2");
                //DashboardPrint3();
            }
        }
        // 입고 차트 복제 프로퍼티
        private SeriesCollection seriesCollection4;
        public SeriesCollection SeriesCollection4               //그래프 큰 틀 만드는거
        {
            get { return seriesCollection4; }
            set
            {
                seriesCollection4 = value;
                OnPropertyChanged("SeriesCollection4");
            }
        }

        // 출고 차트 복제 프로퍼티
        private SeriesCollection seriesCollection5;
        public SeriesCollection SeriesCollection5               //그래프 큰 틀 만드는거
        {
            get { return seriesCollection5; }
            set
            {
                seriesCollection5 = value;
                OnPropertyChanged("SeriesCollection5");
            }
        }
        //파이 차트 프로퍼티
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
        //도넛그래프 복제 그래프 틀
        private SeriesCollection seriesCollection_Pie1;
        public SeriesCollection SeriesCollection_Pie1
        {
            get { return seriesCollection_Pie1; }
            set
            {
                seriesCollection_Pie1 = value;
                OnPropertyChanged("SeriesCollection_Pie1");
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
                    //Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0:#,0}개 ({1:#,0}￦)", item.Prod_out_count, item.Prod_price);
                    SeriesCollection_Pie.Add(new PieSeries
                    {
                        Title = item.Prod_name+ "  가격:", 
                        Values = new ChartValues<int> { (int)item.Prod_price },
                       
                    });

                }//foreache
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
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
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
        }//dashboardPrint_Pie


        public void DashboardPrint2(DateTime? start, DateTime? end)//출고
        {
            log.Info("DashboardPrint2(DateTime?, DateTime?) invoked.");

            try
            {
                Console.WriteLine("DashboardPrint2 실행(출고그래프)---------------");
                SeriesCollection2 = new SeriesCollection();
                Values2 = new ChartValues<int> { }; // 컬럼의 수치 ( y 축 )
                ChartValues<int> useCases = new ChartValues<int>(); // 사용 횟수를 담을 변수
                ChartValues<int> transferCases = new ChartValues<int>(); // 이관 횟수를 담을 변수
                ChartValues<int> discardCases = new ChartValues<int>(); // 폐기 횟수를 담을 변수
                BarLabels2 = new List<string>() { }; // 컬럼의 이름 ( x 축 )
                List<ProductInOutModel> datas = product_dao.ReleaseCases_Info(start, end); // 부서별 출고 유형/횟수 정보
                foreach (var item in datas) // 부서명 Labels에 넣기
                {
                    BarLabels2.Add(item.Dept_name);
                    Console.WriteLine("dashboard2_dept_name" + item.Dept_name);
                }

                foreach (var item in datas)
                {
                    useCases.Add((int)item.prod_use_cases);
                    Console.WriteLine("prod_use_cases" + item.prod_use_cases);
                    transferCases.Add((int)item.prod_transferOut_cases);
                    Console.WriteLine("prod_tarnsferout" + item.prod_transferOut_cases);
                    discardCases.Add((int)item.prod_discard_cases);
                    Console.WriteLine("prod_discard_cases" + item.prod_discard_cases);
                }

                //adding series updates and animates the chart

                SeriesCollection2.Add(new StackedColumnSeries // 부서별 사용 횟수
                {
                    Title = "사용 횟수",
                    Values = useCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection2.Add(new StackedColumnSeries // 부서별 이관 횟수
                {
                    Title = "이관 횟수",
                    Values = transferCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection2.Add(new StackedColumnSeries // 부서별 출고 횟수
                {
                    Title = "폐기 횟수",
                    Values = discardCases,
                    StackMode = StackMode.Values
                });

                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
        }//dashboardprint2 ---------------------------------------------------------------------------------------------------

        // 부서별 입고 유형별 빈도 그래프 (기간 선택 가능) (VIEW : 좌측하단 위치)---------------------
        public void DashboardPrint3(DateTime? start, DateTime? end)
        {
            log.Info("DashboardPrint3(DateTime?, DateTime?) invoked.");

            try
            {
                Console.WriteLine("DashboardPrint3");
                SeriesCollection3 = new SeriesCollection();
                Values3 = new ChartValues<int> { }; // 컬럼의 수치 ( y 축 )
                ChartValues<int> transferCases = new ChartValues<int>(); // 이관 횟수를 담을 변수
                ChartValues<int> orderCases = new ChartValues<int>(); // 신규 횟수를 담을 변수
                ChartValues<int> addCases = new ChartValues<int>(); // 추가 횟수를 담을 변수
                BarLabels3 = new List<string>() { }; // 컬럼의 이름 ( x 축 )
                List<ProductInOutModel> datas = product_dao.incomingCases_Info(start, end); // 부서별 출고 유형/횟수 정보
                foreach (var item in datas) // 부서명 Labels에 넣기
                {
                    BarLabels3.Add(item.Dept_name);
                    Console.WriteLine("dashboard3_dept_name" + item.Dept_name);
                }

                foreach (var item in datas)
                {
                    transferCases.Add((int)item.prod_transferIn_cases);
                    orderCases.Add((int)item.prod_order_cases);
                    addCases.Add((int)item.prod_add_cases);
                }

                //adding series updates and animates the chart

                SeriesCollection3.Add(new StackedColumnSeries // 부서별 이관 횟수
                {
                    Title = "이관입고 횟수",
                    Values = transferCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection3.Add(new StackedColumnSeries // 부서별 신규입고 횟수
                {
                    Title = "신규입고 횟수",
                    Values = orderCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection3.Add(new StackedColumnSeries // 부서별 추가입고 횟수
                {
                    Title = "추가입고 횟수",
                    Values = addCases,
                    StackMode = StackMode.Values
                });

                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
        }//dashboardprint3 ---------------------------------------------------------------------------------------------------

         // 부서별 입고 그래프 복제
        public void DashboardPrint4(DateTime? start, DateTime? end)
        {
            log.Info("DashboardPrint4(DateTime?, DateTime?) invoked.");

            try
            {
                Console.WriteLine("DashboardPrint3");
                SeriesCollection4 = new SeriesCollection();
                Values4 = new ChartValues<int> { }; // 컬럼의 수치 ( y 축 )
                ChartValues<int> transferCases = new ChartValues<int>(); // 이관 횟수를 담을 변수
                ChartValues<int> orderCases = new ChartValues<int>(); // 신규 횟수를 담을 변수
                ChartValues<int> addCases = new ChartValues<int>(); // 추가 횟수를 담을 변수
                BarLabels4 = new List<string>() { }; // 컬럼의 이름 ( x 축 )
                List<ProductInOutModel> datas = product_dao.incomingCases_Info(start, end); // 부서별 출고 유형/횟수 정보
                foreach (var item in datas) // 부서명 Labels에 넣기
                {
                    BarLabels4.Add(item.Dept_name);
                    Console.WriteLine("dashboard3_dept_name" + item.Dept_name);
                }

                foreach (var item in datas)
                {
                    transferCases.Add((int)item.prod_transferIn_cases);
                    orderCases.Add((int)item.prod_order_cases);
                    addCases.Add((int)item.prod_add_cases);
                }

                //adding series updates and animates the chart

                SeriesCollection4.Add(new StackedColumnSeries // 부서별 이관 횟수
                {
                    Title = "이관입고 횟수",
                    Values = transferCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection4.Add(new StackedColumnSeries // 부서별 신규입고 횟수
                {
                    Title = "신규입고 횟수",
                    Values = orderCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection4.Add(new StackedColumnSeries // 부서별 추가입고 횟수
                {
                    Title = "추가입고 횟수",
                    Values = addCases,
                    StackMode = StackMode.Values
                });

                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
        }//dashboardprint4 ---------------------------------------------------------------------------------------------------

        // 부서별 출고 그래프 복제
        public void DashboardPrint5(DateTime? start, DateTime? end)//출고
        {
            log.Info("DashboardPrint5(DateTime?, DateTime?) invoked.");

            try
            {
                Console.WriteLine("DashboardPrint5");
                SeriesCollection5 = new SeriesCollection();
                Values5 = new ChartValues<int> { }; // 컬럼의 수치 ( y 축 )
                ChartValues<int> useCases = new ChartValues<int>(); // 사용 횟수를 담을 변수
                ChartValues<int> transferCases = new ChartValues<int>(); // 이관 횟수를 담을 변수
                ChartValues<int> discardCases = new ChartValues<int>(); // 폐기 횟수를 담을 변수
                BarLabels5 = new List<string>() { }; // 컬럼의 이름 ( x 축 )
                List<ProductInOutModel> datas = product_dao.ReleaseCases_Info(start, end); // 부서별 출고 유형/횟수 정보
                foreach (var item in datas) // 부서명 Labels에 넣기
                {
                    BarLabels5.Add(item.Dept_name);
                    Console.WriteLine("dashboard2_dept_name" + item.Dept_name);
                }

                foreach (var item in datas)
                {
                    useCases.Add((int)item.prod_use_cases);
                    Console.WriteLine("prod_use_cases" + item.prod_use_cases);
                    transferCases.Add((int)item.prod_transferOut_cases);
                    Console.WriteLine("prod_tarnsferout" + item.prod_transferOut_cases);
                    discardCases.Add((int)item.prod_discard_cases);
                    Console.WriteLine("prod_discard_cases" + item.prod_discard_cases);
                }

                //adding series updates and animates the chart

                SeriesCollection5.Add(new StackedColumnSeries // 부서별 사용 횟수
                {
                    Title = "사용 횟수",
                    Values = useCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection5.Add(new StackedColumnSeries // 부서별 이관 횟수
                {
                    Title = "이관 횟수",
                    Values = transferCases,
                    StackMode = StackMode.Values
                });

                SeriesCollection5.Add(new StackedColumnSeries // 부서별 출고 횟수
                {
                    Title = "폐기 횟수",
                    Values = discardCases,
                    StackMode = StackMode.Values
                });

                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            
        }//dashboardprint5 ---------------------------------------------------------------------------------------------------


        #region 입고 pagination
        //검색 텍스트 - 입고
        private string searchKeyword_In;
        public string SearchKeyword_In
        {
            get { return searchKeyword_In; }
            set { searchKeyword_In = value; OnPropertyChanged("SearchKeyword_In"); }
        }


        public string SelectedSearchType_In { get; set; }

        //입고 시작일을 담을 프로퍼티
        private DateTime? selectedStartDate_In;
        public DateTime? SelectedStartDate_In
        {
            get { return selectedStartDate_In; }
            set
            {
                selectedStartDate_In = value;
                                                                                         
                SearchKeyword_In = "";
                SearchKeyword_Out = "";
                getProductIn_By_Date();
                getProductOut_By_Date();
                if (selectedStartDate_In > selectedEndDate_In)
                {
                    SelectedStartDate_In = SelectedEndDate_In.Value.AddDays(-1);
                }
                OnPropertyChanged("SelectedStartDate_In");
                DashboardPrint2(selectedStartDate_In, selectedEndDate_In);
                DashboardPrint3(selectedStartDate_In, selectedEndDate_In);
                DashboardPrint4(selectedStartDate_In, selectedEndDate_In);
                DashboardPrint5(selectedStartDate_In, selectedEndDate_In);
            }
        }
        //종료일을 담을 프로퍼티
        private DateTime? selectedEndDate_In;
        public DateTime? SelectedEndDate_In
        {
            get { return selectedEndDate_In; }
            set
            {
                selectedEndDate_In = value;

                //ShowProductIn_By_Date();
                SearchKeyword_In = "";
                SearchKeyword_Out = "";

                getProductIn_By_Date();
                getProductOut_By_Date();
                if (selectedStartDate_In > selectedEndDate_In)
                {
                    SelectedStartDate_In = SelectedEndDate_In.Value.AddDays(-1);
                }
                OnPropertyChanged("SelectedEndDate_In");

                DashboardPrint2(selectedStartDate_In, selectedEndDate_In);
                DashboardPrint3(selectedStartDate_In, selectedEndDate_In);
                DashboardPrint4(selectedStartDate_In, selectedEndDate_In);
                DashboardPrint5(selectedStartDate_In, selectedEndDate_In);
            }
        }
        //화면에 보여줄 리스트 입고 내역 담을 프로퍼티
        private ObservableCollection<ProductInOutModel> product_in;
        public ObservableCollection<ProductInOutModel> Product_in
        {
            get { return product_in; }
            set
            {
                product_in = value;
                OnPropertyChanged("Product_in");
            }
        }
        public IEnumerable<ProductInOutModel> searchedInProducts { get; set; } // 검색결과에 해당하는 객체들을 임시로 담아놓을 프로퍼티

        private ActionCommand inSearchKeywordCommand;
        public ICommand InSearchKeywordCommand
        {
            get
            {
                if (inSearchKeywordCommand == null)
                {
                    inSearchKeywordCommand = new ActionCommand(updateInSearchedProducts);
                }
                return inSearchKeywordCommand;
            }//get
        }

        public void updateInSearchedProducts()
        {
            log.Info("updateInSearchedProducts() invoked.");

            try
            {
                Console.WriteLine("updateInSearchedProducts() 검색어 : " + SearchKeyword_In);

                InCurrentPage = 1; // 검색어 바뀔 때마다 1페이지로 이동


                if (SearchKeyword_In != null) // 키워드 있을 때
                {
                    if (SelectedSearchType_In == "제품명")
                    {
                        searchedInProducts = InLstOfRecords.Where(model => model.Prod_name.Contains(SearchKeyword_In) || model.Prod_name.Contains(SearchKeyword_In.ToUpper()) || model.Prod_name.Contains(SearchKeyword_In.ToLower()));
                        Console.WriteLine("제품명 : " + searchedInProducts.Count() + SearchKeyword_In.ToUpper());

                        UpdateInRecordCount();

                    }
                    else if (SelectedSearchType_In == "제품코드")
                    {
                        searchedInProducts = InLstOfRecords.Where(model => model.Prod_code.Contains(SearchKeyword_In) || model.Prod_code.Contains(SearchKeyword_In.ToUpper()) || model.Prod_code.Contains(SearchKeyword_In.ToLower()));
                        Console.WriteLine("제품코드 : " + searchedInProducts.Count());

                        UpdateInRecordCount();
                    }
                    else // 품목/종류
                    {
                        searchedInProducts = InLstOfRecords.Where(model => model.Category_name.Contains(SearchKeyword_In) || model.Category_name.Contains(SearchKeyword_In.ToUpper()) || model.Category_name.Contains(SearchKeyword_In.ToLower()));
                        Console.WriteLine("품목/종류 : " + searchedInProducts.Count());

                        UpdateInRecordCount();
                    }

                }//if
                else // 키워드 없을 때
                {
                    searchedInProducts = InLstOfRecords;
                    Console.WriteLine("검색어없음: " + searchedInProducts.Count());

                    UpdateInRecordCount();

                }//else
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            

        }//updateInSearchedProducts

        public ObservableCollection<ProductInOutModel> InLstOfRecords { get; set; }

        public void getProductIn_By_Date()
        {
            log.Info("getProductIn_By_Date() invoked.");
            try
            {
                if (SelectedStartDate_In != null && SelectedEndDate_In != null)
                {
                    InLstOfRecords = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept, SelectedStartDate_In, SelectedEndDate_In));
                    updateInSearchedProducts();
                    //UpdateInRecordCount();
                }//if
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
        }//getProductIn_By_Date

        public void getInListByDept()
        {
            log.Info("getInListByDept() invoked.");

            try
            {
                InLstOfRecords = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept));
                updateInSearchedProducts();
                //UpdateInRecordCount();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//getInListByDept

        int InRecordStartFrom = 0;
        private void InPreviousPage(object obj)
        {
            log.Info("InPreviousPage(object) invoked.");

            try
            {
                InCurrentPage--;
                InRecordStartFrom = (InCurrentPage - 1) * InSelectedRecord;
                var recorsToShow = searchedInProducts.Skip(InRecordStartFrom).Take(InSelectedRecord);
                UpdateInCollection(recorsToShow);
                UpdateInEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch


        }//InPreviousPage

        private ActionCommand inNextCommand;
        public ICommand InNextCommand
        {
            get
            {
                if (inNextCommand == null)
                {
                    inNextCommand = new ActionCommand(InNextPage);
                }
                return inNextCommand;
            }//get
        }



        private ActionCommand infirstCommand;
        public ICommand InFirstCommand
        {
            get
            {
                if (infirstCommand == null)
                {
                    infirstCommand = new ActionCommand(InFirstPage);
                }
                return infirstCommand;
            }//get
        }

        private ActionCommand inLastCommand;
        public ICommand InLastCommand
        {
            get
            {
                if (inLastCommand == null)
                {
                    inLastCommand = new ActionCommand(InLastPage);
                }
                return inLastCommand;
            }//get
        }

        private ActionCommand inPreviouCommand;
        public ICommand InPreviousCommand
        {
            get
            {
                if (inPreviouCommand == null)
                {
                    inPreviouCommand = new ActionCommand(InPreviousPage);
                }
                return inPreviouCommand;
            }//get
        }
        private void InLastPage(object obj)
        {
            log.Info("InLastPage(object) invoked.");

            try
            {
                //30 1->20, 2->10
                //last page - 21 -30
                //11-30=>30-20=10 -> 11-30

                //fix
                //20*(2-1)=20
                //skip = 20
                var recordsToskip = InSelectedRecord * (InNumberOfPages - 1);
                UpdateInCollection(searchedInProducts.Skip(recordsToskip));
                InCurrentPage = InNumberOfPages;
                //MessageBox.Show(CurrentPage + "페이지");
                UpdateInEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//InLastPage


        private void InFirstPage(object obj)
        {
            log.Info("InFirstPage(object) invoked.");

            try
            {
                UpdateInCollection(searchedInProducts.Take(InSelectedRecord));
                InCurrentPage = 1;
                UpdateInEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }
        private void InNextPage(object obj)
        {
            log.Info("InNextPage(object) invoked.");

            try
            {
                InRecordStartFrom = InCurrentPage * InSelectedRecord;
                var recordsToShow = searchedInProducts.Skip(InRecordStartFrom).Take(InSelectedRecord);
                UpdateInCollection(recordsToShow);
                InCurrentPage++;
                UpdateInEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//InNextPage

        private void UpdateInCollection(IEnumerable<ProductInOutModel> enumerable)
        {
            log.Info("UpdateInCollection(IEnumerable<ProductInOutModel>) invoked.");

            try
            {
                if (Product_in != null)
                {
                    Product_in.Clear();
                }
                else
                {
                    Product_in = new ObservableCollection<ProductInOutModel>();
                }

                foreach (var item in enumerable)
                {
                    Product_in.Add(item);
                }
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//UpdateInCollection

        private int inCurrentPage = 1;

        public int InCurrentPage
        {
            get { return inCurrentPage; }
            set
            {
                inCurrentPage = value;
                OnPropertyChanged(nameof(InCurrentPage));
                UpdateInEnableState();
            }
        }
        private void UpdateInEnableState()
        {
            log.Info("UpdateInEnableState() invoked.");

            try
            {
                IsInFirstEnabled = InCurrentPage > 1;
                IsInPreviousEnabled = InCurrentPage > 1;
                IsInNextEnabled = InCurrentPage < InNumberOfPages;
                IsInLastEnabled = InCurrentPage < InNumberOfPages;
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//UpdateInEnableState

        private int inNumberOfPages = 10;

        public int InNumberOfPages
        {
            get { return inNumberOfPages; }
            set
            {
                inNumberOfPages = value;
                OnPropertyChanged(nameof(InNumberOfPages));
                UpdateInEnableState();
            }
        }
        private bool isInFirstEnabled;

        public bool IsInFirstEnabled
        {
            get { return isInFirstEnabled; }
            set
            {
                isInFirstEnabled = value;
                OnPropertyChanged(nameof(IsInFirstEnabled));
            }
        }

        private bool isInPreviousEnabled;

        public bool IsInPreviousEnabled
        {
            get { return isInPreviousEnabled; }
            set
            {
                isInPreviousEnabled = value;
                OnPropertyChanged(nameof(IsInPreviousEnabled));
            }
        }
        private bool isInLastEnabled;

        public bool IsInLastEnabled
        {
            get { return isInLastEnabled; }
            set
            {
                isInLastEnabled = value;
                OnPropertyChanged(nameof(IsInLastEnabled));
            }
        }

        private bool isInNextEnabled;

        public bool IsInNextEnabled
        {
            get { return isInNextEnabled; }
            set
            {
                isInNextEnabled = value;
                OnPropertyChanged(nameof(IsInNextEnabled));
            }
        }

        private int inSelectedRecord = 10;

        public int InSelectedRecord
        {
            get { return inSelectedRecord; }
            set
            {
                inSelectedRecord = value;
                OnPropertyChanged(nameof(InSelectedRecord));
                UpdateInRecordCount();
            }
        }
        private void UpdateInRecordCount()
        {
            log.Info("UpdateInRecordCount() invoked.");

            try
            {
                InNumberOfPages = (int)Math.Ceiling((double)searchedInProducts.Count() / InSelectedRecord);
                InNumberOfPages = InNumberOfPages == 0 ? 1 : InNumberOfPages;

                InRecordStartFrom = (InCurrentPage - 1) * InSelectedRecord;
                var recordsToShow = searchedInProducts.Skip(InRecordStartFrom).Take(InSelectedRecord);

                UpdateInCollection(recordsToShow);
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//UpdateInRecordCount
        #endregion

        #region 출고 pagination
        //검색 텍스트 - 입고
        private string searchKeyword_Out;
        public string SearchKeyword_Out
        {
            get { return searchKeyword_Out; }
            set { searchKeyword_Out = value; OnPropertyChanged("SearchKeyword_Out"); }
        }


        public string SelectedSearchType_Out { get; set; }

        //입고 시작일을 담을 프로퍼티
        private DateTime? selectedStartDate_Out;
        public DateTime? SelectedStartDate_Out
        {
            get { return selectedStartDate_Out; }
            set
            {
                selectedStartDate_Out = value;

                //ShowProductIn_By_Date();
                SearchKeyword_Out = null;
                getProductOut_By_Date();
                if (selectedStartDate_Out > selectedEndDate_Out)
                {
                    SelectedStartDate_Out = SelectedEndDate_Out.Value.AddDays(-1);
                }
                DashboardPrint4(selectedStartDate_Out, selectedEndDate_Out);
                DashboardPrint5(selectedStartDate_Out, selectedEndDate_Out);
                OnPropertyChanged("SelectedStartDate_Out");
            }
        }
        //종료일을 담을 프로퍼티
        private DateTime? selectedEndDate_Out;
        public DateTime? SelectedEndDate_Out
        {
            get { return selectedEndDate_Out; }
            set
            {
                selectedEndDate_Out = value;

                //ShowProductIn_By_Date();                
                SearchKeyword_Out = null;
                getProductOut_By_Date();
                if (selectedStartDate_Out > selectedEndDate_Out)
                {
                    SelectedStartDate_Out = SelectedEndDate_Out.Value.AddDays(-1);
                }
                DashboardPrint4(selectedStartDate_Out, selectedEndDate_Out);
                DashboardPrint5(selectedStartDate_Out, selectedEndDate_Out);
                OnPropertyChanged("SelectedEndDate_Out");
            }
        }
        //화면에 보여줄 리스트 입고 내역 담을 프로퍼티
        private ObservableCollection<ProductInOutModel> product_out;
        public ObservableCollection<ProductInOutModel> Product_out
        {
            get { return product_out; }
            set
            {
                product_out = value;
                OnPropertyChanged("Product_out");
            }
        }
        public IEnumerable<ProductInOutModel> searchedOutProducts { get; set; } // 검색결과에 해당하는 객체들을 임시로 담아놓을 프로퍼티

        private ActionCommand outSearchKeywordCommand;
        public ICommand OutSearchKeywordCommand
        {
            get
            {
                if (outSearchKeywordCommand == null)
                {
                    outSearchKeywordCommand = new ActionCommand(updateOutSearchedProducts);
                }
                return outSearchKeywordCommand;
            }//get
        }

        public void updateOutSearchedProducts()
        {
            log.Info("updateOutSearchedProducts() invoked.");

            try
            {
                Console.WriteLine("updateOutSearchedProducts() 검색어 : " + SearchKeyword_Out);

                OutCurrentPage = 1; // 검색어 바뀔 때마다 1페이지로 이동


                if (SearchKeyword_Out != null) // 키워드 있을 때
                {
                    if (SelectedSearchType_Out == "제품명")
                    {
                        searchedOutProducts = OutLstOfRecords.Where(model => model.Prod_name.Contains(SearchKeyword_Out) || model.Prod_name.Contains(SearchKeyword_Out.ToUpper()) || model.Prod_name.Contains(SearchKeyword_Out.ToLower()));
                        Console.WriteLine("제품명 : " + searchedOutProducts.Count() + SearchKeyword_Out.ToUpper());

                        UpdateOutRecordCount();

                    }
                    else if (SelectedSearchType_Out == "제품코드")
                    {
                        searchedOutProducts = OutLstOfRecords.Where(model => model.Prod_code.Contains(SearchKeyword_Out) || model.Prod_code.Contains(SearchKeyword_Out.ToUpper()) || model.Prod_code.Contains(SearchKeyword_Out.ToLower()));
                        Console.WriteLine("제품코드 : " + searchedOutProducts.Count());

                        UpdateOutRecordCount();
                    }
                    else // 품목/종류
                    {
                        searchedOutProducts = OutLstOfRecords.Where(model => model.Category_name.Contains(SearchKeyword_Out) || model.Category_name.Contains(SearchKeyword_Out.ToUpper()) || model.Category_name.Contains(SearchKeyword_Out.ToLower()));
                        Console.WriteLine("품목/종류 : " + searchedInProducts.Count());

                        UpdateOutRecordCount();
                    }

                }//if
                else // 키워드 없을 때
                {
                    searchedOutProducts = OutLstOfRecords;
                    Console.WriteLine("검색어없음: " + searchedOutProducts.Count());

                    UpdateOutRecordCount();

                }//else
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch


        }//updateInSearchedProducts

        public ObservableCollection<ProductInOutModel> OutLstOfRecords { get; set; }

        public void getProductOut_By_Date()
        {
            log.Info("getProductOut_By_Date() invoked.");

            try
            {
                if (SelectedStartDate_Out != null && SelectedEndDate_Out != null)
                {
                    OutLstOfRecords = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept, SelectedStartDate_In, SelectedEndDate_In));
                    updateOutSearchedProducts();
                    UpdateOutRecordCount();
                }
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//getProductOut_By_Date

        public void getOutListByDept()
        {
            log.Info("getOutListByDept() invoked.");

            try
            {
                OutLstOfRecords = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept));
                updateOutSearchedProducts();
                UpdateOutRecordCount();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch


        }//getOutListByDept

        int OutRecordStartFrom = 0;
        private void OutPreviousPage(object obj)
        {
            log.Info("OutPreviousPage(object) invoked.");

            try
            {
                OutCurrentPage--;
                OutRecordStartFrom = (OutCurrentPage - 1) * OutSelectedRecord;
                var recorsToShow = searchedOutProducts.Skip(OutRecordStartFrom).Take(OutSelectedRecord);
                UpdateOutCollection(recorsToShow);
                UpdateOutEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutPreviousPage

        private ActionCommand outNextCommand;
        public ICommand OutNextCommand
        {
            get
            {
                if (outNextCommand == null)
                {
                    outNextCommand = new ActionCommand(OutNextPage);
                }
                return outNextCommand;
            }//get
        }



        private ActionCommand outfirstCommand;
        public ICommand OutFirstCommand
        {
            get
            {
                if (outfirstCommand == null)
                {
                    outfirstCommand = new ActionCommand(OutFirstPage);
                }
                return outfirstCommand;
            }//get
        }

        private ActionCommand outLastCommand;
        public ICommand OutLastCommand
        {
            get
            {
                if (outLastCommand == null)
                {
                    outLastCommand = new ActionCommand(OutLastPage);
                }
                return outLastCommand;
            }//get
        }

        private ActionCommand outPreviouCommand;
        public ICommand OutPreviousCommand
        {
            get
            {
                if (outPreviouCommand == null)
                {
                    outPreviouCommand = new ActionCommand(OutPreviousPage);
                }
                return outPreviouCommand;
            }//get
        }
        private void OutLastPage(object obj)
        {
            log.Info("OutLastPage(object) invoked.");

            try
            {
                var recordsToskip = OutSelectedRecord * (OutNumberOfPages - 1);
                UpdateInCollection(searchedOutProducts.Skip(recordsToskip));
                OutCurrentPage = OutNumberOfPages;
                //MessageBox.Show(CurrentPage + "페이지");
                UpdateOutEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutLastPage


        private void OutFirstPage(object obj)
        {
            log.Info("OutFirstPage(object) invoked.");

            try
            {
                UpdateOutCollection(searchedOutProducts.Take(OutSelectedRecord));
                OutCurrentPage = 1;
                UpdateOutEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutFirstPage

        private void OutNextPage(object obj)
        {
            log.Info("OutNextPage(object) invoked.");

            try
            {
                OutRecordStartFrom = OutCurrentPage * OutSelectedRecord;
                var recordsToShow = searchedOutProducts.Skip(OutRecordStartFrom).Take(OutSelectedRecord);
                UpdateOutCollection(recordsToShow);
                OutCurrentPage++;
                UpdateOutEnableState();
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutNextPage

        private void UpdateOutCollection(IEnumerable<ProductInOutModel> enumerable)
        {
            log.Info("UpdateOutCollection(IEnumerable<ProductInOutModel>) invoked.");

            try
            {
                if (Product_out != null)
                {
                    Product_out.Clear();
                }
                else
                {
                    Product_out = new ObservableCollection<ProductInOutModel>();
                }

                foreach (var item in enumerable)
                {
                    Product_out.Add(item);
                }
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//UpdateOutCollection


        private int outCurrentPage = 1;

        public int OutCurrentPage
        {
            get { return outCurrentPage; }
            set
            {
                outCurrentPage = value;
                OnPropertyChanged(nameof(OutCurrentPage));
                UpdateOutEnableState();
            }
        }
        private void UpdateOutEnableState()
        {
            log.Info("UpdateOutEnableState() invoked.");

            try
            {
                IsOutFirstEnabled = OutCurrentPage > 1;
                IsOutPreviousEnabled = OutCurrentPage > 1;
                IsOutNextEnabled = OutCurrentPage < OutNumberOfPages;
                IsOutLastEnabled = OutCurrentPage < OutNumberOfPages;
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//UpdateOutEnableState

        private int outNumberOfPages = 10;

        public int OutNumberOfPages
        {
            get { return outNumberOfPages; }
            set
            {
                outNumberOfPages = value;
                OnPropertyChanged(nameof(OutNumberOfPages));
                UpdateOutEnableState();
            }
        }
        private bool isOutFirstEnabled;

        public bool IsOutFirstEnabled
        {
            get { return isOutFirstEnabled; }
            set
            {
                isOutFirstEnabled = value;
                OnPropertyChanged(nameof(IsOutFirstEnabled));
            }
        }

        private bool isOutPreviousEnabled;

        public bool IsOutPreviousEnabled
        {
            get { return isOutPreviousEnabled; }
            set
            {
                isOutPreviousEnabled = value;
                OnPropertyChanged(nameof(IsOutPreviousEnabled));
            }
        }
        private bool isOutLastEnabled;

        public bool IsOutLastEnabled
        {
            get { return isOutLastEnabled; }
            set
            {
                isOutLastEnabled = value;
                OnPropertyChanged(nameof(IsOutLastEnabled));
            }
        }

        private bool isOutNextEnabled;

        public bool IsOutNextEnabled
        {
            get { return isOutNextEnabled; }
            set
            {
                isOutNextEnabled = value;
                OnPropertyChanged(nameof(IsOutNextEnabled));
            }
        }

        private int outSelectedRecord = 10;

        public int OutSelectedRecord
        {
            get { return outSelectedRecord; }
            set
            {
                outSelectedRecord = value;
                OnPropertyChanged(nameof(OutSelectedRecord));
                UpdateOutRecordCount();
            }
        }
        private void UpdateOutRecordCount()
        {
            log.Info("UpdateOutRecordCount() invoked.");

            try
            {
                OutNumberOfPages = (int)Math.Ceiling((double)searchedOutProducts.Count() / OutSelectedRecord);
                OutNumberOfPages = OutNumberOfPages == 0 ? 1 : OutNumberOfPages;

                OutRecordStartFrom = (OutCurrentPage - 1) * OutSelectedRecord;
                var recordsToShow = searchedOutProducts.Skip(OutRecordStartFrom).Take(OutSelectedRecord);

                UpdateOutCollection(recordsToShow);
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//UpdateOutRecordCount
        #endregion


        #region 입고 (미사용) 
        ////입고 - 선택한 부서를 담을 프로퍼티
        //private DeptModel selectedDept_In;
        //public DeptModel SelectedDept_In
        //{
        //    get { return selectedDept_In; }
        //    set
        //    {
        //        selectedDept_In = value;

        //        //부서 변경 시에
        //        searchKeyword_In = null; //검색 텍스트 초기화
        //        SelectedStartDate_In = Convert.ToDateTime(product_dao.GetProductIn_MinDate(SelectedDept_In)); //날짜 컨트롤 최대, 최소 날짜로 설정
        //        SelectedEndDate_In = Convert.ToDateTime(product_dao.GetProductIn_MaxDate(SelectedDept_In));

        //        OnPropertyChanged("SelectedDept_In");
        //        showInListByDept();
        //    }
        //}




        ////입고 - 검색 수행
        //private ActionCommand inSearchCommand;
        //public ICommand InSearchCommand
        //{
        //    get
        //    {
        //        if (inSearchCommand == null)
        //        {
        //            inSearchCommand = new ActionCommand(InListSearch);
        //        }
        //        return inSearchCommand;
        //    }//get

        //}//Command

        //public void InListSearch()
        //{
        //    if (SelectedStartDate_In != null && SelectedEndDate_In != null)
        //    {
        //        Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In, selectedSearchType_In, searchKeyword_In, SelectedStartDate_In, SelectedEndDate_In));
        //    }
        //    else
        //    {
        //        MessageQueue.Enqueue("날짜를 모두 선택해주세요.", "닫기", (x) => { IsInOutEnable = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
        //        IsInOutEnable = true;
        //        //Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept_In, selectedSearchType_In, searchKeyword_In));
        //    }

        //}// InListSearch


        //입고 - 부서별 입고 리스트
        //public void showInListByDept()
        //{
        //    Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept));

        //}//showInListByDept

        //// 입고 - 시작, 끝 날짜 지정해서 입고 데이터 조회
        //public void ShowProductIn_By_Date()
        //{
        //    if (SelectedStartDate_In != null && SelectedEndDate_In != null)
        //    {
        //        Product_in = new ObservableCollection<ProductInOutModel>(product_dao.GetProductIn(SelectedDept, SelectedStartDate_In, SelectedEndDate_In));
        //    }
        //}//ShowProductIn_By_Date

        #endregion
        #region 출고 (미사용)

        //private ObservableCollection<ProductInOutModel> product_out;
        ////출고 내역을 담을 프로퍼티
        //public ObservableCollection<ProductInOutModel> Product_out
        //{
        //    get { return product_out; }
        //    set
        //    {
        //        product_out = value;
        //        OnPropertyChanged("Product_out");
        //    }
        //}

        ////출고 - 선택한 부서를 담을 프로퍼티
        //private DeptModel selectedDept_Out;
        //public DeptModel SelectedDept_Out
        //{
        //    get { return selectedDept_Out; }
        //    set
        //    {
        //        selectedDept_Out = value;

        //        //부서 변경 시에 
        //        searchKeyword_Out = null; // 검색 텍스트 초기화
        //        SelectedStartDate_Out = Convert.ToDateTime(product_dao.GetProductOut_MinDate(SelectedDept_Out)); //날짜 컨트롤 최대, 최소 날짜로 설정
        //        SelectedEndDate_Out = Convert.ToDateTime(product_dao.GetProductOut_MaxDate(SelectedDept_Out));

        //        OnPropertyChanged("SelectedDept_Out");
        //        showOutListByDept();
        //    }
        //}

        ////검색 텍스트 - 출고
        //private string _searchKeyword_Out;
        //public string searchKeyword_Out
        //{
        //    get { return _searchKeyword_Out; }
        //    set { _searchKeyword_Out = value; OnPropertyChanged("searchKeyword_Out"); }
        //}

        //public string selectedSearchType_Out { get; set; }


        ////출고
        ////시작일을 담을 프로퍼티
        //private DateTime? selectedStartDate_Out;
        //public DateTime? SelectedStartDate_Out
        //{
        //    get { return selectedStartDate_Out; }
        //    set
        //    {
        //        selectedStartDate_Out = value;

        //        ShowProductOut_By_Date();
        //        if (selectedStartDate_Out > selectedEndDate_Out)
        //        {
        //            SelectedStartDate_Out = SelectedEndDate_Out.Value.AddDays(-1);
        //        }
        //        OnPropertyChanged("SelectedStartDate_Out");
        //    }
        //}
        ////종료일을 담을 프로퍼티
        //private DateTime? selectedEndDate_Out;
        //public DateTime? SelectedEndDate_Out
        //{
        //    get { return selectedEndDate_Out; }
        //    set
        //    {
        //        selectedEndDate_Out = value;

        //        ShowProductOut_By_Date();
        //        if (selectedStartDate_Out > selectedEndDate_Out)
        //        {
        //            SelectedStartDate_Out = SelectedEndDate_Out.Value.AddDays(-1);
        //        }
        //        OnPropertyChanged("SelectedEndDate_Out");
        //    }
        //}

        ////출고- 검색 수행
        //private ActionCommand outSearchCommand;
        //public ICommand OutSearchCommand
        //{
        //    get
        //    {
        //        if (outSearchCommand == null)
        //        {
        //            outSearchCommand = new ActionCommand(OutListSearch);
        //        }
        //        return outSearchCommand;
        //    }//get

        //}//Command

        //public void OutListSearch()
        //{
        //    if (SelectedStartDate_Out != null && SelectedEndDate_Out != null)
        //    {
        //        Product_out = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept_Out, selectedSearchType_Out, searchKeyword_Out, SelectedStartDate_Out, SelectedEndDate_Out));
        //    }
        //    else
        //    {
        //        MessageQueue.Enqueue("날짜를 모두 선택해주세요.", "닫기", (x) => { IsInOutEnable = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));
        //        IsInOutEnable = true;
        //    }

        //}// OutListSearch


        ////출고 - 부서별 출고 리스트
        //public void showOutListByDept()
        //{
        //    Product_out = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept_Out));

        //}//showOutListByDept

        //// 출고 - 시작, 끝 날짜 지정해서 입고 데이터 조회
        //public void ShowProductOut_By_Date()
        //{
        //    if (SelectedStartDate_Out != null && SelectedEndDate_Out != null)
        //    {
        //        Product_out = new ObservableCollection<ProductInOutModel>(product_dao.GetProductOut(SelectedDept_Out, SelectedStartDate_Out, SelectedEndDate_Out));
        //    }
        //}//ShowProductOut_By_Date


        #endregion

        #region 스넥바
        //스넥바 
        private bool isInOutEnable = false;
        public bool IsInOutEnable
        {
            get { return isInOutEnable; }
            set
            {
                isInOutEnable = value;
                OnPropertyChanged("IsInOutEnable");
            }
        }

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
        #endregion

        
        

    }//class

}//namespace
