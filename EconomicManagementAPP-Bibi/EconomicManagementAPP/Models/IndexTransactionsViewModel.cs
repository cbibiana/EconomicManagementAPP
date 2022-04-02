using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class IndexTransactionsViewModel
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 1000, ErrorMessage = "La descripción no debe pasar de {1} caracteres")]
        public string Description { get; set; }

        public Decimal Total { get; set; }
    }
}
