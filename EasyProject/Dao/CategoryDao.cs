using EasyProject.Model;
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
    public class CategoryDao : CommonDBConn
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public List<CategoryModel> GetCategories()
        {
            log.Info("GetCategories() invoked.");

            List<CategoryModel> list = new List<CategoryModel>();

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
                        cmd.CommandText = "SELECT category_name, category_id FROM CATEGORY ORDER BY category_id";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            CategoryModel dto = new CategoryModel()
                            {
                                Category_name = reader.GetString(0),
                                Category_id = reader.GetInt32(1)
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

        }//GetCategoris()

        public int GetCategoryID(string category_name)
        {
            log.Info("GetCategoryID(string) invoked");
            int category_id = 0;

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
                        cmd.CommandText = "SELECT category_id FROM CATEGORY WHERE category_name = :category_name";

                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            category_id = reader.GetInt32(0);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return category_id;
        }//GetCategoryID

        public List<CategoryModel> GetCategoriesvalues()
        {
            log.Info("GetCategoriesvalues() invoked.");
            List<CategoryModel> list = new List<CategoryModel>();

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
                        cmd.CommandText = "SELECT * FROM CATEGORY ORDER BY category_id";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            CategoryModel dto = new CategoryModel()
                            {
                                Category_id = reader.GetInt32(0),
                                Category_name = reader.GetString(1)
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

        }//GetCategoris()




        public bool IsExistsCategory(string Category_name)
        {
            log.Info("IsExistsCategory(string) invoked.");
            bool result = false;
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
                        cmd.CommandText = "SELECT * FROM category WHERE category_name = :category_name ";

                        cmd.Parameters.Add(new OracleParameter("category_name", Category_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }//if-else
                    }//using(cmd)
                }//using(conn)

            }//try
            catch(Exception e)
            {
                log.Error(e.Message);
            }//catch

            return result;

        }//IsExistsCategory


        public void AddCategory(string Category_name)
        {
            log.Info("AddCategory(string) invoked.");
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
                        cmd.CommandText = "INSERT INTO CATEGORY (category_name) " +
                                          "VALUES (:category_name) ";

                        cmd.Parameters.Add(new OracleParameter("category_name", Category_name));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)
                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }//AddCategory

        public void ChangeCategoryName(CategoryModel category_dto)
        {
            log.Info("ChangeCategoryName(CategoryModel category_dto) invoked.");
            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                string category_name = category_dto.Category_name;
                int? category_id = category_dto.Category_id;

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE CATEGORY " +
                                          "SET " +
                                          "category_name = :category_name " +
                                          "WHERE category_id = :category_id";

                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));
                        cmd.Parameters.Add(new OracleParameter("category_id", category_id));
                        cmd.ExecuteNonQuery();

                    }//using(cmd)
                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }//ChangeCategoryName


    }//class

}//namespace
