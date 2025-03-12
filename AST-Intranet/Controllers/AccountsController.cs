using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AST_Intranet.Controllers
{
    public class AccountsController : Controller
    {
        // GET: accounts
        public ActionResult Index()
        {
            return View();
        }
        // Login Action
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (IsValidUser(username, password)) // You need to write the logic for checking valid user credentials
            {
                // Redirect to dashboard if login is successful
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Show an error if login fails
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
        }

        private bool IsValidUser(string username, string password)
        {
            // Write your database authentication logic here
            // E.g., check user credentials against a database
            return true; // Placeholder logic
        }

        // Login Action
        public ActionResult signup()
        {
            return View("signupView");
        }
    }
}