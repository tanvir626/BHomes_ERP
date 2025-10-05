
using Bhomes_ERP.Models.VM_Model;
using Bhomes_ERP.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bhomes_ERP.Controllers
{
    public class PageController(IPageController page, Icrud cd) : Controller
    {
        [Route("/Basic Setup/Page Access")]
        public IActionResult PageCrud()
        {
            GetDropDownList();
            GetDataForView();
            return View();
        }

        private void GetDataForView()
        {
            ViewBag.Data = page.Get_RolePermissions();
        }
        private void GetDropDownList()
        {
            ViewBag.RoleId = page.Get_DDPage_Role();
            ViewBag.Method = page.Get_DDPage_Action();
            ViewBag.Controller = page.Get_DDPage_Controller();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VM_PageCrud model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    bool status = page.Save_to_RolePermission(model);
                    return Json(new
                    {
                        info = status
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return RedirectToAction("Error500");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePage(VM_PageCrud model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    bool status = page.Update_to_RolePermission(model);
                    return Json(new
                    {
                        info = status
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return RedirectToAction("Error500");
        }

        [Route("/500/")]
        public IActionResult Error500() => View();


        [HttpPost]
        public JsonResult Delete(string TableName, string ColName, string id)
        {
            try
            {
                bool status = cd.Delete(TableName, ColName, id);

                return Json(new { info = status });
            }
            catch (Exception ex)
            {
                return Json(new { info = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult ShowIndivisualRow(string TableName, string ColName, string Id)
        {
            try
            {
                var row = cd.ShowIndivisualRow(TableName, ColName, Id);
                string RoleName = page.RoleNameByID(Id);
                string PageLink = page.PageLinkById(Id);

                return Json(new { row, RoleName, PageLink, Info = true });
            }
            catch (Exception ex)
            {
                return Json(new { info = false, message = ex.Message });
            }
        }

    }
}
