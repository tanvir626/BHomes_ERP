
using Bhomes_ERP.Models.DropDown;
using Bhomes_ERP.Repository.Interface;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bhomes_ERP.Repository.Implementation
{
    public class Im_Crud(IDapper con) : Icrud
    {
        public bool Delete(string tableName, string colName, string id)
        {
            try
            {
                int.TryParse(id, out int parsedId);

                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = $@"DELETE FROM [{tableName}] WHERE [{colName}] = @Id";

                    int rows = connection.Execute(sql, new { Id = id });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<dynamic> ShowIndivisualRow(string TableName, string ColName, string Id)
        {
            int.TryParse(Id, out int parsedId);
            try
            {

                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = $@"
                SELECT * 
                FROM [{TableName}] 
                WHERE [{ColName}] = @Id";

                    var data = connection.Query(sql, new { Id = Id }).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<dynamic>();
            }
        }

        public List<dynamic> ShowTable(string TableName)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = $@"SELECT * FROM [{TableName}]";
                    var row = connection.Query(sql).ToList();
                    return row;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<dynamic>();
            }
        }

        public List<SelectListItem> DropDownList(string tableName, string ShowCol, string idcol)
        {
            using (var connection = new SqlConnection(con.Dappercon()))
            {
                string sql = $"SELECT {idcol}, {ShowCol} FROM {tableName}";

                var roles = connection.Query<dynamic>(sql).ToList();

                var selectList = roles.Select(r =>
                {
                    var dict = (IDictionary<string, object>)r;
                    return new SelectListItem
                    {
                        Value = dict[idcol].ToString(),
                        Text = dict[ShowCol].ToString()
                    };
                }).ToList();



                return selectList;
            }
        }


    }
}
