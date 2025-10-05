using Bhomes_ERP.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bhomes_ERP.Controllers
{
    [Authorize]
    public class CrudController (Icrud con) : Controller
    {
        public JsonResult Delete(string TableName, string ColName, string id)
        {
            var info = con.Delete(TableName, ColName, id);
            return new JsonResult(new { info });
        }

        public JsonResult ShowIndivisualDeptRow(string TableName, string ColName, string id)
        {
            var row = con.ShowIndivisualRow(TableName, ColName, id);
            return new JsonResult(new { row });
        }
    }
}
