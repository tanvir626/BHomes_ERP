using AspNetCoreGeneratedDocument;
using Bhomes_ERP.Models.VM_Model;
using Bhomes_ERP.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Net.Http;

namespace Bhomes_ERP.Controllers.HRM
{
    [Authorize]
    public class HRConfigController(IHRConfig hr, Icrud con) : Controller
    {

        #region Department
        private readonly string HR_Department = "HR_Department";
        public IActionResult Department()
        {
            ViewBag.Data = con.ShowTable(HR_Department);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create_Department(VM_Department model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.HR_DeptName))
                {
                    bool status = hr.Save_to_HR_Department(model);
                    return Json(new { info = status });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { info = false});
            }
            return Json(new { info = false });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDepartment(VM_Department model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.HR_DeptName))
                {
                    bool status = hr.Update_to_HR_Department(model);
                    return Json(new { info = status });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return RedirectToAction("Error500");
        }

        #endregion

        #region Sub Department
        private readonly string HR_SubDepartment = "HR_SubDepartment";

        [Route("/HRConfig/Sub Department")]
        public IActionResult SubDepartment()
        {
            Get_SubDept_Pre_Data();           
            return View();
        }
        private void Get_SubDept_Pre_Data()
        {
            ViewBag.DeptID = con.DropDownList(HR_Department, "HR_DeptName", "HR_DeptID");
            ViewBag.Data = hr.Get_HR_SubDepartment(); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create_SubDepartment(VM_SubDepartment model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.HR_SubDeptName))
                {
                    bool status = hr.Save_to_HR_SubDepartment(model);
                    return Json(new { info = status });
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
        public IActionResult UpdateSubDepartment(VM_SubDepartment model)
        {
            try
            {
                    bool status = hr.Update_to_HR_Sub_Department(model);
                    return Json(new { info = status });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return RedirectToAction("Error500");
        }


        #endregion

        #region Shift
        private readonly string HR_Shift = "HR_Shift";
        public IActionResult Shift()
        {
            ViewBag.Data = con.ShowTable(HR_Shift);
            return View();
        }
        #endregion
    }
}
