using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IServicesUsers servicesUsers;

        public CategoriesController(IRepositorieCategories repositorieCategories, IServicesUsers servicesUsers)
        {
            this.repositorieCategories = repositorieCategories;
            this.servicesUsers = servicesUsers;
        }

        // Creamos index para ejecutar la interfaz
        public async Task<IActionResult> Index()
        {
            
            var userId = servicesUsers.getUsersId();
            var categories = await repositorieCategories.getCategories(userId);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories categories)
        {
            if (!ModelState.IsValid)
            {
                return View(categories);
            }
            var userId = servicesUsers.getUsersId();
            categories.UserId = userId;
            await repositorieCategories.Create(categories);
            return RedirectToAction("Index");
        }



   
        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {

            var userId = servicesUsers.getUsersId();
            var categories = await repositorieCategories.getCategoriesById(id, userId);

            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(categories);
        }
        
        [HttpPost]

        public async Task<ActionResult> Modify(Categories categorie)
        {
            if (!ModelState.IsValid)
            {
                return View(categorie);
            }

            var userId = servicesUsers.getUsersId();
            var categories = await repositorieCategories.getCategoriesById(categorie.Id, userId);

            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            categorie.UserId = userId;
            await repositorieCategories.Modify(categorie);
            return RedirectToAction("Index");
        }
       
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var userId = servicesUsers.getUsersId();
            var categories = await repositorieCategories.getCategoriesById(id, userId);

            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(categories);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteCategories(int id)
        {

            var userId = servicesUsers.getUsersId();
            var categories = await repositorieCategories.getCategoriesById(id, userId);

            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
