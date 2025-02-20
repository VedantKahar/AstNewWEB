using AST_Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AST_Intranet.Controllers
{
    public class SearchController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext(); // Assuming you have a DbContext

        // GET: Search/GetSuggestions
        public JsonResult GetSuggestions(string query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 2)
            {
                return Json(new List<SearchResult>(), JsonRequestBehavior.AllowGet);
            }

            // Create a list to store the results
            var results = new List<SearchResult>();

            // Example: Search within the Resources
            //var resourcesResults = db.Resources
            //    .Where(r => r.Name.Contains(query) ||
            //                r.Files.Any(f => f.Contains(query)) ||
            //                r.SubCategories.Any(sc => sc.Name.Contains(query) ||
            //                                          sc.Files.Any(f => f.Contains(query))))
            //    .ToList();

            //if (resourcesResults.Any())
            //{
            //    var resourceSuggestions = new List<string>();

            //    foreach (var resource in resourcesResults)
            //    {
            //        // Add the resource name
            //        if (resource.Name.Contains(query))
            //            resourceSuggestions.Add("Resource: " + resource.Name);

            //        // Add files under the resource
            //        foreach (var file in resource.Files.Where(f => f.Contains(query)))
            //        {
            //            resourceSuggestions.Add("File: " + file);
            //        }

            //        // Add subcategory names and files under subcategories
            //        foreach (var subcategory in resource.SubCategories)
            //        {
            //            if (subcategory.Name.Contains(query))
            //                resourceSuggestions.Add("Subcategory: " + subcategory.Name);

            //            foreach (var file in subcategory.Files.Where(f => f.Contains(query)))
            //            {
            //                resourceSuggestions.Add("File (Subcategory): " + file);
            //            }
            //        }
            //    }

            //    if (resourceSuggestions.Any())
            //    {
            //        results.Add(new SearchResult
            //        {
            //            Page = "Resources",
            //            Suggestions = resourceSuggestions
            //        });
            //    }
            //}

            //// Example: Search within the Dashboard content
            //var dashboardResults = db.Dashboards
            //    .Where(d => d.Title.Contains(query) || d.Description.Contains(query))
            //    .Select(d => d.Title)
            //    .ToList();

            //if (dashboardResults.Any())
            //{
            //    results.Add(new SearchResult
            //    {
            //        Page = "Dashboard",
            //        Suggestions = dashboardResults
            //    });
            //}

            //// Example: Search within the Employees table
            //var employeeResults = db.Employees
            //    .Where(e => e.Name.Contains(query) || e.Position.Contains(query))
            //    .Select(e => e.Name)
            //    .ToList();

            //if (employeeResults.Any())
            //{
            //    results.Add(new SearchResult
            //    {
            //        Page = "Employees",
            //        Suggestions = employeeResults
            //    });
            //}

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }

    // Model for Search results
    public class SearchResult
    {
        public string Page { get; set; }
        public List<string> Suggestions { get; set; }
    }
}
