using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Model
{
    public class LogModel : Notifier
    {
        //EVENT LOG
        public int? Log_no { get; set; }
        public string User_no { get; set; }
        public string User_name { get; set; }
        public string User_auth { get; set; }
        public string User_ip { get; set; }
        public string User_nation { get; set; }
        public DateTime? Log_date { get; set; }
        public string Log_level { get; set; }
        public string Log_class { get; set; }
        public string Log_method { get; set; }
        public string Message { get; set; }


        //LOGIN
        public int? Login_log_no { get; set; }
        public string Login_log_ip { get; set; }
        public string Login_log_nation { get; set; }
        public DateTime? Login_log_date { get; set; }
        public string Nurse_no { get; set; }
        public string Nurse_name { get; set; }
        public string Nurse_auth { get; set; }
        public int? Dept_id { get; set; }
        public string Dept_name { get; set; }


        //LOGOUT
        public int? Logout_log_no { get; set; }
        public string Logout_log_ip { get; set; }
        public string Logout_log_nation { get; set; }
        public DateTime? Logout_log_date { get; set; }

        //대시보드

        public int? Log_total { get; set; }
        public string Today_Log_date { get; set; }

    }//class

}//namespace
