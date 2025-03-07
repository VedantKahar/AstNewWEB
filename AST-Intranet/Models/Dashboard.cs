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
        //public string ImageUrl { get; set; }   New property for image URL
    }

    public class CelebrationViewModel
    {
        public string EmployeeName { get; set; }
        public string CelebrationType { get; set; }  // "Birthday" or "Anniversary"
        public DateTime Date { get; set; }
        public int Age { get; set; }
        public int Year { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
    }
    public class EventMetadata
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
