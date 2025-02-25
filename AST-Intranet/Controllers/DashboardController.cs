using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AST_Intranet.Controllers
{
    public class DashboardController : Controller
    {
        // Path to the network shared folder
        private string baseFolderPath = @"\\astns\General\INTRANET\";

        // GET: Dashboard
        public ActionResult DashboardView()
        {
            // Fetch the video files from the 'Homepage' and 'tp' folder
            var corporateVideo = GetVideoFromFolder("Homepage");
            var bgVideo = GetVideoFromFolder("Homepage/tp");

            // If there are videos, we pick the first one from each folder (you can change logic as needed)
            string corporateVideoUrl = corporateVideo.Any() ? Url.Action("GetVideo", "Dashboard", new { folderName = "Homepage", fileName = corporateVideo.First() }) : null;
            string bgVideoUrl = bgVideo.Any() ? Url.Action("GetVideo", "Dashboard", new { folderName = "Homepage/tp", fileName = bgVideo.First() }) : null;

            // Pass the video URLs to the view
            ViewBag.CorporateVideoUrl = corporateVideoUrl;
            ViewBag.BgVideoUrl = bgVideoUrl;

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
    }
}
