using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using AST_Intranet.Models;

namespace AST_Intranet.Controllers
{
    public class ResourcesController : Controller
    {
        private string baseFolderPath = @"\\astns\General\INTRANET\Resources"; // Root folder path

        // The main method to load resources and display them
        public ActionResult ResourcesView()
        {
            var resources = GetResourcesFromFolder(baseFolderPath); // Get all resources from the Resources directory
            return View(resources); // Pass the list of resources to the view
        }

        // Helper method to fetch resources and subcategories from a folder recursively
        private List<Resource> GetResourcesFromFolder(string parentFolderPath)
        {
            var resources = new List<Resource>();

            // Get all directories in the given folder
            var subFolders = Directory.GetDirectories(parentFolderPath);

            foreach (var subFolderPath in subFolders)
            {
                string folderName = Path.GetFileName(subFolderPath); // Get the folder name (e.g., "Brochures and Templates")

                var resource = new Resource
                {
                    Name = folderName, // Set the resource name to the folder name
                    Files = GetFilesFromFolder(subFolderPath), // Get files in this folder
                    SubCategories = GetSubcategories(subFolderPath) // Get subfolders and their files recursively
                };

                resources.Add(resource); // Add the resource to the list
            }

            return resources;
        }

        // Helper method to fetch subcategories recursively (subfolders and their files)
        private List<Subcategory> GetSubcategories(string parentFolderPath)
        {
            var subcategories = new List<Subcategory>();

            // Get all directories in the given folder (subcategories)
            var subFolders = Directory.GetDirectories(parentFolderPath);

            foreach (var subFolderPath in subFolders)
            {
                string subFolderName = Path.GetFileName(subFolderPath); // Get the subfolder name

                var subcategory = new Subcategory
                {
                    Name = subFolderName, // Set the subcategory name to the folder name
                    Files = GetFilesFromFolder(subFolderPath), // Get files in this subfolder
                    SubCategories = GetSubcategories(subFolderPath) // Recursively fetch sub-subcategories
                };

                subcategories.Add(subcategory); // Add the subcategory to the list
            }

            return subcategories; // Return the list of subcategories (recursively populated)
        }

        // Helper method to fetch files from a folder
        private List<string> GetFilesFromFolder(string folderPath)
        {
            var files = new List<string>();

            if (Directory.Exists(folderPath))
            {
                // Get all files in the folder
                var allFiles = Directory.GetFiles(folderPath);
                var allowedExtensions = new List<string>
                {
                    ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
                    ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".jfif",
                    ".exe", ".msi", ".dmg", ".iso", ".apk"
                };

                foreach (var file in allFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    // Only add files with allowed extensions
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        files.Add(fileName);
                    }
                }
            }

            return files;
        }

        // Method to serve files based on folder name and file name
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

                    // Map the file extension to the proper MIME type
                    if (fileExtension == ".pdf") contentType = "application/pdf";
                    else if (fileExtension == ".doc") contentType = "application/msword";
                    else if (fileExtension == ".docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    else if (fileExtension == ".xls") contentType = "application/vnd.ms-excel";
                    else if (fileExtension == ".xlsx") contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    else if (fileExtension == ".ppt") contentType = "application/vnd.ms-powerpoint";
                    else if (fileExtension == ".pptx") contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    else if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".jfif") contentType = "image/jpeg";
                    else if (fileExtension == ".png") contentType = "image/png";
                    else if (fileExtension == ".gif") contentType = "image/gif";
                    else if (fileExtension == ".bmp") contentType = "image/bmp";
                    else if (fileExtension == ".exe") contentType = "application/octet-stream";  // Handle executables
                    else if (fileExtension == ".msi") contentType = "application/x-msi";  // MSI installers
                    else if (fileExtension == ".dmg") contentType = "application/octet-stream";  // macOS Disk Image
                    else if (fileExtension == ".iso") contentType = "application/x-iso9660-image";  // ISO image

                    var fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, contentType); // Return the file content to display it in the browser
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
