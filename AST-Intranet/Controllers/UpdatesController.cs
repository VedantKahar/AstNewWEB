using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AST_Intranet.Models.Database;
using AST_Intranet.Models;
using System.IO;

namespace AST_Intranet.Controllers
{
    public class UpdatesController : Controller
    {
        // GET: Updates
        public ActionResult UpdatesView()
        {
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority); // Gets the base URL of the app

            List<CelebrationViewModel> celebrations = new List<CelebrationViewModel>();
            // Fetch employee celebrations (birthdays & anniversaries)
            UpdateDBConnector.GetEmployeeCelebrations(out celebrations, baseUrl);

            // Pass the fetched list to the view
            ViewBag.Celebrations = celebrations;

            return View();
        }

       
public ActionResult GetEmployeeImage(string empCode)
        {
            try
            {
                // Network shared folder path
                string imageFolderPath = @"\\astns\General\Vedant (intern)\Images";

                // Check for image file extensions
                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                foreach (var ext in allowedExtensions)
                {
                    string imagePath = Path.Combine(imageFolderPath, $"{empCode}{ext}");

                    // Check if the image exists in the network folder
                    if (System.IO.File.Exists(imagePath))
                    {
                        string contentType;

                        if (ext == ".jpg" || ext == ".jpeg")
                        {
                            contentType = "image/jpeg";
                        }
                        else if (ext == ".png")
                        {
                            contentType = "image/png";
                        }
                        else if (ext == ".gif")
                        {
                            contentType = "image/gif";
                        }
                        else if (ext == ".bmp")
                        {
                            contentType = "image/bmp";
                        }
                        else
                        {
                            contentType = "image/jpeg"; // Default to JPEG
                        }

                        // Return the file using FileStream, but don't use 'using' block
                        var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                        return File(fileStream, contentType);
                    }
                }

                // If no image found, return the default image from network or local path
                string defaultImagePath = @"\\astns\General\Vedant (intern)\Images\default-image.jpg";
                if (System.IO.File.Exists(defaultImagePath))
                {
                    return File(defaultImagePath, "image/jpeg");
                }

                // Fallback to local default image if network image not found
                string localDefaultImagePath = Server.MapPath("~/Images/images/profile-pic.jfif");
                return File(localDefaultImagePath, "image/jpeg");
            }
            catch (Exception ex)
            {
                // Log the error (using a logging framework in production)
                Console.WriteLine("Error fetching image: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }
    }
}


