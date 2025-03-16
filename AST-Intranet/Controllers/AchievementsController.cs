using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AST_Intranet.Controllers
{
    public class AchievementsController : Controller
    {
        // Path to the network shared folder
        //private string baseFolderPath = @"\\astns\General\";
        private string baseFolderPath = @"D:\INTRANET Dummy Folder\Achievements";

        // Method to fetch achievement data from the JSON file
        public List<AchievementSection> GetAchievementData()
        {
            try
            {
                string filePath = Path.Combine(baseFolderPath, "AchievementsPage.json");

                if (System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);
                    var achievementSections = JsonConvert.DeserializeObject<List<AchievementSection>>(json);
                    return achievementSections;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading achievement data: " + ex.Message);
            }

            return new List<AchievementSection>();
        }

        // GET: achievements
        public ActionResult AchievementsView()
        {
            var achievementData = GetAchievementData();
            ViewBag.AchievementData = achievementData;
            return View();
        }

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


    // Model to hold section and images data
    public class AchievementSection
    {
        public string Section { get; set; }
        public List<Achievement> Images { get; set; }
    }

    public class Achievement
    {
        public string Folder { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


}
