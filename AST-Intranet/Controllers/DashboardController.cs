using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AST_Intranet.Models.Database;
using AST_Intranet.Models;
using Newtonsoft.Json;

namespace AST_Intranet.Controllers
{
    public class DashboardController : Controller
    {
        // Path to the network shared folder
        private string baseFolderPath = @"\\astns\General\INTRANET\";


        // Method to read event metadata from JSON file
        public List<EventMetadata> GetEventMetadata()
        {
            string filePath = Path.Combine(baseFolderPath, "Homepage/Events Images/EventMetadata.json");

            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                var events = JsonConvert.DeserializeObject<List<EventMetadata>>(json);
                return events;
            }

            return new List<EventMetadata>(); // Return an empty list if the file doesn't exist
        }

        // GET: Dashboard
        public ActionResult DashboardView()
        {
            // Fetching Birthday, Anniversary, and New Joiners
            List<BirthdayViewModel> birthdays;
            DashboardDBConnector.GetEmployeeBirthday(out birthdays);
            ViewBag.Birthdays = birthdays;

            List<WorkAnniversaryViewModel> workAnniversaries;
            DashboardDBConnector.GetEmployeeWorkAnniversary(out workAnniversaries);
            ViewBag.workAnniversary = workAnniversaries;

            List<NewJoinerViewModel> newJoiners;
            DashboardDBConnector.GetNewJoiners(out newJoiners);
            ViewBag.NewJoiners = newJoiners;

            // Fetch video files from folders
            var corporateVideo = GetVideoFromFolder("Homepage");
            var bgVideo = GetVideoFromFolder("Homepage/Corporate Video");
            ViewBag.CorporateVideoUrl = corporateVideo.Any() ? Url.Action("GetVideo", "Dashboard", new { folderName = "Homepage", fileName = corporateVideo.First() }) : null;
            ViewBag.BgVideoUrl = bgVideo.Any() ? Url.Action("GetVideo", "Dashboard", new { folderName = "Homepage/Corporate Video", fileName = bgVideo.First() }) : null;

            // Fetch other documents
            var cafeteriaMenu = GetDocumentsFromFolder("Homepage/Cafeteria Menu");
            var contactList = GetDocumentsFromFolder("Homepage/Contact List");
            var yearlyHolidays = GetDocumentsFromFolder("Homepage/Yearly Holidays");
            ViewBag.CafeteriaMenu = cafeteriaMenu;
            ViewBag.ContactList = contactList;
            ViewBag.YearlyHolidays = yearlyHolidays;

            // Fetch event metadata and event images
            List<EventMetadata> events = GetEventMetadata();
            ViewBag.EventMetadata = events;

            var eventImages = GetImagesFromFolder("Homepage/Events Images");
            ViewBag.EventImages = eventImages;

            return View();
            //List<string> birthdays;
            //List<string> anniversaries;

            //// Fetch birthdays and anniversaries using the UpdateDBConnector method
            //UpdateDBConnector.GetEmployeeBirthdaysAndAnniversaries(out birthdays, out anniversaries);

            //// Pass the fetched lists to the view
            //ViewBag.Birthdays = birthdays;
            //ViewBag.Anniversaries = anniversaries;


           
        }
        public ActionResult CelebrationPopup()
        {
            List<CelebrationViewModel> celebrations;
            DashboardDBConnector.GetEmployeeCelebrations(out celebrations);
            Console.WriteLine($"Number of celebrations found: {celebrations.Count}");
            ViewBag.Celebrations = celebrations;
            return View();
        }




        //// Fetch the video files from the 'Homepage' folder
        //var videoFiles = GetVideoFromFolder("tp");

        //// If there are any videos, pass the first video URL to the view
        //if (videoFiles.Any())
        //{
        //    string videoFile = videoFiles.First(); // Get the first video or choose as needed
        //    string videoUrl = Url.Action("GetVideo", "Dashboard", new { folderName = "tp", fileName = videoFile });
        //    ViewBag.VideoUrl = videoUrl;  // Pass the video URL to the view
        //}
        //else
        //{
        //    ViewBag.VideoUrl = null;  // No video available
        //}



        // Corrected method to fetch only video files from the specified folder
        private List<string> GetVideoFromFolder(string folderName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName); // Network path
                var videoFiles = new List<string>();

                if (Directory.Exists(folderPath))
                {
                    // Get all files in the folder
                    var allFiles = Directory.GetFiles(folderPath);

                    // List of allowed video extensions
                    var allowedExtensions = new List<string> { ".mp4", ".avi", ".mov", ".wmv", ".mkv", ".flv", ".mpeg" };

                    foreach (var file in allFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        // Only add files with allowed extensions
                        if (allowedExtensions.Contains(fileExtension))
                        {
                            videoFiles.Add(fileName); // Add valid video file to the list
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Folder not found: " + folderPath);
                }

                return videoFiles;
            }
            catch (Exception ex)
            {
                // Handle errors like folder not accessible, etc.
                Console.WriteLine("Error accessing folder: " + ex.Message);
                return new List<string>();
            }
        }

