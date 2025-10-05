using System.ComponentModel.DataAnnotations;

namespace Bhomes_ERP.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CateName { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
