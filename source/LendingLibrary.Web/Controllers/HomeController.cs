using System.Web.Mvc;
using LendingLibrary.Web.ViewModels;
using System.Collections.Generic;

namespace LendingLibrary.Web.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var itemViewModel = new List<ItemViewModel>();
            return View(itemViewModel);
        }

        public ViewResult Create()
        {
            return View();
        }
    }
}

