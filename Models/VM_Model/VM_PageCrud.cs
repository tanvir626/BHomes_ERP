using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bhomes_ERP.Models.VM_Model
{
    public class VM_PageCrud
    {
        public int Id { get; set; }

        public string? Name { get; set; }


        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }


        [Required]
        [Display(Name = "Action")]
        public int SubCategoryId { get; set; }

        [Required]
        [Display(Name = "Controller")]
        public int CategoriesID { get; set; }


        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }

        [Required]
        public string? ActionName { get; set; }

        [Required]
        public string? Createdby { get; set; }

        [Required]
        public string? Editedby { get; set; }
   

    }
}
