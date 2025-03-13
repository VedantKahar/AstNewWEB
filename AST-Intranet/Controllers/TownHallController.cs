using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace AST_Intranet.Controllers
{
    public class TownHallController : Controller
    {
        private string baseFolderPath = @"\\astns\General\INTRANET\TownHall";

        // GET: TownHall
        public ActionResult TownHallView()
        {
            var foldersAndImages = GetFoldersAndImages();
            ViewBag.FoldersAndImages = foldersAndImages;  // Explicitly setting the ViewBag
            return View();
        }


        // Method to get all folders and images from those folders
        private Dictionary<string, List<string>> GetFoldersAndImages()
        {
            try
            {
                var foldersAndImages = new Dictionary<string, List<string>>();

                if (Directory.Exists(baseFolderPath))
                {
                    // Get all subfolders inside the base folder (TownHall)
                    var allFolders = Directory.GetDirectories(baseFolderPath);

                    foreach (var folder in allFolders)
                    {
                        string folderName = Path.GetFileName(folder);
                        var imageFiles = GetImagesFromFolder(folderName);
                        foldersAndImages.Add(folderName, imageFiles);
                    }
                }

                return foldersAndImages;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing folder: " + ex.Message);
                return new Dictionary<string, List<string>>(); // Return empty list on error
            }
        }

        // Method to get images from a specific folder
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

        // Action to fetch image from the folder
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
