using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using EasyProject.Model;
using log4net;

namespace EasyProject.Dao
{
    public class LogDao : CommonDBConn, ILogDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public List<LogModel> GetAllLogs()
        {
            log.Info("GetAllLogs() invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                                          "FROM EVENT_LOG " +
                                          "ORDER BY log_no";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Log_no = reader.GetInt32(0),
                                User_no = reader.GetString(1),
                                User_name = reader.GetString(2),
                                User_auth = reader.GetString(3),
                                User_ip = reader.GetString(4),
                                User_nation = reader.GetString(5),
                                Log_date = reader.GetDateTime(6),
                                Log_level = reader.GetString(7),
                                Log_class = reader.GetString(8),
                                Log_method = reader.GetString(9),
                                Message = reader.GetString(10)
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;

        }//GetAllLogs()


        public List<LogModel> GetAllLogs(DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetAllLogs() invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT /*+ INDEX(EVENT_LOG_INDEX)*/ * " +
                                          "FROM EVENT_LOG " +
                                          "WHERE log_date BETWEEN :start_date AND :end_date +1 " +
                                          "ORDER BY log_no";

                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Log_no = reader.GetInt32(0),
                                User_no = reader.GetString(1),
                                User_name = reader.GetString(2),
                                User_auth = reader.GetString(3),
                                User_ip = reader.GetString(4),
                                User_nation = reader.GetString(5),
                                Log_date = reader.GetDateTime(6),
                                Log_level = reader.GetString(7),
                                Log_class = reader.GetString(8),
                                Log_method = reader.GetString(9),
                                Message = reader.GetString(10)
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetAllLogs


        public List<LogModel> Search_GetLogs(string search_type, string search_keyword)
        {
            log.Info("GetAllLogs(string, string) invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                                          "FROM EVENT_LOG " +
                                          "WHERE " +
                                          "((:search_type = '사번') AND (user_no LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '사용자명') AND (user_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '클래스') AND (log_class LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '메소드') AND (log_method LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '내용') AND (message LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY log_no";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_type", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_keyword));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Log_no = reader.GetInt32(0),
                                User_no = reader.GetString(1),
                                User_name = reader.GetString(2),
                                User_auth = reader.GetString(3),
                                User_ip = reader.GetString(4),
                                User_nation = reader.GetString(5),
                                Log_date = reader.GetDateTime(6),
                                Log_level = reader.GetString(7),
                                Log_class = reader.GetString(8),
                                Log_method = reader.GetString(9),
                                Message = reader.GetString(10)
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//Search_GetLogs


        public List<LogModel> Search_GetLogs(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetAllLogs(string, string, DateTime?, DateTime?) invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                                          "FROM EVENT_LOG " +
                                          "WHERE " +
                                          "((:search_type = '사번' AND log_date BETWEEN :start_date AND :end_date) AND (user_no LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '사용자명' AND log_date BETWEEN :start_date AND :end_date) AND (user_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '클래스' AND log_date BETWEEN :start_date AND :end_date) AND (log_class LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '메소드' AND log_date BETWEEN :start_date AND :end_date) AND (log_method LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '내용' AND log_date BETWEEN :start_date AND :end_date) AND (message LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY log_no";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_type", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_keyword));
                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Log_no = reader.GetInt32(0),
                                User_no = reader.GetString(1),
                                User_name = reader.GetString(2),
                                User_auth = reader.GetString(3),
                                User_ip = reader.GetString(4),
                                User_nation = reader.GetString(5),
                                Log_date = reader.GetDateTime(6),
                                Log_level = reader.GetString(7),
                                Log_class = reader.GetString(8),
                                Log_method = reader.GetString(9),
                                Message = reader.GetString(10)
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//Search_GetLogs



        public List<LogModel> GetLoginLogs()
        {
            log.Info("GetLoginLogs() invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                            "FROM LOGIN_LOG " +
                            "ORDER BY login_log_no";

                        cmd.Parameters.Add(new OracleParameter());

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Login_log_no = reader.GetInt32(0),
                                Login_log_ip = reader.GetString(1),
                                Login_log_nation = reader.GetString(2),
                                Login_log_date = reader.GetDateTime(3),
                                Nurse_no = reader.GetString(4),
                                Nurse_name = reader.GetString(5),
                                Nurse_auth = reader.GetString(6),
                                Dept_id = reader.GetInt32(7),
                                Dept_name = reader.GetString(8)
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetLoginLogs

        public List<LogModel> GetLogOutLogs()
        {
            log.Info("GetLogOutLogs() invoked.");
            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                            "FROM LOGOUT_LOG " +
                            "ORDER BY logout_log_no ";

                        cmd.Parameters.Add(new OracleParameter());

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Logout_log_no = reader.GetInt32(0),
                                Logout_log_ip = reader.GetString(1),
                                Logout_log_nation = reader.GetString(2),
                                Logout_log_date = reader.GetDateTime(3),
                                Nurse_no = reader.GetString(4),
                                Nurse_name = reader.GetString(5),
                                Nurse_auth = reader.GetString(6),
                                Dept_id = reader.GetInt32(7),
                                Dept_name = reader.GetString(8)
                            };
                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetLogOutLogs

        public List<LogModel> GetLoginLogs(DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLoginLogs(DateTime?, DateTime?) invoked.");
            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                            "FROM LOGIN_LOG " +
                            "WHERE login_log_date BETWEEN :start_date AND :end_date +1 " +
                            "ORDER BY login_log_no ";

                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Login_log_no = reader.GetInt32(0),
                                Login_log_ip = reader.GetString(1),
                                Login_log_nation = reader.GetString(2),
                                Login_log_date = reader.GetDateTime(3),
                                Nurse_no = reader.GetString(4),
                                Nurse_name = reader.GetString(5),
                                Nurse_auth = reader.GetString(6),
                                Dept_id = reader.GetInt32(7),
                                Dept_name = reader.GetString(8)
                            };
                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetLoginLogs

        public List<LogModel> GetLoginLogs(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLoginLogs(string, string, DateTime?, DateTime?) invoked.");
            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                            "FROM LOGIN_LOG " +
                            "WHERE " +
                            "((:search_type = '사용자명' AND login_log_date BETWEEN :start_date AND :end_date) AND (nurse_name LIKE '%'||:search_text||'%')) " +
                            "OR " +
                            "((:search_type = '부서명' AND login_log_date BETWEEN :start_date AND :end_date) AND (dept_name LIKE '%'||:search_text||'%')) " +
                            "OR " +
                            "((:search_type = 'IP주소' AND login_log_date BETWEEN :start_date AND :end_date) AND (login_log_ip LIKE '%'||:search_text||'%')) " +
                            "ORDER BY login_log_no ";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_type", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_keyword));

                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Login_log_no = reader.GetInt32(0),
                                Login_log_ip = reader.GetString(1),
                                Login_log_nation = reader.GetString(2),
                                Login_log_date = reader.GetDateTime(3),
                                Nurse_no = reader.GetString(4),
                                Nurse_name = reader.GetString(5),
                                Nurse_auth = reader.GetString(6),
                                Dept_id = reader.GetInt32(7),
                                Dept_name = reader.GetString(8)
                            };
                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetLoginLogs

        public List<LogModel> GetLogOutLogs(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLogOutLogs(string, string, DateTime?, DateTime?) invoked.");
            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                            "FROM LOGOUT_LOG " +
                            "WHERE " +
                            "((:search_type = '사용자명' AND logout_log_date BETWEEN :start_date AND :end_date) AND (nurse_name LIKE '%'||:search_text||'%')) " +
                            "OR " +
                            "((:search_type = '부서명' AND logout_log_date BETWEEN :start_date AND :end_date) AND (dept_name LIKE '%'||:search_text||'%')) " +
                            "OR " +
                            "((:search_type = 'IP주소' AND logout_log_date BETWEEN :start_date AND :end_date) AND (logout_log_ip LIKE '%'||:search_text||'%')) " +
                            "ORDER BY logout_log_no ";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_type", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_keyword));
                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Logout_log_no = reader.GetInt32(0),
                                Logout_log_ip = reader.GetString(1),
                                Logout_log_nation = reader.GetString(2),
                                Logout_log_date = reader.GetDateTime(3),
                                Nurse_no = reader.GetString(4),
                                Nurse_name = reader.GetString(5),
                                Nurse_auth = reader.GetString(6),
                                Dept_id = reader.GetInt32(7),
                                Dept_name = reader.GetString(8)
                            };
                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetLogOutLogs



        public List<LogModel> GetLogOutLogs(DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLogOutLogs(DateTime?, DateTime?) invoked.");
            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                            "FROM LOGOUT_LOG " +
                            "WHERE logout_log_date BETWEEN :start_date AND :end_date +1 " +
                            "ORDER BY logout_log_no ";

                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            LogModel dto = new LogModel()
                            {
                                Logout_log_no = reader.GetInt32(0),
                                Logout_log_ip = reader.GetString(1),
                                Logout_log_nation = reader.GetString(2),
                                Logout_log_date = reader.GetDateTime(3),
                                Nurse_no = reader.GetString(4),
                                Nurse_name = reader.GetString(5),
                                Nurse_auth = reader.GetString(6),
                                Dept_id = reader.GetInt32(7),
                                Dept_name = reader.GetString(8)
                            };
                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;
        }//GetLogOutLogs

        public string GetEventLogs_Min_Date()
        {
            log.Info("GetEventLogs_Min_Date() invoked.");
            string result = null;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT MIN(log_date) " +
                                          "FROM EVENT_LOG " +
                                          "ORDER BY log_date ";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return result;
        }//GetEventLogs_Min_Date

        public string GetLoginLogs_Min_Date()
        {
            log.Info("GetLoginLogs_Min_Date() invoked.");
            string result = null;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT MIN(login_log_date) " +
                                          "FROM LOGIN_LOG " +
                                          "ORDER BY login_log_date ";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return result;
        }//GetLoginLogs_Min_Date

        public string GetLogOutLogs_MinDate()
        {
            log.Info("GetLogOutLogs_MinDate() invoked.");
            string result = null;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT MIN(logout_log_date) " +
                                          "FROM LOGOUT_LOG " +
                                          "ORDER BY logout_log_date ";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return result;
        }//GetLogOutLogs_MinDate


        public List<LogModel> Logintotal()
        {
            log.Info("GetAllLogs() invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                                          "FROM " +
                                          "(SELECT TO_CHAR(login_log_date, 'YYYYMMDD') as rnum, count(nurse_no) " +
                                          "FROM LOGIN_LOG " +
                                          "GROUP BY TO_CHAR(login_log_date, 'YYYYMMDD') " +
                                          "ORDER BY 1 desc)A " +
                                          "WHERE ROWNUM <= 7 " +
                                          "ORDER BY A.rnum";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Today_Log_date = reader.GetString(0);
                            int Log_total = reader.GetInt32(1);
                            LogModel dto = new LogModel()
                            {
                                Today_Log_date = Today_Log_date,
                                Log_total = Log_total
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;

        }//Logintotal()

        public List<LogModel> Logouttotal()
        {
            log.Info("GetAllLogs() invoked.");

            List<LogModel> list = new List<LogModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                                          "FROM " +
                                          "(SELECT TO_CHAR(logout_log_date, 'YYYYMMDD') as rnum, count(nurse_no) " +
                                          "FROM LOGOUT_LOG " +
                                          "GROUP BY TO_CHAR(logout_log_date, 'YYYYMMDD') " +
                                          "ORDER BY 1 desc)A " +
                                          "WHERE ROWNUM <= 7 " +
                                          "ORDER BY A.rnum";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Today_Log_date = reader.GetString(0);
                            int Log_total = reader.GetInt32(1);
                            LogModel dto = new LogModel()
                            {
                                Today_Log_date = Today_Log_date,
                                Log_total = Log_total
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return list;

        }//Logouttotal()

        public int GetLogLevel_INFO(DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLogLevel_INFO(DateTime?, DateTime?) invoked.");

            int result = 0;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT COUNT(log_level) " +
                                          "FROM event_log " +
                                          "WHERE log_level = 'INFO' " +
                                          "AND log_date BETWEEN :start_date AND :end_date ";

                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return result;
        }//GetLogLevel_INFO

        public int GetLogLevel_INFO(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLogLevel_INFO(string, string, DateTime?, DateTime?) invoked.");

            int result = 0;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT COUNT(log_level) " +
                                          "FROM EVENT_LOG " +
                                          "WHERE " +
                                          "((:search_type = '사번' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'INFO') AND (user_no LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '사용자명' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'INFO') AND (user_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '클래스' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'INFO') AND (log_class LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '메소드' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'INFO') AND (log_method LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '내용' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'INFO') AND (message LIKE '%'||:search_text||'%')) ";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_type", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_keyword));
                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return result;
        }//GetLogLevel_INFO

        public int GetLogLevel_ERROR(DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLogLevel_ERROR(DateTime?, DateTime?) invoked.");

            int result = 0;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT COUNT(log_level) " +
                                          "FROM event_log " +
                                          "WHERE log_level = 'ERROR' " +
                                          "AND log_date BETWEEN :start_date AND :end_date ";

                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return result;
        }//GetLogLevel_ERROR

        public int GetLogLevel_ERROR(string search_type, string search_keyword, DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetLogLevel_ERROR(string, string, DateTime?, DateTime?) invoked.");

            int result = 0;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT COUNT(log_level) " +
                                          "FROM EVENT_LOG " +
                                          "WHERE " +
                                          "((:search_type = '사번' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'ERROR') AND (user_no LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '사용자명' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'ERROR') AND (user_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '클래스' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'ERROR') AND (log_class LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '메소드' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'ERROR') AND (log_method LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                          "((:search_type = '내용' AND log_date BETWEEN :start_date AND :end_date AND log_level = 'ERROR') AND (message LIKE '%'||:search_text||'%')) ";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_type", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_keyword));
                        DateTime? start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 00, 00, 01);
                        DateTime? end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 23, 59, 59);
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

            return result;
        }//GetLogLevel_ERROR


    }//class

}//namespace
