using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AST_Intranet.Models.Database;
using static AST_Intranet.Models.Database.DatabaseConnector;

namespace AST_Intranet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string dbStatusMessage = DatabaseConnector.TestConnection();
            ViewBag.DbStatusMessage = dbStatusMessage;

            List<Department> departments = DatabaseConnector.GetDepartments();
            ViewBag.Departments = departments;

            // Fetch the list of employees (dynamic columns using Dictionary)
            List<Dictionary<string, object>> employees = DatabaseConnector.GetEmployees();
            ViewBag.Employees = employees;

            return View();
        }
        // Action to handle the search
        //public ActionResult Search(string query)
        //{
        //    if (string.IsNullOrEmpty(query))
        //    {
        //        return View("DashboardView"); // Return the same page if no query
        //    }

        //    // Simulate search by searching through a predefined list or use a database query.
        //    var searchResults = SearchWebsiteContent(query);

        //    // Return the search results to the view
        //    ViewBag.Query = query;
        //    ViewBag.SearchResults = searchResults;
        //    return View("DashboardView"); // Render the Dashboard page again with results
        //}

        //// Example search function (replace with actual search logic)
        //private List<string> SearchWebsiteContent(string query)
        //{
        //    // Example: This would normally be a database or site content search
        //    var allContent = new List<string> {
        //    "Dashboard", "Employees", "Updates", "Town Hall", "Resources", "Manuals",
        //    "Achievements", "Policies", "Events"
        //};

        //    return allContent.Where(content => content.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        //}
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}