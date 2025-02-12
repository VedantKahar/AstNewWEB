using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace AST_Intranet.Controllers
{
    public class ManualsController : Controller
    {
        // Path to the network shared folder
        private string baseFolderPath = @"\\astns\General\INTRANET\Manuals";
       

        // GET: Manuals
        public ActionResult ManualsView()
        {
            // Fetch files from various network locations
            var cimFiles = GetFilesFromFolder("CIM");
            var crrFiles = GetFilesFromFolder("CRR");
            var eResourceFiles = GetFilesFromFolder("EResources");
            var spineHRExpenseFiles = GetFilesFromFolder("SpineHR/SpineHR-Expense");
            var spineHRTimesheetFiles = GetFilesFromFolder("SpineHR/SpineHR-Timesheet");
            var spineHRPayrollFiles = GetFilesFromFolder("SpineHR/SpineHR-Payroll");
            var productManualsFiles = GetFilesFromFolder("Products");
            var othersFiles = GetFilesFromFolder("Others");

            // Use ViewBag to pass data to the view
            ViewBag.CIMManuals = cimFiles;
            ViewBag.CRRManuals = crrFiles;
            ViewBag.EResourceManuals = eResourceFiles;
            ViewBag.SpineHRExpenseManuals = spineHRExpenseFiles;
            ViewBag.SpineHRTimesheetManuals = spineHRTimesheetFiles;
            ViewBag.SpineHRPayrollManuals = spineHRPayrollFiles;
            ViewBag.ProductManuals = productManualsFiles;
            ViewBag.OthersManuals = othersFiles;

            return View();
        }

        private List<string> GetFilesFromFolder(string folderName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName); // Network path
                var files = new List<string>();

                if (Directory.Exists(folderPath))
                {
                    // Get all files (any format) in the folder
                    var allFiles = Directory.GetFiles(folderPath);

                    // Filter files based on allowed extensions
                    var allowedExtensions = new List<string> { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };

                    foreach (var file in allFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        if (allowedExtensions.Contains(fileExtension))
                        {
                            files.Add(fileName); // Add the valid file to the list
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Folder not found: " + folderPath);
                }

                return files;
            }
            catch (Exception ex)
            {
                // Handle error (folder not accessible, etc.)
                Console.WriteLine("Error accessing folder: " + ex.Message);
                return new List<string>();
            }
        }

        /* pdf will open in browser, other format should be downloaded */
        public ActionResult GetManual(string folderName, string fileName)
        {
            try
            {
                string folderPath = Path.Combine(baseFolderPath, folderName);
                string filePath = Path.Combine(folderPath, fileName);

                // Log the file path and extension for debugging
                Console.WriteLine("Attempting to access file: " + filePath);

                if (System.IO.File.Exists(filePath))
                {
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    string contentType = "application/octet-stream"; // Default MIME type

                    // Log the file extension
                    Console.WriteLine("File extension: " + fileExtension);

                    // Map the file extension to the proper MIME type using if-else
                    if (fileExtension == ".pdf")
                    {
                        contentType = "application/pdf";
                    }
                    else if (fileExtension == ".doc")
                    {
                        contentType = "application/msword";
                    }
                    else if (fileExtension == ".docx")
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    }
                    else if (fileExtension == ".xls")
                    {
                        contentType = "application/vnd.ms-excel";
                    }
                    else if (fileExtension == ".xlsx")
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    }
                    else if (fileExtension == ".ppt")
                    {
                        contentType = "application/vnd.ms-powerpoint";
                    }
                    else if (fileExtension == ".pptx")
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    }

                    // Log the MIME type
                    Console.WriteLine("Using MIME type: " + contentType);

                    var fileBytes = System.IO.File.ReadAllBytes(filePath);

                    // Set the Content-Disposition to 'inline' to show the file in the browser (not download)
                    Response.Headers["Content-Disposition"] = "inline; filename=" + fileName;

                    // Return the file content to display it in the browser
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

        //public ActionResult GetManual(string folderName, string fileName)
        //{
        //    try
        //    {
        //        // Check if the user is authenticated
        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            return new HttpStatusCodeResult(403, "Forbidden: You must be logged in to access the files.");
        //        }

        //        string folderPath = Path.Combine(baseFolderPath, folderName);
        //        string filePath = Path.Combine(folderPath, fileName);

        //        if (System.IO.File.Exists(filePath))
        //        {
        //            string fileExtension = Path.GetExtension(fileName).ToLower();

        //            // If the file is a PDF, return directly
        //            if (fileExtension == ".pdf")
        //            {
        //                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        //                return File(fileBytes, "application/pdf");
        //            }
        //            else
        //            {
        //                // Generate a secure URL to view the file through Google Docs Viewer
        //                string fileUrl = Url.Action("ServeFile", new { folderName = folderName, fileName = fileName });
        //                string googleDocsViewerUrl = "https://docs.google.com/viewer?url=" + Uri.EscapeDataString("https://yourwebsite.com/" + fileUrl);
        //                return Redirect(googleDocsViewerUrl);  // Redirect to Google Docs Viewer
        //            }
        //        }
        //        else
        //        {
        //            return HttpNotFound("File not found: " + filePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpStatusCodeResult(500, "Error accessing file: " + ex.Message);
        //    }
        //}

        //public ActionResult ServeFile(string folderName, string fileName)
        //{
        //    try
        //    {
        //        string folderPath = Path.Combine(baseFolderPath, folderName);
        //        string filePath = Path.Combine(folderPath, fileName);

        //        if (System.IO.File.Exists(filePath))
        //        {
        //            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        //            string contentType = "application/octet-stream"; // Default MIME type

        //            string fileExtension = Path.GetExtension(fileName).ToLower();
        //            if (fileExtension == ".pdf")
        //            {
        //                contentType = "application/pdf";
        //            }
        //            else if (fileExtension == ".doc" || fileExtension == ".docx")
        //            {
        //                contentType = "application/msword";
        //            }
        //            else if (fileExtension == ".xls" || fileExtension == ".xlsx")
        //            {
        //                contentType = "application/vnd.ms-excel";
        //            }
        //            else if (fileExtension == ".ppt" || fileExtension == ".pptx")
        //            {
        //                contentType = "application/vnd.ms-powerpoint";
        //            }

        //            return File(fileBytes, contentType);  // Return the file securely to the user
        //        }
        //        else
        //        {
        //            return HttpNotFound("File not found: " + filePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpStatusCodeResult(500, "Error accessing file: " + ex.Message);
        //    }
        //}
    }
}