using EasyProject.Model;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using log4net;

namespace EasyProject.Dao
{
    public class ProductDao : CommonDBConn, IProductDao //DB연결 Class 및 인터페이스 상속
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public List<ProductShowModel> GetProducts()
        {
            log.Info("GetProducts() invoked.");
            Console.WriteLine("GetProducts !");
            List<ProductShowModel> list = new List<ProductShowModel>();
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, I.imp_dept_count, P.prod_expire, P.prod_id, I.imp_dept_id " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "INNER JOIN DEPT D " +
                                          "ON I.dept_id = D.dept_id " +
                                          "WHERE D.dept_status != '폐지' " +
                                          "AND D.dept_name = (select dept_name from dept where dept_id = :dept_id) " +
                                          "ORDER BY P.prod_code, P.prod_expire ";

                        cmd.Parameters.Add(new OracleParameter("dept_id", App.nurse_dto.Dept_id));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? imp_dept_count = reader.GetInt32(4);
                            DateTime prod_expire = reader.GetDateTime(5);
                            int? prod_id = reader.GetInt32(6);
                            int? imp_dept_id = reader.GetInt32(7);


                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Imp_dept_count = imp_dept_count,
                                Prod_expire = prod_expire,
                                Prod_id = prod_id,
                                Imp_dept_id = imp_dept_id,
                                Prod_remainexpire = (prod_expire.Date - DateTime.Now.Date).Days

                            };
                            Console.WriteLine(dto);

                            list.Add(dto);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProduct()


