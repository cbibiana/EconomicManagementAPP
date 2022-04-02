using AutoMapper;
using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Accounts, AccountsCreateViewModel>();
            CreateMap<TransactionsModifyViewModel, Transactions>().ReverseMap();
        }
    }
}
