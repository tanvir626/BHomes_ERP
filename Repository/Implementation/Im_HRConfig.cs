
using Bhomes_ERP.Models.VM_Model;
using Bhomes_ERP.Repository.Interface;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json.Linq;

namespace Bhomes_ERP.Repository.Implementation
{
    public class Im_HRConfig(IDapper con):IHRConfig
    {

        #region Tools
        private char status(string status)
        {
            if (status == "true")
                return 'Y';
            return 'N';
        }
        #endregion

        #region Department
        public List<VM_Department> Get_HR_Department()
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"select HR_DeptID, HR_DeptCode,HR_DeptName,Status from HR_Department";

                    var data = connection.Query<VM_Department>(sql).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<VM_Department>();
            }
        }
        public bool Save_to_HR_Department(VM_Department model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    INSERT INTO [CoreDB].[dbo].[HR_Department]
                                    (
                                        HR_DeptCode,
                                        HR_DeptName,
                                        Status,
                                        CreatedBy,
                                        EditedBy,
                                        CreateDate
                                    )
                                    SELECT 
                                        @HR_DeptCode,
                                        @HR_DeptName,
                                        @Status,
                                        @CreatedBy,
                                        @EditedBy,
                                        @CreateDate
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM [CoreDB].[dbo].[HR_Department]
                                        WHERE HR_DeptName LIKE '%' + @HR_DeptName + '%'
                                    );";


                    var rowsAffected = connection.Execute(sql, new
                    {
                        HR_DeptCode = Get_HR_Dept_CODE(),
                        model.HR_DeptName,
                        Status = "true",
                        CreatedBy = con.GetLoggedUserName(),
                        EditedBy = "Fresh",
                        CreateDate = DateTime.Now
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool Update_to_HR_Department(VM_Department model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_Department]
                                    SET             
                                        HR_DeptName = @HR_DeptName,
                                        Status = @Status,
                                        EditedBy = @EditedBy,
                                        EditDate = @EditDate
                                    where HR_DeptCode=@DeptCode;
                                ";
                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.HR_DeptName,
                        model.Status,
                        Editedby = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        DeptCode = model.HR_DeptCode
                    });
                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private int Get_HR_Dept_CODE()
        {
            return Get_Last_DeptCode() + 1;
        }

        private int Get_Last_DeptCode()
        {
            using (var connection = new SqlConnection(con.Dappercon()))
            {
                string sql = @"SELECT ISNULL(MAX(HR_DeptCode), 0) FROM HR_Department";
                int Code = connection.QuerySingle<int>(sql);

                if (Code <= 0)
                    Code = 100;

                return Code;

            }
        }
        #endregion

        #region Sub-Department
        public List<VM_SubDepartment> Get_HR_SubDepartment()
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    Select s.Id,s.HR_DeptID,d.HR_DeptName,s.HR_SubDeptName,s.Status 
                                    from HR_SubDepartment s inner join HR_Department d 
                                    on d.HR_DeptID=s.HR_DeptID";

                    var data = connection.Query<VM_SubDepartment>(sql).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<VM_SubDepartment>();
            }
        }
  
        public bool Save_to_HR_SubDepartment(VM_SubDepartment model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    INSERT INTO [CoreDB].[dbo].[HR_SubDepartment]
                                    (
                                        HR_DeptID,
                                        HR_SubDeptName,
                                        Status,
                                        Createdby,
                                        Editby,
                                        CreateDate
                                    )
                                    SELECT 
                                        @HR_DeptID,
                                        @HR_SubDeptName,
                                        @Status,
                                        @Createdby,
                                        @Editby,
                                        @CreateDate
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM [CoreDB].[dbo].[HR_SubDepartment]
                                        WHERE HR_SubDeptName LIKE '%' + @HR_SubDeptName + '%'
                                    );";


                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.HR_DeptID,
                        model.HR_SubDeptName,
                        Status = "Y",
                        CreatedBy = con.GetLoggedUserName(),
                        EditBy = "Fresh",
                        CreateDate = DateTime.Now                     
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update_to_HR_Sub_Department(VM_SubDepartment model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_SubDepartment]
                                    SET  
                                        Status = @Status,
                                        Editby = @Editby,
                                        EditDate = @EditDate
                                    where Id=@Id;
                                ";
                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        Status=status(model.Status),
                        Editby = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        model.Id
                    });
                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #region Shift
        public bool Save_to_HR_Shift(VM_Shift model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    INSERT INTO [CoreDB].[dbo].[HR_Shift]
                                    (
                                        ShiftName,
                                        Weekend1,
                                        StartTime,
                                        EndTime,
                                        LateAllowed,
                                        Status,
                                        CreatedBy,
                                        CreateDate,
                                        EditedBy                           

                                    )
                                    SELECT 
                                        @ShiftName,
                                        @Weekend1,
                                        @StartTime,
                                        @EndTime,
                                        @LateAllowed,
                                        @Status,
                                        @CreatedBy,
                                        @CreateDate,
                                        @EditedBy
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM [CoreDB].[dbo].[HR_Shift]
                                        WHERE  StartTime =  @StartTime and EndTime = @EndTime
                                    );";


                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.ShiftName,
                        model.Weekend1,
                        model.StartTime,
                        model.EndTime,
                        model.LateAllowed,
                        Status="Y",
                        CreatedBy = con.GetLoggedUserName(),
                        CreateDate = DateTime.Now,
                        EditedBy = "Fresh",
                        
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update_to_HR_Shift(VM_Shift model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_Shift]
                                    SET
                                        ShiftName=@ShiftName,
                                        Weekend1=@Weekend1,
                                        StartTime=@StartTime,
                                        EndTime=@EndTime,
                                        LateAllowed=@LateAllowed,
                                        Status = @Status,
                                        EditedBy = @EditedBy,
                                        EditDate = @EditDate
                                        where ShiftID=@ShiftID;
                                ";
                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.ShiftName,
                        model.Weekend1,
                        model.StartTime,
                        model.EndTime,
                        model.LateAllowed,
                        Status= status(model.Status),
                        EditedBy = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        model.ShiftId
                    });
                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region Designation
        public bool Save_to_HR_Designation(VM_Designation model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    INSERT INTO [CoreDB].[dbo].[HR_Designation]
                                    (
                                        DesignationTitle,
                                        Status,                                        
                                        CreatedBy,
                                        CreateDate,
                                        EditedBy
                                    )
                                    SELECT 
                                       @DesignationTitle,
                                       @Status,                                        
                                        @CreatedBy,
                                        @CreateDate,
                                        @EditedBy
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM [CoreDB].[dbo].[HR_Designation]
                                        WHERE  DesignationTitle =  @DesignationTitle
                                    );";


                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.DesignationTitle,
                        Status = "Y",
                        CreatedBy = con.GetLoggedUserName(),
                        CreateDate = DateTime.Now,
                        EditedBy = "Fresh",

                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update_to_HR_Designation(VM_Designation model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_Designation]
                                    SET                                        
                                        Status = @Status,
                                        EditedBy = @EditedBy,
                                        EditDate = @EditDate
                                        where DesigID=@DesigID;
                                ";
                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        Status = status(model.Status),
                        EditedBy = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        model.DesigId
                    });
                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region EmployeeType
        public bool Save_to_HR_EmployeeType(VM_EmployeeType model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    INSERT INTO [CoreDB].[dbo].[HR_EmployeeType]
                                    (
                                        EmployeeType,
                                        Status,
                                        CreatedBy,
                                        CreateDate,
                                        EditedBy
                                    )
                                    SELECT 
                                        @EmployeeType,
                                        @Status,
                                        @CreatedBy,
                                        @CreateDate,
                                        @EditedBy
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM [CoreDB].[dbo].[HR_EmployeeType]
                                        WHERE  EmployeeType =  @EmployeeType
                                    );";


                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.EmployeeType,
                        Status = "Y",                        
                        CreatedBy = con.GetLoggedUserName(),
                        CreateDate = DateTime.Now,
                        EditedBy = "Fresh",
                        model.EmpTypeId

                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update_to_HR_EmployeeType(VM_EmployeeType model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_EmployeeType]
                                    SET                                        
                                        Status = @Status,
                                        EditedBy = @EditedBy,
                                        EditDate = @EditDate
                                        where EmpTypeID=@EmpTypeID;
                                ";
                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        Status = status(model.Status),
                        EditedBy = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        model.EmpTypeId
                    });
                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region Education Type        
        public bool Save_to_HR_EmpEducationType(VM_EmpEducationType model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    INSERT INTO [CoreDB].[dbo].[HR_EmpEducationType]
                                    (
                                        EducationType,
                                        Status,
                                        CreatedBy,
                                        CreateDate,
                                        EditedBy
                                    )
                                    SELECT 
                                        @EducationType,
                                        @Status,
                                        @CreatedBy,
                                        @CreateDate,
                                        @EditedBy
                                    WHERE NOT EXISTS (
                                        SELECT 1
                                        FROM [CoreDB].[dbo].[HR_EmpEducationType]
                                        WHERE  EducationType =  @EducationType
                                    );";


                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.EducationType,
                        Status = "Y",
                        CreatedBy = con.GetLoggedUserName(),
                        CreateDate = DateTime.Now,
                        EditedBy = "Fresh",
                        model.EmpEduTypeId

                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update_to_HR_EmpEducationType(VM_EmpEducationType model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_EmpEducationType]
                                    SET                                        
                                        Status = @Status,
                                        EditedBy = @EditedBy,
                                        EditDate = @EditDate
                                        where EmpEduTypeID=@EmpEduTypeID;
                                ";
                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        Status = status(model.Status),
                        EditedBy = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        model.EmpEduTypeId
                    });
                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region Salary Matrix
        public bool Update_to_HR_SalaryMatrix(VM_SalaryMatrix model)
        {
            
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    const int matrixid = 1;
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[HR_SalaryMatrix]
                                    SET 
                                        BasicPercent           = @BasicPercent,
                                        HouseRentPercent       = @HouseRentPercent,
                                        MedicalAllowPercent    = @MedicalAllowPercent,
                                        ConveyanceAllowPercent = @ConveyanceAllowPercent,
                                        EntertainAllowPercent  = @EntertainAllowPercent,
                                        StampDeduction         = @StampDeduction,
                                        PfemployeePercent      = @PfemployeePercent,
                                        EditedBy               = @EditedBy,
                                        EditDate               = @EditDate
                                    WHERE 
                                        SalaryMatrixID         = @SalaryMatrixID;
                                ";

                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.BasicPercent,
                        model.HouseRentPercent,
                        model.MedicalAllowPercent,
                        model.ConveyanceAllowPercent,
                        model.EntertainAllowPercent,
                        model.StampDeduction,
                        model.PfemployeePercent,
                        EditedBy = con.GetLoggedUserName(),
                        EditDate = DateTime.Now,
                        @SalaryMatrixID=matrixid
                    });

                    if (rowsAffected > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
