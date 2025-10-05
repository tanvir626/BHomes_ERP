using Bhomes_ERP.Models;
using Bhomes_ERP.Models.DropDown;
using Bhomes_ERP.Models.VM_Model;
using Bhomes_ERP.Repository.Interface;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bhomes_ERP.Repository.Implementation
{
    public class Im_PageController(IDapper con, UserManager<ApplicationUser> userManager) : IPageController
    {

        public List<SelectListItem> Get_DDPage_Role()
        {
            using (var connection = new SqlConnection(con.Dappercon()))
            {
                string sql = @"SELECT Id, Name FROM AspNetRoles";

                var roles = connection.Query<DD_Page_Role>(sql).ToList();

                // Map to SelectListItem inside the method
                var selectList = roles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList();

                return selectList;
            }
        }

        public List<SelectListItem> Get_DDPage_Action()
        {
            using (var connection = new SqlConnection(con.Dappercon()))
            {
                string sql = @"SELECT Id, SubCateName FROM SubCategories";

                var Methods = connection.Query<DD_Page_Method>(sql).ToList();

                // Map to SelectListItem inside the method
                var selectList = Methods.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.SubCateName
                }).ToList();

                return selectList;
            }
        }

        public List<SelectListItem> Get_DDPage_Controller()
        {
            using (var connection = new SqlConnection(con.Dappercon()))
            {
                string sql = @"SELECT Id, CateName FROM Categories";

                var Methods = connection.Query<DD_Page_Controller>(sql).ToList();

                // Map to SelectListItem inside the method
                var selectList = Methods.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.CateName
                }).ToList();

                return selectList;
            }
        }

        public bool Save_to_RolePermission(VM_PageCrud model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                INSERT INTO [CoreDB].[dbo].[RolePermissions]
                                (
                                    RoleId,
                                    SubCategoryId,
                                    CanView,
                                    CanCreate,
                                    CanEdit,
                                    CanDelete,
                                    CreateDate,
                                    UpdateDate,
                                    CategoriesID,
                                    Createdby,
                                    Editedby
                                )
                                SELECT 
                                    @RoleId,
                                    @SubCategoryId,
                                    @CanView,
                                    @CanCreate,
                                    @CanEdit,
                                    @CanDelete,
                                    @CreateDate,
                                    @UpdateDate,
                                    @CategoriesID,
                                    @Createdby,
                                    @Editedby
                                WHERE NOT EXISTS (
                                    SELECT 1
                                    FROM [CoreDB].[dbo].[RolePermissions]
                                    WHERE RoleId = @RoleId
                                      AND SubCategoryId = @SubCategoryId
                                      AND CategoriesID = @CategoriesID
                                );
                            ";

                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.RoleId,
                        model.SubCategoryId,
                        model.CanView,
                        model.CanCreate,
                        model.CanEdit,
                        model.CanDelete,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        model.CategoriesID,
                        Createdby = con.GetLoggedUserName(),
                        Editedby = "Fresh"
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

        public List<Table_RolePermission> Get_RolePermissions()
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                SELECT 
                    rp.Id AS Id,
                    r.Name AS RoleName,
                    sc.SubCateName AS Method,
                    c.CateName AS Controller,
                    CASE WHEN rp.CanView = 1 THEN 'Y' ELSE 'N' END AS CanView,
                    CASE WHEN rp.CanCreate = 1 THEN 'Y' ELSE 'N' END AS CanCreate,
                    CASE WHEN rp.CanEdit = 1 THEN 'Y' ELSE 'N' END AS CanEdit,
                    CASE WHEN rp.CanDelete = 1 THEN 'Y' ELSE 'N' END AS CanDelete,
                    rp.CreateDate,
                    rp.UpdateDate
                FROM RolePermissions rp
                INNER JOIN SubCategories sc ON sc.Id = rp.SubCategoryId
                INNER JOIN Categories c ON c.Id = rp.CategoriesID
                INNER JOIN AspNetRoles r ON r.Id = rp.RoleId";

                    var data = connection.Query<Table_RolePermission>(sql).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Table_RolePermission>();
            }
        }

        public string RoleNameByID(string ID)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"SELECT r.Name FROM RolePermissions rp
                                    inner join AspNetRoles r on r.Id=rp.RoleId
                                    where rp.Id=@ID";

                    var roleName = connection.QuerySingleOrDefault<string>(sql, new { Id = Convert.ToInt32(ID) });

                    if (string.IsNullOrEmpty(roleName))
                        return "Empty";
                    return roleName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Something went Wrong";
            }
        }

        public string PageLinkById(string ID)
        {
            using (var connection = new SqlConnection(con.Dappercon()))
            {
                try
                {
                    string sql = @"
                                SELECT 
                                c.CateName+'/'+s.SubCateName as link
                                FROM [CoreDB].[dbo].[RolePermissions]as rp
                                inner join Categories c on c.Id=rp.CategoriesID
                                inner join SubCategories s on s.Id=rp.SubCategoryId
                                where rp.Id=@Id
                            ";

                    var PageLink = connection.QuerySingleOrDefault<string>(sql, new { Id = Convert.ToInt32(ID) });

                    if (string.IsNullOrEmpty(PageLink))
                        return "Empty";
                    return PageLink;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return "Something went Wrong";
                }
            }
        }

        public bool Update_to_RolePermission(VM_PageCrud model)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = @"
                                    UPDATE [CoreDB].[dbo].[RolePermissions]
                                    SET             
                                        CanView = @CanView,
                                        CanCreate = @CanCreate,
                                        CanEdit = @CanEdit,
                                        CanDelete = @CanDelete,
                                        UpdateDate = @UpdateDate,
                                        EditedBy = @EditedBy
                                        WHERE Id = @ID;
                                ";

                    // Dapper parameter binding
                    var rowsAffected = connection.Execute(sql, new
                    {
                        model.CanView,
                        model.CanCreate,
                        model.CanEdit,
                        model.CanDelete,
                        UpdateDate = DateTime.Now,
                        Editedby = con.GetLoggedUserName(),
                        ID = model.Id
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
    }
}
