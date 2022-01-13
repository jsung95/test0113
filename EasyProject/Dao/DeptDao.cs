﻿using EasyProject.Model;
using EasyProject.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using log4net;

namespace EasyProject.Dao
{
    public class DeptDao : CommonDBConn, IDeptDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public List<DeptModel> GetDepts()
        {
            log.Info("GetDepts() invoked.");
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
                        cmd.CommandText = "SELECT * FROM DEPT";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            DeptModel dto = new DeptModel()
                            {
                                Dept_id = reader.GetInt32(0),
                                Dept_name = reader.GetString(1),
                                Dept_phone = reader.GetString(2),
                                Dept_status = reader.GetString(3)
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetDepts()



        public DeptModel GetDeptName(int dept_id)
        {
            log.Info("GetDeptName(int) invoked.");
            DeptModel dto = new DeptModel();
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
                        cmd.CommandText = "SELECT dept_name FROM DEPT WHERE dept_id = :dept_id";

                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_id));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            dto.Dept_name = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return dto;
        }//GetDeptName

        public string GetDeptName_Return_String(int dept_id)
        {
            log.Info("GetDeptName_Return_String(int) invoked.");
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
                        cmd.CommandText = "SELECT dept_name FROM DEPT WHERE dept_id = :dept_id";

                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_id));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return result;

        }//GetDeptName_Return_Int

    }//class

}//namespace
