using EasyProject.Model;
using log4net;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Dao
{
    public class OrderDao : CommonDBConn, IOrderDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public List<DeptModel> GetDeptModels()
        {
            log.Info("GetDeptModels() invoked.");
            List<DeptModel> list = new List<DeptModel>();
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

                        cmd.CommandText = "SELECT DEPT_NAME FROM DEPT";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dept_name = reader.GetString(0); // 선택된 데이터셋 0번 컬럼 = 부서 이름 -> 문자열 변수에 담음.

                            DeptModel dept = new DeptModel() // DeptModel 객체를 생성, 필드값에 dept_name 넣어줌
                            {
                                Dept_name = dept_name
                            };
                            list.Add(dept); // 리스트에 추가
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list; // DeptModel 객체들이 담긴 list 리턴
        }
    }//OrderDao
}//namespace
