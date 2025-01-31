using System;
using System.Web.Mvc;
using AST_Intranet.Models;
using AST_Intranet.Models.Database;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace AST_Intranet.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: employeesView
        public ActionResult employeesView()
        {
            // Fetch data from the database
            int totalEmployees = EmployeeDBConnector.GetTotalEmployees();
            int newJoiners = EmployeeDBConnector.GetNewJoiners();
            int totalDepartments = EmployeeDBConnector.GetTotalDepartments();
            var departments = EmployeeDBConnector.GetDepartments();

            // Pass data to the view using ViewBag
            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.NewJoiners = newJoiners;
            ViewBag.TotalDepartments = totalDepartments;
            ViewBag.Departments = departments;

            return View();
        }

        public ActionResult GetEmployeesByDepartment(string departmentName, int page = 1)
        {
            // Define page size (15 employees per page)
            int pageSize = 15;

            // Get the total number of employees in the specified department
            var totalEmployees = EmployeeDBConnector.GetTotalEmployeesInDepartment(departmentName);

            // Get the employees for the current page in the specified department
            var employees = EmployeeDBConnector.GetEmployeesByDepartment(departmentName, page, pageSize);

            if (employees == null || !employees.Any())
            {
                return Content("No employees found for this department.");
            }

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalEmployees / pageSize);

            // Pass data to the view
            ViewBag.DepartmentName = departmentName;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("_EmployeeList", employees);
        }

        // Fetch data for male/female employees per year

        //public JsonResult GetMaleFemaleEmployeesByYear()
        //{
        //    var maleFemaleData = EmployeeDBConnector.GetMaleFemaleEmployeesByYear();
        //    return Json(maleFemaleData, JsonRequestBehavior.AllowGet);
        //}

        // Fetch data for employees per department per year with year range
        public JsonResult GetEmployeesByDepartmentPerYear(int startYear, int endYear)
        {
            var departmentData = EmployeeDBConnector.GetEmployeesByDepartmentPerYear(startYear, endYear);
            return Json(departmentData, JsonRequestBehavior.AllowGet);
        }
        
    }
}