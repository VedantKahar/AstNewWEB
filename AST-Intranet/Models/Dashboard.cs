using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AST_Intranet.Models
{
    public class Dashboard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
    public class BirthdayViewModel
    {
        public string EmployeeName { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }

    }

    public class WorkAnniversaryViewModel
    {
        public string EmployeeName { get; set; }
        public int Year { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }

    }

    public class NewJoinerViewModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string ImageUrl { get; set; }  // Property for image URL
        // Construct full path to the image
    }


    public class CelebrationViewModel
    {
        public string EmployeeName { get; set; }
        public string CelebrationType { get; set; } // "Birthday" or "Anniversary"
        public DateTime? Date { get; set; }
        public int Age { get; set; }
        public int Year { get; set; } // For anniversaries
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; } // Add Image URL
    }


    public class EventMetadata
    {
        public string Folder { get; set; } // Folder containing event images
        public string Image { get; set; }  // Selected image for display
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AchievementMetadata
    {
        public string Folder { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
    }

}
