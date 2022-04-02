using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Categories
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, ErrorMessage = " Cannot be more than {1} characters")]
        public string Name { get; set; }
        [Display(Name = "Operation Type")]
        public OperationTypes OperationTypeId { get; set; }
        public int UserId { get; set; }
    }
}
