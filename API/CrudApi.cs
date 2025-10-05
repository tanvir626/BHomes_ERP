using Bhomes_ERP.Repository.Interface;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace API
{
    [Authorize]
    [Route("/Api/")]
    [ApiController]
    public class CrudApi(IHttpContextAccessor _httpContextAccessor, IDapper con)
    {        
        [HttpPost("DeleteApi")]
        public JsonResult DeleteApi([FromForm] string TableName, [FromForm] string ColName, [FromForm] string id)
        {
            try
            {
                int.TryParse(id, out int parsedId);

                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = $@"DELETE FROM [{TableName}] WHERE [{ColName}] = @Id";
                    int rows = connection.Execute(sql, new { Id = parsedId });

                    bool info = (rows > 0) ? info = true : info = false;
                    return new JsonResult(new { info });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { info = false });
            }
        }


        [HttpGet("ShowIndivisualRowApi")]
        public JsonResult ShowIndivisualRowApi(string TableName, string ColName, string Id)
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

                    var row = connection.Query(sql, new { Id = parsedId }).ToList();
                    return new JsonResult(new { row });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { row = new List<dynamic>() });
            }
        }


        [HttpGet("ShowTableApi")]
        public JsonResult ShowTableApi(string TableName)
        {
            try
            {
                using (var connection = new SqlConnection(con.Dappercon()))
                {
                    string sql = $@"SELECT * FROM [{TableName}]";
                    var row = connection.Query(sql).ToList();
                    return new JsonResult(new { row });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { row = new List<dynamic>() });
            }
        }
    }
}
