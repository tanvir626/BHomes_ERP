using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Bhomes_ERP.Repository.Interface
{
    public interface Icrud
    {
        public bool Delete(string TableName, string ColName, string Id);
        public List<dynamic> ShowIndivisualRow(string TableName, string ColName, string Id);

        public List<SelectListItem> DropDownList(string tableName,string ShowCol, string id);
        public List<dynamic> ShowTable(string TableName);
    }
}
