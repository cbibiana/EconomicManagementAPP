using AutoMapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IServicesUsers servicesUsers;
        private readonly IMapper mapper;

        public AccountsController(IRepositorieAccounts repositorieAccounts, 
            IRepositorieAccountTypes repositorieAccountTypes, IServicesUsers servicesUsers,
            IMapper mapper)
        {
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.servicesUsers = servicesUsers;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = servicesUsers.getUsersId();
            var accountTypes = await repositorieAccounts.Search(userId);

            var model = accountTypes.GroupBy(x => x.AccountTypes).Select(group => new IndexAccountViewModel
            {
                AccountTypes = group.Key,
                Account = group.AsEnumerable()
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var userId = servicesUsers.getUsersId();
            var model = new AccountsCreateViewModel();
            model.AccountTypes = await getAccountsType(userId);
            return View(model);
        }
     

        [HttpPost]
        public async Task<IActionResult> Create(AccountsCreateViewModel account)
        {
            var userId = servicesUsers.getUsersId();
            var accountType = await repositorieAccountTypes.getAccountById(account.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if(!ModelState.IsValid)
            {
                account.AccountTypes = await getAccountsType(userId);
                return View(account);
            } 

            
            await repositorieAccounts.Create(account);
            return RedirectToAction("Index");

        }



        private async Task<IEnumerable<SelectListItem>> getAccountsType(int userId)
        {
            var accountTypes = await repositorieAccountTypes.getAccounts(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

        }



        //editar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {

            var userId = servicesUsers.getUsersId();
            var accounts = await repositorieAccounts.getAccountById(id, userId);

            if (accounts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = mapper.Map<AccountsCreateViewModel>(accounts);
            model.AccountTypes = await getAccountsType(userId);
            return View(model);
        }

        [HttpPost]

        public async Task<ActionResult> Modify(AccountsCreateViewModel accountModify)
        {
            var userId = servicesUsers.getUsersId();
            var account = await repositorieAccounts.getAccountById(accountModify.Id, userId);


            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var accountType = await repositorieAccountTypes.getAccountById(accountModify.AccountTypeId, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccounts.Modify(accountModify);
            return RedirectToAction("Index");

        }

        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = servicesUsers.getUsersId();
            var accounts = await repositorieAccounts.getAccountById(id, userId);

            if (accounts is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccounts(int id)
        {
            var userId = servicesUsers.getUsersId();
            var accounts = await repositorieAccounts.getAccountById(id, userId);

            if (accounts is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            await repositorieAccounts.Delete(id);
            return RedirectToAction("Index");
        }
    }

}
