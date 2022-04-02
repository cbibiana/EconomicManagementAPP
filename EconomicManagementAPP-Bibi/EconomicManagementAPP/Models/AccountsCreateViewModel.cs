using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Models
{
    public class AccountsCreateViewModel: Accounts
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; }   
    }
}
