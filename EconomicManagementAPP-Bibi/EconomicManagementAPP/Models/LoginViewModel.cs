using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class LoginViewModel : Users
    {
        public bool RememberMe { get; set; }    
    }
}
