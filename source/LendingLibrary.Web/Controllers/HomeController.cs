using System.Web.Mvc;
using LendingLibrary.Web.ViewModels;
using System.Collections.Generic;

namespace LendingLibrary.Web.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        //TODO: use for mananging teams and items
        public ActionResult Index()
        {
            var itemViewModel = new List<ItemViewModel>();
            return View(itemViewModel);
        }
    }
}

