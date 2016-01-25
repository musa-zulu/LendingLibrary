using System.Web.Mvc;
using System.Web.Security;
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
            var userName = viewModel.UserName;
            var password = viewModel.Password;

            if (IsValidUser(userName, password))
            {
                FormsAuthentication.RedirectFromLoginPage(userName, false);
               // return RedirectToAction("Index", "Home");
            }
            
            return View("Login");
        }

        private static bool IsValidUser(string userName, string password)
        {
            return userName == "Musa" && password == "Musa";
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Security");
        }

        public ActionResult Register()
        {
            throw new System.NotImplementedException();
        }
    }
}