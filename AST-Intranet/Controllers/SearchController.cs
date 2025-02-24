using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AST_Intranet.Controllers
{
    public class SearchController : Controller
    {
        public JsonResult GetSearchSuggestions(string query)
        {
            List<SearchResult> results = new List<SearchResult>();

            // Prevent unnecessary searches for very short queries
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            string viewsPath = HostingEnvironment.MapPath("~/Views");

            if (viewsPath != null)
            {
                try
                {
                    // Iterate over all the pages
                    string[] cshtmlFiles = Directory.GetFiles(viewsPath, "*.cshtml", SearchOption.AllDirectories)
                                                    .Where(file => !file.Contains(@"\Shared\") && !Path.GetFileName(file).StartsWith("_"))
                                                    .ToArray();

                    foreach (string file in cshtmlFiles)
                    {
                        // Read the content of the .cshtml file
                        string content = System.IO.File.ReadAllText(file);

                        // Extract text content from the view (visible content only)
                        string pageTextContent = ExtractTextFromHtml(content);

                        // Check if the query is in the page content
                        if (pageTextContent.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string controllerName = GetControllerName(file, viewsPath);
                            string viewName = Path.GetFileNameWithoutExtension(file);

                            // Get the matching lines of the text content
                            var matchingLines = pageTextContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                                               .Where(line => line.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                                                               .Take(3) // Limit to 3 matches per file
                                                               .ToList();

                            results.Add(new SearchResult
                            {
                                PageName = viewName,
                                Content = string.Join(" | ", matchingLines),
                                Url = Url.Action(viewName, controllerName) + "?highlight=" + Uri.EscapeDataString(query) // Pass query as a parameter
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle directory access issues
                    Console.WriteLine(ex.Message); // For debugging
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        // Extracts visible text from HTML (ignores tags)
        private string ExtractTextFromHtml(string htmlContent)
        {
            // Strip out HTML tags to leave only visible text
            var strippedText = System.Text.RegularExpressions.Regex.Replace(htmlContent, "<[^>]*?>", string.Empty);
            return strippedText;
        }

        // Get the controller name based on the file path
        private string GetControllerName(string filePath, string viewsPath)
        {
            string relativePath = filePath.Replace(viewsPath, "").TrimStart('\\', '/');
            string[] pathParts = relativePath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            return pathParts.Length > 1 ? pathParts[0] : "Home"; // Default to Home if not found
        }
    }

    // Search result model
    public class SearchResult
    {
        public string PageName { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}
