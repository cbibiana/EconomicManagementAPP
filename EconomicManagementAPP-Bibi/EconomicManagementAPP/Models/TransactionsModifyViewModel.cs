namespace EconomicManagementAPP.Models
{
    public class TransactionsModifyViewModel : TransactionsCreateViewModel
    {
        public int AccountPreviousId { get; set; }
        public decimal TotalPrevious { get; set; }

        public string UrlReturn { get; set; }
    }
}
