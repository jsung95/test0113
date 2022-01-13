using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyProject.Model;
using EasyProject.Dao;
using log4net;
using System.Collections.ObjectModel;
using Microsoft.Expression.Interactivity.Core;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace EasyProject.ViewModel
{
    public class LogViewModel : Notifier
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        LogDao log_dao = new LogDao();

        //검색 유형 콤보박스 목록을 담을 프로퍼티
        public string[] SearchTypeList_Event_LOG { get; set; }
        public string[] SearchTypeList_LogIn_LOG { get; set; }
        public string[] SearchTypeList_LogOut_LOG { get; set; }

        public LogViewModel()
        {
            log.Info("Constructor LogViewModel() invoked.");

            //로그 데이터 초기화
            Event_Logs = new ObservableCollection<LogModel>();
            LogIn_Logs = new ObservableCollection<LogModel>();
            LogOut_Logs = new ObservableCollection<LogModel>();

            //EVENT 검색 유형 콤보박스 목록
            SearchTypeList_Event_LOG = new[] { "사번", "사용자명", "클래스", "메소드", "내용" };
            SelectedSearchType_Event_Log = SearchTypeList_Event_LOG[0]; // index 0번 item으로 초기화

            //LOGIN
            SearchTypeList_LogIn_LOG = new[] { "사용자명", "부서명", "IP주소" };
            SelectedSearchType_LogIn_Log = SearchTypeList_LogIn_LOG[0];

            //LOGOUT
            SearchTypeList_LogOut_LOG = new[] { "사용자명", "부서명", "IP주소" };
            SelectedSearchType_LogOut_Log = SearchTypeList_LogOut_LOG[0];


            //날짜
            //EVENT
            SelectedStartDate_Event_Log = Convert.ToDateTime(log_dao.GetEventLogs_Min_Date());
            SelectedEndDate_Event_Log = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
            //LOGIN
            SelectedStartDate_LogIn_Log = Convert.ToDateTime(log_dao.GetLoginLogs_Min_Date());
            SelectedEndDate_LogIn_Log = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
            //LOGOUT
            SelectedStartDate_LogOut_Log = Convert.ToDateTime(log_dao.GetLoginLogs_Min_Date());
            SelectedEndDate_LogOut_Log = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            //대시보드
            TodayLoginPrint();
            TodayLogoutPrint();


            //coount
            CountLevel_INFO = log_dao.GetLogLevel_INFO(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);
            CountLevel_ERROR = log_dao.GetLogLevel_ERROR(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);

        }//LogViewModel()





        #region EVENT_LOG

        //INFO, ERROR 갯수 프로퍼티 
        private int countLevel_INFO;
        public int CountLevel_INFO
        {
            get { return countLevel_INFO; }
            set 
            {
                countLevel_INFO = value; 
                OnPropertyChanged("CountLevel_INFO"); 
            }
        }
        private int countLevel_ERROR;
        public int CountLevel_ERROR
        {
            get { return countLevel_ERROR; }
            set 
            {
                countLevel_ERROR = value; 
                OnPropertyChanged("CountLevel_ERROR"); 
            }
        }



        //검색 타입
        public string SelectedSearchType_Event_Log { get; set; }

        //검색 텍스트
        private string searchKeyword_Event_Log;
        public string SearchKeyword_Event_Log
        {
            get { return searchKeyword_Event_Log; }
            set 
            {
                searchKeyword_Event_Log = value; 
                OnPropertyChanged("SearchKeyword_Event_Log"); 
            }
        }



        //EventLog 시작날짜
        private DateTime? selectedStartDate_Event_Log;
        public DateTime? SelectedStartDate_Event_Log
        {
            get { return selectedStartDate_Event_Log; }
            set 
            { 
                selectedStartDate_Event_Log = value;
                SearchKeyword_Event_Log = null;
                //GetEventLogs();

                //coount
                CountLevel_INFO = log_dao.GetLogLevel_INFO(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);
                CountLevel_ERROR = log_dao.GetLogLevel_ERROR(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);

                OnPropertyChanged("SelectedStartDate_Event_Log"); 
            }
        }

        //EventLog 끝날짜
        private DateTime? selectedEndDate_Event_Log;
        public DateTime? SelectedEndDate_Event_Log
        {
            get { return selectedEndDate_Event_Log; }
            set 
            {
                selectedEndDate_Event_Log = value;
                SearchKeyword_Event_Log = null;
                //GetEventLogs();

                //coount
                CountLevel_INFO = log_dao.GetLogLevel_INFO(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);
                CountLevel_ERROR = log_dao.GetLogLevel_ERROR(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);

                OnPropertyChanged("SelectedEndDate_Event_Log"); 
            }
        }





        //로그 데이터들을 담을 프로퍼티
        private ObservableCollection<LogModel> event_Logs;
        public ObservableCollection<LogModel> Event_Logs
        {
            get { return event_Logs; }
            set 
            {
                event_Logs = value; 
                OnPropertyChanged("Event_Logs"); 
            }
        }

        private void GetEventLogs()
        {
            log.Info("GetEventLogs() invoked.");
            try
            {
                if(SelectedStartDate_Event_Log != null && SelectedEndDate_Event_Log != null)
                {
                    Event_Logs = new ObservableCollection<LogModel>(log_dao.GetAllLogs(SelectedStartDate_Event_Log, SelectedEndDate_Event_Log));
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//GetEventLogs




        private ActionCommand searchEventLogsCommand;
        public ICommand SearchEventLogsCommand
        {
            get
            {
                if (searchEventLogsCommand == null)
                {
                    searchEventLogsCommand = new ActionCommand(GetSearchEvnetLogs);
                }
                return searchEventLogsCommand;
            }//get
        }

        private void GetSearchEvnetLogs()
        {
            log.Info("GetSearchEvnetLogs() invoked.");
            try
            {
                if (SelectedStartDate_Event_Log != null && SelectedEndDate_Event_Log != null)
                {
                    Event_Logs = new ObservableCollection<LogModel>(log_dao.Search_GetLogs(SelectedSearchType_Event_Log, SearchKeyword_Event_Log, SelectedStartDate_Event_Log, SelectedEndDate_Event_Log));
                    //coount
                    CountLevel_INFO = log_dao.GetLogLevel_INFO(SelectedSearchType_Event_Log, SearchKeyword_Event_Log, SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);
                    CountLevel_ERROR = log_dao.GetLogLevel_ERROR(SelectedSearchType_Event_Log, SearchKeyword_Event_Log, SelectedStartDate_Event_Log, SelectedEndDate_Event_Log);
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//GetSearchEvnetLogs

        #endregion

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        #region LOGIN



        //검색 타입
        public string SelectedSearchType_LogIn_Log { get; set; }


        //검색 텍스트
        private string searchKeyword_LogIn_Log;
        public string SearchKeyword_LogIn_Log
        {
            get { return searchKeyword_LogIn_Log; }
            set
            {
                searchKeyword_LogIn_Log = value;
                OnPropertyChanged("SearchKeyword_LogIn_Log");
            }
        }


        //Loginout 시작날짜
        private DateTime? selectedStartDate_LogIn_Log;
        public DateTime? SelectedStartDate_LogIn_Log
        {
            get { return selectedStartDate_LogIn_Log; }
            set
            {
                selectedStartDate_LogIn_Log = value;
                SearchKeyword_LogIn_Log = null;
                GetLogInLogs();

                OnPropertyChanged("SelectedStartDate_LogIn_Log");
            }
        }

        //LogInOut 끝날짜
        private DateTime? selectedEndDate_LogIn_Log;
        public DateTime? SelectedEndDate_LogIn_Log
        {
            get { return selectedEndDate_LogIn_Log; }
            set
            {
                selectedEndDate_LogIn_Log = value;
                SearchKeyword_LogIn_Log = null;
                GetLogInLogs();

                OnPropertyChanged("SelectedEndDate_LogIn_Log");
            }
        }


        //로그 데이터들을 담을 프로퍼티
        private ObservableCollection<LogModel> logIn_Logs;
        public ObservableCollection<LogModel> LogIn_Logs
        {
            get { return logIn_Logs; }
            set
            {
                logIn_Logs = value;
                OnPropertyChanged("LogIn_Logs");
            }
        }


        private void GetLogInLogs()
        {
            log.Info("GetLogInLogs() invoked.");
            try
            {
                if (SelectedStartDate_LogIn_Log != null && SelectedEndDate_LogIn_Log != null)
                {
                    LogIn_Logs = new ObservableCollection<LogModel>(log_dao.GetLoginLogs(SelectedStartDate_LogIn_Log, SelectedEndDate_LogIn_Log));
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//GetLogInLogs




        private ActionCommand searchLoginLogsCommand;
        public ICommand SearchLoginLogsCommand
        {
            get
            {
                if (searchLoginLogsCommand == null)
                {
                    searchLoginLogsCommand = new ActionCommand(GetSearchLoginLogs);
                }
                return searchLoginLogsCommand;
            }//get
        }

        private void GetSearchLoginLogs()
        {
            log.Info("GetSearchLoginLogs() invoked.");
            try
            {
                if (SelectedStartDate_LogIn_Log != null && SelectedEndDate_LogIn_Log != null)
                {
                    LogIn_Logs = new ObservableCollection<LogModel>(log_dao.GetLoginLogs(SelectedSearchType_LogIn_Log, SearchKeyword_LogIn_Log, SelectedStartDate_LogIn_Log, SelectedEndDate_LogIn_Log));
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//GetSearchLoginLogs


        #endregion


        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        #region LOGOUT

        //검색 타입
        public string SelectedSearchType_LogOut_Log { get; set; }


        //검색 텍스트
        private string searchKeyword_LogOut_Log;
        public string SearchKeyword_LogOut_Log
        {
            get { return searchKeyword_LogOut_Log; }
            set
            {
                searchKeyword_LogOut_Log = value;
                OnPropertyChanged("SearchKeyword_LogOut_Log");
            }
        }


        //Loginout 시작날짜
        private DateTime? selectedStartDate_LogOut_Log;
        public DateTime? SelectedStartDate_LogOut_Log
        {
            get { return selectedStartDate_LogOut_Log; }
            set
            {
                selectedStartDate_LogOut_Log = value;
                SearchKeyword_LogOut_Log = null;
                GetLogOutLogs();

                OnPropertyChanged("SelectedStartDate_LogOut_Log");
            }
        }

        //LogInOut 끝날짜
        private DateTime? selectedEndDate_LogOut_Log;
        public DateTime? SelectedEndDate_LogOut_Log
        {
            get { return selectedEndDate_LogOut_Log; }
            set
            {
                selectedEndDate_LogOut_Log = value;
                SearchKeyword_LogOut_Log = null;
                GetLogOutLogs();

                OnPropertyChanged("SelectedEndDate_LogOut_Log");
            }
        }


        //로그 데이터들을 담을 프로퍼티
        private ObservableCollection<LogModel> logOut_Logs;
        public ObservableCollection<LogModel> LogOut_Logs
        {
            get { return logOut_Logs; }
            set
            {
                logOut_Logs = value;
                OnPropertyChanged("LogOut_Logs");
            }
        }


        private void GetLogOutLogs()
        {
            log.Info("GetLogOutLogs() invoked.");
            try
            {
                if (SelectedStartDate_LogOut_Log != null && SelectedEndDate_LogOut_Log != null)
                {
                    LogOut_Logs = new ObservableCollection<LogModel>(log_dao.GetLogOutLogs(SelectedStartDate_LogOut_Log, SelectedEndDate_LogOut_Log));
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//GetLogOutLogs




        private ActionCommand searchLogOutLogsCommand;
        public ICommand SearchLogOutLogsCommand
        {
            get
            {
                if (searchLogOutLogsCommand == null)
                {
                    searchLogOutLogsCommand = new ActionCommand(GetSearchLogOutLogs);
                }
                return searchLogOutLogsCommand;
            }//get
        }

        private void GetSearchLogOutLogs()
        {
            log.Info("GetSearchLogOutLogs() invoked.");
            try
            {
                if (SelectedStartDate_LogOut_Log != null && SelectedEndDate_LogOut_Log != null)
                {
                    LogOut_Logs = new ObservableCollection<LogModel>(log_dao.GetLogOutLogs(SelectedSearchType_LogOut_Log, SearchKeyword_LogOut_Log, SelectedStartDate_LogOut_Log, SelectedEndDate_LogOut_Log));
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//GetSearchLogOutLogs

        #endregion

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///
        #region For XAML

        //그리드버튼 누르면 접혀지는 프로퍼티
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

        //그래프버튼 누르면 접혀지는 프로퍼티
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

        #endregion


        #region 그래프
        // 7일간 하루로그인 횟수 그래프
        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection               //그래프 큰 틀 만드는거
        {
            get { return seriesCollection; }
            set
            {
                seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }
        // 로그인 유지 시간 그래프
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
        

        // 대시보드 프로퍼티
        public ChartValues<int> Values { get; set; }
        public List<string> BarLabels { get; set; }       //string[]
        public ChartValues<int> Values1 { get; set; }
        public List<string> BarLabels1 { get; set; }       //string[]
        public ChartValues<int> Values2 { get; set; }
        public List<string> BarLabels2 { get; set; }       //string[]
        public Func<double, string> Formatter { get; set; }

        //하루 로그인 횟수(7일 제한) 그래프
        public void TodayLoginPrint()
        {
            log.Info("TodayLoginPrint() invoked.");
            try
            {
                ChartValues<int> mount = new ChartValues<int>();   //y축들어갈 임시 값
                SeriesCollection = new SeriesCollection();   //대시보드 틀
                                                             //Console.WriteLine(selected.Dept_id); 
                List<LogModel> list_xy = log_dao.Logintotal();


                foreach (var item in list_xy)
                {
                    mount.Add((int)item.Log_total);
                }
                Values = new ChartValues<int> { };

                SeriesCollection.Add(new LineSeries
                {
                    Title = "로그인 횟수",   //+ i
                    Values = mount,
                });
                BarLabels = new List<string>() { };                           //x축출력
                foreach (var item in list_xy)
                {
                    BarLabels.Add(item.Today_Log_date);
                }
                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }

        //하루 로그아웃 횟수(7일 제한) 그래프
        public void TodayLogoutPrint()
        {
            log.Info("TodayLogoutPrint() invoked.");
            try
            {
                ChartValues<int> mount = new ChartValues<int>();   //y축들어갈 임시 값
                SeriesCollection1 = new SeriesCollection();   //대시보드 틀
                                                              //Console.WriteLine(selected.Dept_id); 
                List<LogModel> list_xy = log_dao.Logouttotal();


                foreach (var item in list_xy)
                {
                    mount.Add((int)item.Log_total);
                }
                Values1 = new ChartValues<int> { };

                SeriesCollection1.Add(new LineSeries
                {
                    Title = "로그아웃 횟수",   //+ i
                    Values = mount,
                });
                BarLabels1 = new List<string>() { };                           //x축출력
                foreach (var item in list_xy)
                {
                    BarLabels1.Add(item.Today_Log_date);
                }
                Formatter = value => value.ToString("N0");   //문자열 10진수 변환
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
        #endregion

    }//class

}//namespace
