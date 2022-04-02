using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength:50)]
        [FirstCapitalLetter]
        public string Name { get; set; }
        [Display(Name ="Type Accounts")]
        public int AccountTypeId { get; set; }
        public Decimal Balance { get; set; }
        [StringLength(maximumLength:1000)]
        public string Description { get; set; }
        public string AccountTypes { get; set; }
    }
}
