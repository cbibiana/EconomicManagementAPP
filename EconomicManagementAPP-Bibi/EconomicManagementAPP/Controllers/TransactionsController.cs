using AutoMapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IServicesUsers servicesUsers;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IRepositorieTransactions repositorieTransactions;
        private readonly IMapper mapper;
        public TransactionsController(IServicesUsers servicesUsers,
                                      IRepositorieAccounts repositorieAccounts,
                                      IRepositorieCategories repositorieCategories,
                                      IRepositorieTransactions repositorieTransactions,
                                      IMapper mapper)
        {
            this.servicesUsers = servicesUsers;
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieCategories = repositorieCategories;
            this.repositorieTransactions = repositorieTransactions;
            this.mapper = mapper;
        }

      public async Task<IActionResult> Index()
        {
            var userId = servicesUsers.getUsersId();
            var transactions = await repositorieTransactions.getTransactions( userId);
            return View(transactions);
        }

        public async Task<IActionResult> Create()
        {
            var userId = servicesUsers.getUsersId();
            var model = new TransactionsCreateViewModel();
            model.Accounts = await getAccounts(userId);
            model.Categories = await getCategories(userId, model.OperationTypeId);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionsCreateViewModel model)
        {
            var userId = servicesUsers.getUsersId();

            if (!ModelState.IsValid)
            {
                model.Accounts = await getAccounts(userId);
                model.Categories = await getCategories(userId, model.OperationTypeId);
                return View(model);
            }
            var accounts = await repositorieAccounts.getAccountById(model.AccountId, userId);

            if (accounts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var categories = await repositorieCategories.getCategoriesById(model.CategoryId, userId);

            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            model.UserId = userId;

            if (model.OperationTypeId == OperationTypes.Egress )
            {
                model.Total *= -1;
            }

            await repositorieTransactions.Create(model);
            return RedirectToAction("Index");

        }



        [HttpGet]

        public async Task<IActionResult> Modify(int id)
        {
            var userId = servicesUsers.getUsersId();
            var transactions = await repositorieTransactions.getTransactionsById(id, userId); 
            
            if(transactions is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var model = mapper.Map<TransactionsModifyViewModel>(transactions);

            model.TotalPrevious = model.Total;

            if (model.OperationTypeId == OperationTypes.Egress)
            {
                model.TotalPrevious = model.Total * -1;
            }

            model.AccountPreviousId = transactions.AccountId;
            model.Categories = await getCategories(userId, transactions.OperationTypeId);
            model.Accounts = await getAccounts(userId);

            return View(model);
        }


        [HttpPost]

        public async Task<IActionResult> Modify(TransactionsModifyViewModel model)
        {
            var userId = servicesUsers.getUsersId();

            if (!ModelState.IsValid)
            {
                model.Accounts = await getAccounts(userId);
                model.Categories = await getCategories(userId, model.OperationTypeId);
                return View(model);
            }

            var accounts = await repositorieAccounts.getAccountById(model.AccountId, userId);

            if(accounts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var categories = await repositorieCategories.getCategoriesById(model.CategoryId, userId);

            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var transactions = mapper.Map<Transactions>(model);   

            if(model.OperationTypeId == OperationTypes.Egress)
            {
                transactions.Total *= -1;
            }

            await repositorieTransactions.Modify(transactions, model.TotalPrevious, model.AccountPreviousId);
            return RedirectToAction("Index");

        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id, string UrlReturn = null)
        {
            var userId = servicesUsers.getUsersId();
            var transactions = await repositorieTransactions.getTransactionsById(id, userId);

            if (transactions is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieTransactions.Delete(id);

            if (string.IsNullOrEmpty(UrlReturn))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(UrlReturn);
            }
           
        }

        private async Task<IEnumerable<SelectListItem>> getAccounts(int userId)
        {
            var accounts = await repositorieAccounts.getAccounts(userId);
            return accounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> getCategories(int userId, OperationTypes operationTypes)
        {
            var categories = await repositorieCategories.getCategories(userId, operationTypes);
            return categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> getCategories([FromBody] OperationTypes operationTypes)
        {
            var userId = servicesUsers.getUsersId();
            var categories = await getCategories(userId, operationTypes);
            return Ok(categories);

        }

       }
    
}
