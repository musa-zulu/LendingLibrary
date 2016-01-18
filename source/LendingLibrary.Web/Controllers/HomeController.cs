using System.Web.Mvc;
using LendingLibrary.Web.ViewModels;
using System.Collections.Generic;

namespace LendingLibrary.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
         var itemViewModel = new ItemViewModel
            {
                DaysLentOut = 5,
                ItemName = "Eggs",
                PersonName = "Musa"
            };

            var itemViewModel2 = new ItemViewModel
            {
                DaysLentOut = 5,
                ItemName = "Ball",
                PersonName = "Matt"
            };

            var ItemViewModelList = new List<ItemViewModel>
            {
                itemViewModel, itemViewModel2
            };

            return View(ItemViewModelList);
        }
    }
}

