using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class RegisterUsersViewModel
    {
       
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = " Email is required")] 
        public string Email { get; set; }
 
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