        // Action method to serve a specific video file
        public ActionResult GetVideo(string folderName, string fileName)
        {
            try
            {
                // Combine the folder path and file name to get the full file path
                string folderPath = Path.Combine(baseFolderPath, folderName); // Network path
                string filePath = Path.Combine(folderPath, fileName); // Complete file path

                if (System.IO.File.Exists(filePath))
                {
                    // Get file extension and determine MIME type based on the extension
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    string contentType = "application/octet-stream"; // Default MIME type for binary files

                    // Determine MIME type based on file extension
                    switch (fileExtension)
                    {
                        case ".mp4":
                            contentType = "video/mp4";
                            break;
                        case ".avi":
                            contentType = "video/x-msvideo";
                            break;
                        case ".mov":
                            contentType = "video/quicktime";
                            break;
                        case ".wmv":
                            contentType = "video/x-ms-wmv";
                            break;
                        case ".mkv":
                            contentType = "video/x-matroska";
                            break;
                        case ".flv":
                            contentType = "video/x-flv";
                            break;
                        case ".mpeg":
                            contentType = "video/mpeg";
                            break;
                    }

                    // Return the video content with the correct MIME type
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, contentType);
                }
                else
                {
                    // File not found, return a 404 error
                    return HttpNotFound("File not found: " + filePath);
                }
            }
            catch (Exception ex)
            {
                // Log error and return 500 error
                Console.WriteLine("Error accessing file: " + ex.Message);
                return new HttpStatusCodeResult(500, "Error accessing file: " + ex.Message);
            }
        }

        private List<string> GetDocumentsFromFolder(string folderName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName); // Network path
                Console.WriteLine("Folder Path: " + folderPath);  // Debugging line

                var documentFiles = new List<string>();

                if (Directory.Exists(folderPath))
                {
                    var allFiles = Directory.GetFiles(folderPath);
                    var allowedExtensions = new List<string> { ".doc", ".docx", ".ppt", ".pptx", ".pdf", ".xls", ".xlsx" };

                    foreach (var file in allFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        if (allowedExtensions.Contains(fileExtension))
                        {
                            documentFiles.Add(fileName);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Folder not found: " + folderPath);
                }

                return documentFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing folder: " + ex.Message);
                return new List<string>(); // Return empty list on error
            }
        }


        public ActionResult GetDocument(string folderName, string fileName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName);
                string filePath = Path.Combine(folderPath, fileName);

                Console.WriteLine("Accessing file at: " + filePath);  // Debugging line

                if (System.IO.File.Exists(filePath))
                {
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    string contentType = "application/octet-stream"; // Default MIME type

                    // MIME type determination
                    switch (fileExtension)
                    {
                        case ".doc":
                        case ".docx":
                            contentType = "application/msword";
                            break;
                        case ".ppt":
                        case ".pptx":
                            contentType = "application/vnd.ms-powerpoint";
                            break;
                        case ".pdf":
                            contentType = "application/pdf";
                            break;
                        case ".xls":
                        case ".xlsx":
                            contentType = "application/vnd.ms-excel";
                            break;
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, contentType);
                }
                else
                {
                    return HttpNotFound($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing file: " + ex.Message);
                return new HttpStatusCodeResult(500, "Error accessing file: " + ex.Message);
            }
        }

        // Method to get images from the folder (as already implemented)
        private List<string> GetImagesFromFolder(string folderName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName);
                var imageFiles = new List<string>();

                if (Directory.Exists(folderPath))
                {
                    var allFiles = Directory.GetFiles(folderPath);
                    var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                    foreach (var file in allFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        if (allowedExtensions.Contains(fileExtension))
                        {
                            imageFiles.Add(fileName);
                        }
                    }
                }
                return imageFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing folder: " + ex.Message);
                return new List<string>(); // Return empty list on error
            }
        }


        /*it will open only image onnext window on browser*/
        public ActionResult GetImage(string folderName, string fileName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName);
                string filePath = Path.Combine(folderPath, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    string contentType = "image/jpeg"; // Default MIME type for images

                    // MIME type determination
                    switch (fileExtension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            contentType = "image/jpeg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".bmp":
                            contentType = "image/bmp";
                            break;
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, contentType);
                }
                else
                {
                    return HttpNotFound($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing file: " + ex.Message);
                return new HttpStatusCodeResult(500, "Error accessing file: " + ex.Message);
            }
        }
    }
}
