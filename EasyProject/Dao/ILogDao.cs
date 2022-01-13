using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyProject.Model;

namespace EasyProject.Dao
{
    public interface ILogDao
    {
        //전체 로그 데이터
        List<LogModel> GetAllLogs();
        List<LogModel> GetAllLogs(DateTime? start_date, DateTime? end_date); // - 날짜 검색

        //전체 로그데이터 - 검색
        List<LogModel> Search_GetLogs(string search_type, string search_keyword); // - 단순 검색
        List<LogModel> Search_GetLogs(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date); // - 날짜 검색
        string GetEventLogs_Min_Date();

        //로그인 데이터
        List<LogModel> GetLoginLogs();
        List<LogModel> GetLoginLogs(DateTime? start_date, DateTime? end_date);
        List<LogModel> GetLoginLogs(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date);
        string GetLoginLogs_Min_Date();

        //로그아웃 데이터
        List<LogModel> GetLogOutLogs();
        List<LogModel> GetLogOutLogs(DateTime? start_date, DateTime? end_date);
        List<LogModel> GetLogOutLogs(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date);
        string GetLogOutLogs_MinDate();

        //그래프
        List<LogModel> Logintotal();
        List<LogModel> Logouttotal();

        // INFO, ERROR LEVEL COUNT
        int GetLogLevel_INFO( DateTime? start_date, DateTime? end_date);
        int GetLogLevel_INFO(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date);
        int GetLogLevel_ERROR(DateTime? start_date, DateTime? end_date);
        int GetLogLevel_ERROR(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date);


    }//interface

}//namespace
