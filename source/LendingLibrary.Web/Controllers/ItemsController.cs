using System;
using System.Web.Mvc;
using LendingLibrary.Web.Repository;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            if (itemsRepository == null) throw new ArgumentNullException(nameof(itemsRepository));
            _itemsRepository = itemsRepository;
        }

        public ActionResult Index()
        {
            return View(_itemsRepository.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ItemViewModel itemViewModel)
        {
            
            if (ModelState.IsValid)
            {
                _itemsRepository.Save(itemViewModel);
                return RedirectToAction("Index", "Items");
            }
            return View(itemViewModel);
        }
    }
}