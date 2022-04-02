using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; } = DateTime.Today;
        
        public Decimal Total { get; set; }
        
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [StringLength(maximumLength:1000, ErrorMessage = "La descripción no debe pasar de {1} caracteres")]
        public string Description { get; set; }
        
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        [Display(Name = "Operation Type")]
        public OperationTypes OperationTypeId { get; set; } = OperationTypes.Ingress;


    }
}
