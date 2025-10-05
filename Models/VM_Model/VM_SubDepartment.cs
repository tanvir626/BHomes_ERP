using System.ComponentModel.DataAnnotations;

namespace Bhomes_ERP.Models.VM_Model
{
    public class VM_SubDepartment
    {
       public int Id { get; set; }

        [Required]
        public int HR_DeptID { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string HR_DeptName { get; set; }

        [Required]
        [Display(Name = "SubDepartment Name")]
        public string? HR_SubDeptName { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        
        [Required]
        public DateTime EditDate { get; set; }
        
        [Required]
        public string? Createdby { get; set; }

        [Required]
        public string? Editedby { get; set; }

    }
}
