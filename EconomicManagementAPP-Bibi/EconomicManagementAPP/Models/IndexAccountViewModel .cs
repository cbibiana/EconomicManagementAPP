namespace EconomicManagementAPP.Models
{
    public class IndexAccountViewModel
    {
        public String AccountTypes { get; set; }
        public IEnumerable<Accounts> Account { get; set; }
        public Decimal Balance => Account.Sum(x => x.Balance);
    }
}