        public bool IsProductDuplicateCheck(ProductModel product_dto, int userDeptId)
        {
            log.Info("IsProductDuplicateCheck(ProductModel product_dto, int userDeptId) invoked.");

            bool isDuplicated = false;
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
                        cmd.CommandText = "SELECT prod_code " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "WHERE P.prod_code = :prod_code " +
                                          "AND I.dept_id = :dept_id " +
                                          "AND P.prod_name = :prod_name " +
                                          "AND P.category_id = :category_id " +
                                          "AND P.prod_price = :prod_price " +
                                          "AND P.prod_expire = TO_DATE(:expire, 'YYYYMMDD') ";
                        
                        cmd.Parameters.Add(new OracleParameter("prod_code", product_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("dept_id", (Int32)userDeptId));
                        cmd.Parameters.Add(new OracleParameter("prod_name", product_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_id", product_dto.Category_id));
                        cmd.Parameters.Add(new OracleParameter("prod_price", product_dto.Prod_price));
                        //cmd.Parameters.Add(new OracleParameter("prod_expire", product_dto.Prod_expire));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = product_dto.Prod_expire.Month.ToString();
                        if (product_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + product_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = product_dto.Prod_expire.Day.ToString();
                        if (product_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + product_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = product_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));


                        Console.WriteLine(product_dto.Prod_expire);
                        Console.WriteLine(product_dto.Category_id + "카페고리idglglglglglglglglglglglg");
                        OracleDataReader reader = cmd.ExecuteReader();
                        Console.WriteLine("readersize: " + reader.RowSize);
                        if (reader.Read() == false)
                        {
                            //없는 제품을 추가하려고 하는 경우(access)
                            isDuplicated = false;
                        }
                        else
                        {
                            //이미 입력된 제품을 추가하려고 하는 경우(deny)
                            isDuplicated = true;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return isDuplicated;

        }//IdCheck


        public bool IsProductDuplicateCheck(ProductModel product_dto, CategoryModel cateogry_dto)//오버로딩
        {
            log.Info("IsProductDuplicateCheck(ProductModel, CategoryModel) invoked.");
            bool isDuplicated = false;
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
                        cmd.CommandText = "SELECT prod_code " +
                                          "FROM PRODUCT " +
                                          "WHERE prod_code = :prod_code " +
                                          "AND prod_name = :prod_name " +
                                          "AND category_id = (SELECT category_id FROM category WHERE category_name = :category_name) " +
                                          "AND prod_price = :prod_price " +
                                          "AND prod_expire = TO_DATE(:expire, 'YYYYMMDD') ";
                        cmd.Parameters.Add(new OracleParameter("prod_code", product_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("prod_name", product_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", cateogry_dto.Category_name));
                        cmd.Parameters.Add(new OracleParameter("prod_price", product_dto.Prod_price));
                        //cmd.Parameters.Add(new OracleParameter("prod_expire", product_dto.Prod_expire));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = product_dto.Prod_expire.Month.ToString();
                        if (product_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + product_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = product_dto.Prod_expire.Day.ToString();
                        if (product_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + product_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = product_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));



                        Console.WriteLine(product_dto.Prod_expire);
                        Console.WriteLine(product_dto.Category_id + "카페고리id");
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() == false)
                        {
                            //없는 제품을 추가하려고 하는 경우(access)
                            isDuplicated = false;
                        }
                        else
                        {
                            //이미 입력된 제품을 추가하려고 하는 경우(deny)
                            isDuplicated = true;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return isDuplicated;

        }//IdCheck


        public bool IsProductDuplicateCheck(ProductModel product_dto, string category_name) //오버로딩
        {
            log.Info("IsProductDuplicateCheck(ProductModel, string) invoked.");
            bool isDuplicated = false;
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
                        cmd.CommandText = "SELECT prod_code " +
                                          "FROM PRODUCT " +
                                          "WHERE prod_code = :prod_code " +
                                          "AND prod_name = :prod_name " +
                                          "AND category_id = (SELECT category_id FROM category WHERE category_name = :category_name) " +
                                          "AND prod_price = :prod_price " +
                                          "AND prod_expire = TO_DATE(:expire, 'YYYYMMDD') ";
                        cmd.Parameters.Add(new OracleParameter("prod_code", product_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("prod_name", product_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));
                        cmd.Parameters.Add(new OracleParameter("prod_price", product_dto.Prod_price));
                        //cmd.Parameters.Add(new OracleParameter("prod_expire", product_dto.Prod_expire));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = product_dto.Prod_expire.Month.ToString();
                        if (product_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + product_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = product_dto.Prod_expire.Day.ToString();
                        if (product_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + product_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = product_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));



                        Console.WriteLine(product_dto.Prod_expire);
                        Console.WriteLine(product_dto.Category_id + "카페고리id");
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() == false)
                        {
                            //없는 제품을 추가하려고 하는 경우(access)
                            isDuplicated = false;
                        }
                        else
                        {
                            //이미 입력된 제품을 추가하려고 하는 경우(deny)
                            isDuplicated = true;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return isDuplicated;

        }//IdCheck



        public List<ProductShowModel> GetProductsByDept(DeptModel dept_dto)
        {
            log.Info("GetProductsByDept(DeptModel) invoked.");
            List<ProductShowModel> list = new List<ProductShowModel>();
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, I.imp_dept_count, P.prod_expire, P.prod_id, I.imp_dept_id " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "INNER JOIN DEPT D " +
                                          "ON I.dept_id = D.dept_id " +
                                          "WHERE D.dept_status != '폐지' " +
                                          "AND D.dept_name = :dept_name " +
                                          "AND I.imp_dept_count > 0 " +
                                          "ORDER BY P.prod_code, P.prod_expire ";

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? imp_dept_count = reader.GetInt32(4);
                            DateTime prod_expire = reader.GetDateTime(5);
                            int? prod_id = reader.GetInt32(6);
                            int? imp_dept_id = reader.GetInt32(7);


                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Imp_dept_count = imp_dept_count,
                                Prod_expire = prod_expire,
                                Prod_id = prod_id,
                                Imp_dept_id = imp_dept_id,
                                Prod_remainexpire = (prod_expire.Date - DateTime.Now.Date).Days
                            };

                            list.Add(dto);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;
        }//GetProductsByDept



        public List<CategoryModel> GetCategoryModels()
        {
            log.Info("GetCategoryModels() invoked.");
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

                        cmd.CommandText = "SELECT CATEGORY_NAME FROM CATEGORY ORDER BY category_id";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string category_name = reader.GetString(0);
                            CategoryModel category = new CategoryModel()
                            {
                                Category_name = category_name
                            };
                            list.Add(category);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }// GetCategoryModels(string sql)

        public void AddProduct(ProductModel prod_dto, CategoryModel category_dto)
        {
            log.Info("AddProduct(ProductModel, CategoryModel) invoked.");

            //CategoryDao category_dao = new CategoryDao();
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

                        cmd.CommandText = "INSERT INTO PRODUCT(PROD_CODE, PROD_NAME, PROD_PRICE, PROD_TOTAL, PROD_EXPIRE, CATEGORY_ID) " +
                                          "VALUES(:code, :name, :price, :total, TO_DATE(:expire, 'YYYYMMDD'), (SELECT category_id FROM CATEGORY WHERE category_name = :category_name) )";


                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));
                        ////////////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("category_name", category_dto.Category_name));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//AddProduct

        public void AddProductForExcel(ProductShowModel prod_dto, String categoryName)
        {
            log.Info("AddProductForExcel(ProductShowModel, string) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT(PROD_CODE, PROD_NAME, PROD_PRICE, PROD_TOTAL, PROD_EXPIRE, CATEGORY_ID) " +
                                          "VALUES(:code, :name, :price, :total, TO_DATE(:expire, 'YYYYMMDD'), (SELECT category_id FROM CATEGORY WHERE category_name = :category_name) )";


                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));


                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expireText = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expireText);
                        ////////////////////////////////////////////////////////////////////////////


                        /*String expireText = prod_dto.Prod_expire.Year.ToString()+
                            prod_dto.Prod_expire.Month.ToString()+ prod_dto.Prod_expire.Day.ToString();*/

                        cmd.Parameters.Add(new OracleParameter("expire", expireText));
                        cmd.Parameters.Add(new OracleParameter("category_name", categoryName));

                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }

        public void AddProduct(ProductModel prod_dto, string category_name) //오버로딩
        {
            log.Info("AddProduct(ProductModel, string) invoked.");

            //CategoryDao category_dao = new CategoryDao();
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

                        cmd.CommandText = "INSERT INTO PRODUCT(PROD_CODE, PROD_NAME, PROD_PRICE, PROD_TOTAL, PROD_EXPIRE, CATEGORY_ID) " +
                                          "VALUES(:code, :name, :price, :total, TO_DATE(:expire, 'YYYYMMDD'), (SELECT category_id FROM CATEGORY WHERE category_name = :category_name) )";


                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));
                        ////////////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//AddProduct



        public void StoredProduct(ProductModel prod_dto, NurseModel nurse_dto)
        {
            log.Info("StoredProduct(ProductModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, PROD_SEQ.CURRVAL, :nurse_no, :dept_id1, :in_from, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :in_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("in_from", "발주처"));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("in_type", "신규"));

                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }// StoredProduct()

        public void StoredProductForExcel(ProductShowModel prod_dto, NurseModel nurse_dto)
        {
            log.Info("StoredProductForExcel(ProductShowModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, PROD_SEQ.CURRVAL, :nurse_no, :dept_id1, :in_from, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :in_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", "발주처"));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("in_type", "신규"));

                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }// StoredProduct()


        public List<ProductInOutModel> GetProductIn()
        {
            log.Info("GetProductIn() invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductIn

        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto) // 오버로딩
        {
            log.Info("GetProductIn(DeptModel) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.prod_in_to = :name " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        Console.WriteLine("ProductDao - GetProductIn() 부서명 파라미터: " + dept_dto.Dept_name);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductIn


        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto, DateTime? start_date, DateTime? end_date) // 오버로딩
        {
            log.Info("GetProductIn(DeptModel, DateTime?, DateTime?) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.prod_in_to = :name " +
                                          "AND I.prod_in_date BETWEEN :start_date AND :end_date + 1 " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name ";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));

                        Console.WriteLine("ProductDao - GetProductIn() 부서명 파라미터: " + dept_dto.Dept_name);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductIn

        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto, string search_type, string search_text) // 오버로딩
        {
            log.Info("GetProductIn(DeptModel, string, string) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND I.prod_in_to = :dept_name ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND I.prod_in_to = :dept_name ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND I.prod_in_to = :dept_name ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name ";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductIn



        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto, string search_type, string search_text, DateTime? start_date, DateTime? end_date) // 오버로딩
        {
            log.Info("GetProductIn(DeptModel, string, string, DateTime?, DateTime?) invoked.");

            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND I.prod_in_to = :dept_name AND I.prod_in_date BETWEEN :start_date AND :end_date +1 ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND I.prod_in_to = :dept_name AND I.prod_in_date BETWEEN :start_date AND :end_date  +1 ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND I.prod_in_to = :dept_name AND I.prod_in_date BETWEEN :start_date AND :end_date +1 ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductIn


        public string GetProductIn_MaxDate(DeptModel dept_dto)
        {

            log.Info("GetProductIn_MaxDate(DeptModel) invoked.");
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
                        cmd.CommandText = "SELECT MAX(I.prod_in_date) " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.prod_in_to = :name " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name ";


                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));

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
        }//GetProductIn_MaxDate

        public string GetProductIn_MaxDate(NurseModel nurse_dto) //오버로딩
        {

            log.Info("GetProductIn_MaxDate(NurseModel) invoked.");
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
                        cmd.CommandText = "SELECT MAX(I.prod_in_date) " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.nurse_no = :no " +
                                          "AND I.PROD_IN_TYPE = '신규' " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name ";


                        cmd.Parameters.Add(new OracleParameter("name", nurse_dto.Nurse_no));

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
        }//GetProductIn_MaxDate

        public string GetProductIn_MinDate(DeptModel dept_dto)
        {
            log.Info("GetProductIn_MinDate(DeptModel) invoked.");
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
                        cmd.CommandText = "SELECT MIN(I.prod_in_date) " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.prod_in_to = :name " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name ";


                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));

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
        }//GetProductIn_MinDate

        public string GetProductIn_MinDate(NurseModel nurse_dto) // 오버로딩
        {
            log.Info("GetProductIn_MinDate(DeptModel) invoked.");
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
                        cmd.CommandText = "SELECT MIN(I.prod_in_date) " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.nurse_no = :no " +
                                          "AND I.PROD_IN_TYPE = '신규' " +
                                          "ORDER BY I.prod_in_date desc, P.prod_name ";


                        cmd.Parameters.Add(new OracleParameter("no", nurse_dto.Nurse_no));
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
        }//GetProductIn_MinDate

        public List<ProductInOutModel> GetProductOut()
        {
            log.Info("GetProductOut() invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to, P.prod_expire " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);
                            DateTime prod_expire = reader.GetDateTime(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to,
                                Prod_expire = prod_expire
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductOut

        public List<ProductInOutModel> GetProductOut(DeptModel dept_dto) // 오버로딩
        {
            log.Info("GetProductOut(DeptModel) invoked.");

            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to, P.prod_expire " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_from = :name " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        Console.WriteLine("ProductDao - GetProductOut() 부서명 파라미터: " + dept_dto.Dept_name);

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);
                            DateTime prod_expire = reader.GetDateTime(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to,
                                Prod_expire = prod_expire
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductOut

        public List<ProductInOutModel> GetProductOut(DeptModel dept_dto, DateTime? start_date, DateTime? end_date) // 오버로딩
        {
            log.Info("GetProductOut(DeptModel, DateTime?, DateTime?) invoked.");

            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to, P.prod_expire " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_from = :name " +
                                          "AND O.prod_out_date BETWEEN :start_date AND :end_date + 1 " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));
                        Console.WriteLine("ProductDao - GetProductOut() 부서명 파라미터: " + dept_dto.Dept_name);

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);
                            DateTime prod_expire = reader.GetDateTime(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to,
                                Prod_expire = prod_expire
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductOut

        public List<ProductInOutModel> GetProductOut(DeptModel dept_dto, string search_type, string search_text)
        {
            log.Info("GetProductOut(DeptModel, string, string) invoked.");

            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to, P.prod_expire " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND O.prod_out_from = :dept_name ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND O.prod_out_from = :dept_name ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND O.prod_out_from = :dept_name ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);
                            DateTime prod_expire = reader.GetDateTime(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to,
                                Prod_expire = prod_expire
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductOut


        public List<ProductInOutModel> GetProductOut(DeptModel dept_dto, string search_type, string search_text, DateTime? start_date, DateTime? end_date)
        {
            log.Info("GetProductOut(DeptModel, string, string, DateTime?, DateTime?) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to, P.prod_expire " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND O.prod_out_from = :dept_name AND O.prod_out_date BETWEEN :start_date AND :end_date + 1 ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND O.prod_out_from = :dept_name AND O.prod_out_date BETWEEN :start_date AND :end_date + 1 ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND O.prod_out_from = :dept_name AND O.prod_out_date BETWEEN :start_date AND :end_date + 1 ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);
                            DateTime prod_expire = reader.GetDateTime(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to,
                                Prod_expire = prod_expire
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//GetProductOut


        public string GetProductOut_MaxDate(DeptModel dept_dto)
        {
            log.Info("GetProductOut_MaxDate(DeptModel) invoked.");
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
                        cmd.CommandText = "SELECT MAX(O.prod_out_date) " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_to = :name " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name ";


                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));

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
        }//GetProductOut_MaxDate


        public string GetProductOut_MinDate(DeptModel dept_dto)
        {
            log.Info("GetProductOut_MinDate(DeptModel) invoked.");
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
                        cmd.CommandText = "SELECT MIN(O.prod_out_date) " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_to = :name " +
                                          "ORDER BY O.prod_out_date desc, P.prod_name ";


                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));

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
        }//GetProductOut_MaxDate



        public void AddImpDept(ProductModel prod_dto, NurseModel nurse_dto)
        {
            log.Info("AddImpDept(ProductModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO IMP_DEPT(imp_dept_count, dept_id, prod_id) " +
                                          "VALUES(:count, :dept_id, PROD_SEQ.CURRVAL) ";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));



                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//AddImpDept

        public void AddImpDeptForExcel(ProductShowModel prod_dto, NurseModel nurse_dto)
        {
            log.Info("AddImpDeptForExcel(ProductShowModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO IMP_DEPT(imp_dept_count, dept_id, prod_id) " +
                                          "VALUES(:count, :dept_id, PROD_SEQ.CURRVAL) ";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));



                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//AddImpDept

        public ObservableCollection<ProductInOutModel> GetProductInByNurse(NurseModel nurse_dto)
        {
            log.Info("GetProductInByNurse(NurseModel) invoked.");
            ObservableCollection<ProductInOutModel> list = new ObservableCollection<ProductInOutModel>();
            Console.WriteLine("GetProductInByNurse 실행");
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "WHERE I.nurse_no = :no " +
                                          "AND I.PROD_IN_TYPE = '신규' " +
                                          "ORDER BY I.prod_in_date DESC";

                        cmd.Parameters.Add(new OracleParameter(":no", nurse_dto.Nurse_no));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;
        }//GetProductInByNurse

        public ObservableCollection<ProductInOutModel> GetProductInByNurse(NurseModel nurse_dto, DateTime? start, DateTime? end)
        {
            log.Info("GetProductInByNurse(NurseModel) invoked.");
            ObservableCollection<ProductInOutModel> list = new ObservableCollection<ProductInOutModel>();
            Console.WriteLine("GetProductInByNurse 실행");
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "WHERE I.nurse_no = :no " +
                                          "AND I.PROD_IN_TYPE = '신규' " +
                                          "AND I.prod_in_date BETWEEN :start_date AND :end_date + 1 " +
                                          "ORDER BY I.prod_in_date DESC";

                        cmd.Parameters.Add(new OracleParameter(":no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("start_date", start));
                        cmd.Parameters.Add(new OracleParameter("end_date", end));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;
        }//GetProductInByNurse

        public List<ProductShowModel> SearchProducts(DeptModel dept_dto, string search_type, string search_text)
        {
            log.Info("SearchProducts(DeptModel, string, string) invoked.");
            List<ProductShowModel> list = new List<ProductShowModel>();
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, I.imp_dept_count, P.prod_expire, P.prod_id, I.imp_dept_id " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "INNER JOIN DEPT D " +
                                          "ON I.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND D.dept_name = :dept_name AND I.imp_dept_count != 0 ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND D.dept_name = :dept_name AND I.imp_dept_count != 0 ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND D.dept_name = :dept_name AND I.imp_dept_count != 0 ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY P.prod_expire, P.prod_name";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));
                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? imp_dept_count = reader.GetInt32(4);
                            DateTime prod_expire = reader.GetDateTime(5);
                            int? prod_id = reader.GetInt32(6);
                            int? imp_dept_id = reader.GetInt32(7);


                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Imp_dept_count = imp_dept_count,
                                Prod_expire = prod_expire,
                                Prod_id = prod_id,
                                Imp_dept_id = imp_dept_id,
                                Prod_remainexpire = (prod_expire.Date - DateTime.Now.Date).Days
                            };

                            list.Add(dto);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return list;

        }//SearchProducts()


        public void ChangeProductInfo(ProductShowModel prod_dto)
        {
            log.Info("ChangeProductInfo(ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_code = :code, " +
                                          "prod_name = :name, " +
                                          "category_id = (SELECT category_id FROM CATEGORY WHERE category_name = :category_name), " +
                                          "prod_expire = TO_DATE(:expire, 'YYYYMMDD'), " +
                                          "prod_price = :pirce, " +
                                          "prod_total = :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", prod_dto.Category_name));


                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));
                        ////////////////////////////////////////////////////////////////////////////


                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Imp_dept_count));
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }//ChangeProductInfo()

        public void ChangeProductInfo_IMP_DEPT(ProductShowModel prod_dto)
        {
            log.Info("ChangeProductInfo_IMP_DEPT(ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", prod_dto.Imp_dept_count));
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }//ChangeProductInfo_IMP_DEPT()

        public void OutProduct(int? InputOutCount, ProductShowModel prod_dto, NurseModel nurse_dto, string SelectedOutType, DeptModel dept_dto)
        {
            log.Info("OutProduct(int?, ProductShowModel, NurseModel, string, DeptModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_OUT(PROD_OUT_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_OUT_FROM, PROD_OUT_TO, PROD_OUT_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id1, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :out_to, :out_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));

                        ///////////////////////////////////////////////////////////////////////
                        if (SelectedOutType.Equals("이관"))
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", dept_dto.Dept_name)); //출고된 곳은 콤보박스에서 선택한 부서로 출고
                            OutProduct_FromTo(InputOutCount, prod_dto, nurse_dto, SelectedOutType, dept_dto); // 이관 시에 이관받은 부서에서 입고도 함께 진행
                            OutProduct_FromTo_IMP_DEPT(InputOutCount, prod_dto, dept_dto); // 이관되어서 해당부서는 입고받았기 때문에 imp_dept 테이블에도 추가
                        }
                        else if (SelectedOutType.Equals("사용")) //출고된 곳이 '사용'이라면
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", GetNurseDeptName(nurse_dto)));
                        }
                        else // 폐기 일때
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", SelectedOutType));
                        }
                        ///////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("out_type", SelectedOutType));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }// OutProduct()

        public void OutProduct(ProductShowModel prod_dto, NurseModel nurse_dto)//오버로딩
        {
            log.Info("OutProduct(ProductShowModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_OUT(PROD_OUT_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_OUT_FROM, PROD_OUT_TO, PROD_OUT_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id1, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :out_to, :out_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));

                        ///////////////////////////////////////////////////////////////////////
                        if (prod_dto.SelectedOutType.Equals("이관"))
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", prod_dto.SelectedOutDept.Dept_name)); //출고된 곳은 콤보박스에서 선택한 부서로 출고
                            OutProduct_FromTo(prod_dto, nurse_dto); // 이관 시에 이관받은 부서에서 입고도 함께 진행
                            OutProduct_FromTo_IMP_DEPT(prod_dto); // 이관되어서 해당부서는 입고받았기 때문에 imp_dept 테이블에도 추가
                        }
                        else if (prod_dto.SelectedOutType.Equals("사용")) //출고 유형이 '사용'이라면
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", GetNurseDeptName(nurse_dto)));
                        }
                        else // 폐기 일때
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", prod_dto.SelectedOutType));
                        }
                        ///////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("out_type", prod_dto.SelectedOutType));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }// OutProduct()
        public string GetNurseDeptName(NurseModel nurse_dto)
        {
            log.Info("GetNurseDeptName(NurseModel) invoked.");
            string dept_name = "";
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
                        cmd.CommandText = "SELECT dept_name FROM DEPT WHERE dept_id = :dept_id ";

                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));

                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            dept_name = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

            return dept_name;

        }//GetNurseDeptName()





        public void OutProduct_FromTo(int? InputOutCount, ProductShowModel prod_dto, NurseModel nurse_dto, string SelectedOutType, DeptModel dept_dto)
        {
            log.Info("OutProduct_FromTo(int?, ProductShowModel, NurseModel, string, DeptModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id, :in_from, :in_to, :in_type) ";

                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", GetNurseDeptName(nurse_dto))); // 출고한 사원 소속 부서
                        cmd.Parameters.Add(new OracleParameter("in_to", dept_dto.Dept_name)); // 입고받은 부서명
                        cmd.Parameters.Add(new OracleParameter("in_type", SelectedOutType));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutPorduct_FromTo()

        public void OutProduct_FromTo(ProductShowModel prod_dto, NurseModel nurse_dto) //오버로딩
        {
            log.Info("OutProduct_FromTo(ProductShowModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id, :in_from, :in_to, :in_type) ";

                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", GetNurseDeptName(nurse_dto))); // 출고한 사원 소속 부서
                        cmd.Parameters.Add(new OracleParameter("in_to", prod_dto.SelectedOutDept.Dept_name)); // 입고받은 부서명
                        cmd.Parameters.Add(new OracleParameter("in_type", prod_dto.SelectedOutType));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutPorduct_FromTo()

        public void OutProduct_FromTo_IMP_DEPT(int? InputOutCount, ProductShowModel prod_dto, DeptModel dept_dto)
        {
            log.Info("OutProduct_FromTo_IMP_DEPT(int?, ProductShowModel, DeptModel) invoked.");
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

                        /*                        cmd.CommandText = "INSERT INTO IMP_DEPT(IMP_DEPT_COUNT, DEPT_ID, PROD_ID) " +
                                                                  "VALUES(:count, :dept_id, :pord_id) ";*/

                        cmd.CommandText = "MERGE INTO IMP_DEPT A " +
                                          "USING dual " +
                                          "ON(:prod_id IN(SELECT prod_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id) " +
                                          "   AND :dept_id IN(SELECT dept_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id)) " +
                                          "WHEN MATCHED THEN " +
                                          "UPDATE SET A.imp_dept_count = A.imp_dept_count + :count " +
                                          "WHERE A.dept_id = :dept_id AND A.prod_id = :prod_id " +
                                          "WHEN NOT MATCHED THEN " +
                                          "INSERT(A.imp_dept_count, A.dept_id, A.prod_id) " +
                                          "VALUES(:count, :dept_id, :prod_id)";

                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));

                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutProduct_FromTo_IMP_DEPT

        public void OutProduct_FromTo_IMP_DEPT(ProductShowModel prod_dto)//오버로딩
        {
            log.Info("OutProduct_FromTo_IMP_DEPT(ProductShowModel) invoked.");
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

                        /*                        cmd.CommandText = "INSERT INTO IMP_DEPT(IMP_DEPT_COUNT, DEPT_ID, PROD_ID) " +
                                                                  "VALUES(:count, :dept_id, :pord_id) ";*/

                        cmd.CommandText = "MERGE INTO IMP_DEPT A " +
                                          "USING dual " +
                                          "ON(:prod_id IN(SELECT prod_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id) " +
                                          "   AND :dept_id IN(SELECT dept_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id)) " +
                                          "WHEN MATCHED THEN " +
                                          "UPDATE SET A.imp_dept_count = A.imp_dept_count + :count " +
                                          "WHERE A.dept_id = :dept_id AND A.prod_id = :prod_id " +
                                          "WHEN NOT MATCHED THEN " +
                                          "INSERT(A.imp_dept_count, A.dept_id, A.prod_id) " +
                                          "VALUES(:count, :dept_id, :prod_id)";

                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));

                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//OutProduct_FromTo_IMP_DEPT

        public void ChangeProductInfo_IMP_DEPT_ForOut(int? InputOutCount, ProductShowModel prod_dto)
        {
            log.Info("ChangeProductInfo_IMP_DEPT_ForOut(int?, ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = imp_dept_count - :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//ChangeProductInfo_IMP_DEPT_ForOut

        public void ChangeProductInfo_IMP_DEPT_ForOut(ProductShowModel prod_dto)//오버로딩
        {
            log.Info("ChangeProductInfo_IMP_DEPT_ForOut(ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = imp_dept_count - :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//ChangeProductInfo_IMP_DEPT_ForOut
        public void ChangeProductInfo_ForOut(int? InputOutCount, ProductShowModel prod_dto)
        {
            log.Info("ChangeProductInfo_ForOut(int?, ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_total = prod_total - :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("total", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//ChangeProductInfo_ForOut

        public void ChangeProductInfo_ForOut(ProductShowModel prod_dto) //오버로딩
        {
            log.Info("ChangeProductInfo_ForOut(ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_total = prod_total - :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//ChangeProductInfo_ForOut 

        //부서별 카테고리별//제품 총수량 그래프 
        public List<ImpDeptModel> Dept_Category_Mount(DeptModel SelectedDepts)
        {
            log.Info("Dept_Category_Mount(DeptModel) invoked.");
            List<ImpDeptModel> list = new List<ImpDeptModel>();
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
                        cmd.CommandText = "SELECT D.dept_name, C.category_name, SUM(I.imp_dept_count) " +
                            "FROM IMP_DEPT I " +
                            "INNER JOIN PRODUCT P " +
                            "ON I.prod_id = P.prod_id " +
                            "INNER JOIN CATEGORY C " +
                            "ON P.category_id = C.category_id " +
                            "INNER JOIN DEPT D " +
                            "ON I.dept_id = D.dept_id " +
                            "WHERE D.dept_name = :dept_name " +
                            "AND I.imp_dept_count > 0 " +
                            "GROUP BY C.category_name, D.dept_name";


                        cmd.Parameters.Add(new OracleParameter("dept_name", SelectedDepts.Dept_name)); //category_name
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Category_name = reader.GetString(1);
                            int? SUM_dept = reader.GetInt32(2);
                            ImpDeptModel dto = new ImpDeptModel()
                            {
                                Category_name = Category_name,
                                Imp_dept_count = SUM_dept
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
            return list;
        }///Dept_Category_Mount

        public List<ProductShowModel> Get_Dept_Category_Number_RemainExpire(DeptModel SelectedDept, CategoryModel SelectedCategory, int SelectedNumber)
        {
            log.Info("Get_Dept_Category_Number_RemainExpire(DeptModel, CategoryModel, int) invoked.");
            List<ProductShowModel> list = new List<ProductShowModel>();
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
                        cmd.CommandText = "SELECT A.* " +
                            "FROM (SELECT P.prod_code, TO_NUMBER(TO_DATE(TO_CHAR(P.prod_expire, 'YYYYMMDD')) - TO_DATE(TO_CHAR(CURRENT_DATE, 'YYYYMMDD'))) " +
                            "FROM product P " +
                            "INNER JOIN category C " +
                            "ON P.category_id = C.category_id " +
                            "INNER JOIN imp_dept I " +
                            "ON P.prod_id = I.prod_id " +
                            "INNER JOIN dept D " +
                            "ON I.dept_id = D.dept_id " +
                            "WHERE d.dept_name= :dept_name and C.category_name = :category_name and P.prod_total>0 " +
                            "GROUP BY P.prod_code, P.prod_expire " +
                            "ORDER BY 2 desc) A " +
                            "WHERE ROWNUM <= :SelectedNumber";


                        cmd.Parameters.Add(new OracleParameter("dept_name", SelectedDept.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", SelectedCategory.Category_name)); //category_name
                        cmd.Parameters.Add(new OracleParameter("SelectedNumber", SelectedNumber));

                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Prod_code = reader.GetString(0);
                            string Prod_remainexpire = reader.GetString(1);
                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = Prod_code,
                                Prod_remainexpire = Convert.ToInt32(Prod_remainexpire)
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
            return list;
        } //Get_Dept_Category_RemainExpire

        public void InProduct(ProductShowModel prod_dto, NurseModel nurse_dto)
        {
            log.Info("InProduct(ProductShowModel, NurseModel) invoked.");
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id, :in_from, :in_to, :in_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputInCount)); // PROD_IN_COUNT
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));     // PROD_ID
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));  // NURSE_NO
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));   // DEPT_ID
                        cmd.Parameters.Add(new OracleParameter("in_from", "발주처"));            // PROD_IN_FROM
                        cmd.Parameters.Add(new OracleParameter("in_to", GetNurseDeptName(nurse_dto))); // PROD_IN_TO * 입고된 부서는 팝업박스에서 입고한 사용자의 부서와 같다.(자기 부서에만 입고할 수 있음)
                        cmd.Parameters.Add(new OracleParameter("in_type", prod_dto.SelectedInType)); // PROD_IN_TYPE

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch

        }//InProduct()

        public void ChangeProductInfo_IMP_DEPT_ForIn(ProductShowModel prod_dto)
        {
            log.Info("ChangeProductInfo_IMP_DEPT_ForIn(ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = imp_dept_count + :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", prod_dto.InputInCount)); //사용자가 입력한 추가 수량
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }//ChangeProductInfo_IMP_DEPT_ForIn

        public void ChangeProductInfo_ForIn(ProductShowModel prod_dto)
        {
            log.Info("ChangeProductInfo_ForIn(ProductShowModel) invoked.");
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_total = prod_total + :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.InputInCount)); //사용자가 입력한 추가 수량
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
        }//ChangeProductInfo_ForIn

        // 사용/이관/폐기 리스트로 얻기
        public List<ProductInOutModel> GetProdOutType()
        {
            log.Info("GetProdOutType() invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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
                        cmd.CommandText = "SELECT DISTINCT PROD_OUT_TYPE " +
                                            "FROM PRODUCT_OUT";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ProductInOutModel dto = new ProductInOutModel()
                            {

                                Prod_out_type = reader.GetString(0)
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

        }//GetProdOutType()

        //도넛그래프 dao
        public List<ProductInOutModel> GetDiscardTotalCount(DeptModel dept_dto, ProductInOutModel outtype_dto)
        {
            log.Info("GetDiscardTotalCount(DeptModel, ProductInOutModel) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();
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
                        cmd.CommandText = "SELECT P.prod_name , O.prod_out_count, O.prod_out_count * P.prod_price " +
                                          "FROM product_out O " +
                                          "INNER JOIN product P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN dept D " +
                                          "ON O.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_type = :prod_out_type " +
                                          "AND D.dept_name = :dept_name " +
                                          "AND ROWNUM<=10 " +
                                          "ORDER BY O.prod_out_count * P.prod_price DESC";


                        cmd.Parameters.Add(new OracleParameter("prod_out_type", outtype_dto.Prod_out_type));
                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));


                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_name = reader.GetString(0),
                                Prod_out_count = reader.GetInt32(1),
                                Prod_price = reader.GetInt32(2)
                            };
                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
            return list;
        }//GetDiscardTotalCount

        public List<ProductInOutModel> incomingCases_Info(DateTime? startDate, DateTime? endDate)
        {
            log.Info("incomingCases_Info(DateTime?, DateTime?) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();
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
                        cmd.CommandText = "SELECT prod_in_to, " +
                                          "COUNT(CASE WHEN prod_in_type = '이관' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_in_type = '신규' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_in_type = '추가' THEN 1 END) " +
                                          "FROM product_in " +
                                          "WHERE prod_in_date > :startDate " +
                                          "AND prod_in_date < :endDate + 1 " +
                                          "GROUP BY prod_in_to";

                        cmd.Parameters.Add(new OracleParameter("startDate", startDate));
                        cmd.Parameters.Add(new OracleParameter("endDate", endDate));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dept_name = reader.GetString(0);
                            int? prod_transferIn_cases = reader.GetInt32(1);
                            int? prod_order_cases = reader.GetInt32(2);
                            int? prod_add_cases = reader.GetInt32(3);
                            Console.WriteLine("prod_add_cases : " + prod_add_cases);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Dept_name = dept_name,
                                prod_transferIn_cases = prod_transferIn_cases,
                                prod_order_cases = prod_order_cases,
                                prod_add_cases = prod_add_cases,
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
            return list;
        }//orderCases_Info

        public List<ProductInOutModel> ReleaseCases_Info(DateTime? startDate, DateTime? endDate) // 부서별 출고 횟수 정보를 담은 리스트
        {
            log.Info("ReleaseCases_Info(DateTime?, DateTime?) invoked.");
            List<ProductInOutModel> list = new List<ProductInOutModel>();
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
                        cmd.CommandText = "SELECT prod_out_from, " +
                                          "COUNT(CASE WHEN prod_out_type = '사용' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_out_type = '이관' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_out_type = '폐기' THEN 1 END) " +
                                          "FROM product_out " +
                                          "WHERE prod_out_date > :startDate " +
                                          "AND prod_out_date < :endDate + 1 " +
                                          "GROUP BY prod_out_from";

                        cmd.Parameters.Add(new OracleParameter("startDate", startDate));
                        cmd.Parameters.Add(new OracleParameter("endDate", endDate));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dept_name = reader.GetString(0);
                            int? prod_use_cases = reader.GetInt32(1);
                            int? prod_transferOut_cases = reader.GetInt32(2);
                            int? prod_discard_cases = reader.GetInt32(3);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Dept_name = dept_name,
                                prod_use_cases = prod_use_cases,
                                prod_transferOut_cases = prod_transferOut_cases,
                                prod_discard_cases = prod_discard_cases
                            };
                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                log.Error(e.Message);
            }//catch
            return list;
        }//ReleaseCases_Info
    }//class



}//namespace
