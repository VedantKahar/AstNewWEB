using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Add this to enable LINQ functionality
using System.Web.Mvc;
using AST_Intranet.Models;

namespace AST_Intranet.Controllers
{
    public class ResourcesController : Controller
    {
        private string baseFolderPath = @"\\astns\General\INTRANET\Resources";
        //private string baseFolderPath = @"D:\INTRANET Dummy Folder\Resources";

        public ActionResult ResourcesView()
        {
            var resources = GetResourcesFromFolder(baseFolderPath);

            return View(resources);
        }

        private List<Resource> GetResourcesFromFolder(string parentFolderPath)
        {
            var resources = new List<Resource>();

            if (Directory.Exists(parentFolderPath))
            {
                // Process files directly in the parent folder (root folder)
                var rootFiles = GetFilesFromFolder(parentFolderPath);
                if (rootFiles.Any()) // This checks if there are any files directly in the parent folder
                {
                    resources.Add(new Resource
                    {
                        Name = Path.GetFileName(parentFolderPath), // Label for the root folder (like "Resources")
                        Files = rootFiles, // Files directly in the root folder
                        SubCategories = new List<Subcategory>() // Initialize an empty list for subcategories (if needed later)
                    });
                    Console.WriteLine("FETCHING FILES FROM ROOT FOLDER");
                }
                else
                {
                    Console.WriteLine("No files found in root folder: " + parentFolderPath);
                }

                // Process subfolders (recursive)
                var subFolders = Directory.GetDirectories(parentFolderPath);
                foreach (var subFolderPath in subFolders)
                {
                    string folderName = Path.GetFileName(subFolderPath);

                    // Get files and subcategories from the subfolder
                    var resource = new Resource
                    {
                        Name = folderName,
                        Files = GetFilesFromFolder(subFolderPath), // Get files from the subfolder
                        SubCategories = GetSubcategories(subFolderPath) // Get subcategories from subfolder
                    };

                    resources.Add(resource);
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist: " + parentFolderPath);
            }

            return resources;
        }

        private List<Subcategory> GetSubcategories(string parentFolderPath)
        {
            var subcategories = new List<Subcategory>();

            if (Directory.Exists(parentFolderPath))
            {
                var subFolders = Directory.GetDirectories(parentFolderPath);

                foreach (var subFolderPath in subFolders)
                {
                    string subFolderName = Path.GetFileName(subFolderPath);

                    var subcategory = new Subcategory
                    {
                        Name = subFolderName,
                        Files = GetFilesFromFolder(subFolderPath),
                        SubCategories = GetSubcategories(subFolderPath)
                    };

                    subcategories.Add(subcategory);
                }
            }
            return subcategories;
        }

        private List<string> GetFilesFromFolder(string folderPath)
        {
            var files = new List<string>();

            if (Directory.Exists(folderPath))
            {
                // Get all files in the folder
                var allFiles = Directory.GetFiles(folderPath);

                // Define allowed extensions
                var allowedExtensions = new List<string>
                {
                    ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",".txt",
                    ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".jfif",
                    ".exe", ".msi", ".dmg", ".iso", ".apk",
                    ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm", ".mpeg", ".mpg", ".3gp"
                };

                // Iterate through each file in the folder
                foreach (var file in allFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    // Add file if it has an allowed extension
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        files.Add(fileName);
                    }
                }
            }

            return files;
        }

        public ActionResult GetFiles(string folderName, string fileName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName);
                string filePath = Path.Combine(folderPath, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    string contentType = "application/octet-stream"; // Default MIME type

                    // Dictionary to map file extensions to MIME types
                    var mimeTypes = new Dictionary<string, string>
                    {
                        {".txt", "text/plain" },
                        { ".pdf", "application/pdf" },
                        { ".doc", "application/msword" },
                        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                        { ".xls", "application/vnd.ms-excel" },
                        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                        { ".ppt", "application/vnd.ms-powerpoint" },
                        { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                        { ".jpg", "image/jpeg" }, { ".jpeg", "image/jpeg" }, { ".jfif", "image/jpeg" },
                        { ".png", "image/png" }, { ".gif", "image/gif" }, { ".bmp", "image/bmp" },
                        { ".exe", "application/octet-stream" }, { ".msi", "application/x-msi" },
                        { ".dmg", "application/octet-stream" }, { ".iso", "application/x-iso9660-image" },
                        { ".mp4", "video/mp4" }, { ".avi", "video/x-msvideo" },
                        { ".mov", "video/quicktime" }, { ".mkv", "video/x-matroska" },
                        { ".wmv", "video/x-ms-wmv" }, { ".flv", "video/x-flv" },
                        { ".webm", "video/webm" }, { ".mpeg", "video/mpeg" },
                        { ".mpg", "video/mpeg" }, { ".3gp", "video/3gpp" }
                    };

                    // If MIME type exists for this extension, use it
                    if (mimeTypes.ContainsKey(fileExtension))
                    {
                        contentType = mimeTypes[fileExtension];
                    }

                    // Set the file to be opened in the browser by attaching Content-Disposition header as 'inline'
                    Response.AppendHeader("Content-Disposition", $"inline; filename={fileName}");

                    var fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, contentType);
                }
                else
                {
                    return HttpNotFound("File not found: " + filePath);
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