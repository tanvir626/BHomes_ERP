using Microsoft.VisualBasic;

namespace Bhomes_ERP.Models.VM_Model
{
    public class VM_Department
    {
        public int HR_DeptID { get; set; }
        public int HR_DeptCode { get; set; }
        public string? HR_DeptName { get; set; }
        public string? Status { get; set; }
        public string? CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public string? EditUserID { get; set; }
        public DateTime EditDate { get; set; }
    }
}
