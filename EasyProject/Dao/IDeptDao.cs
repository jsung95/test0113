using System;
using System.Collections.Generic;
using EasyProject.Model;

namespace EasyProject.Dao
{
    public interface IDeptDao
    {
        List<DeptModel> GetDepts();
        DeptModel GetDeptName(int dept_id);
        string GetDeptName_Return_String(int dept_id);
    }//interface

}//namespace
