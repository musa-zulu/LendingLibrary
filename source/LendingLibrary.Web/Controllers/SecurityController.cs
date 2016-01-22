using System.Web.Mvc;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Login()
        {
            var securityViewModel = new SecurityViewModel();
            return View(securityViewModel);
        }

        [HttpPost]
        public ActionResult Login(SecurityViewModel viewModel)
        {
            if (viewModel.UserName == "Musa" && viewModel.Password == "Musa")
                return RedirectToAction("Index", "Home");
            return View("Login");
        }
    }
}