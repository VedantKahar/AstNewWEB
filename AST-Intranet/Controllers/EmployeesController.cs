using System;
using System.Web.Mvc;
using AST_Intranet.Models;
using AST_Intranet.Models.Database;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Newtonsoft.Json;

namespace AST_Intranet.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: employeesView
        public ActionResult EmployeesView()
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
            ViewBag.TotalEmployees = totalEmployees;

            return View("_EmployeeList", employees);
        }

        public JsonResult GetYearRange()
        {
            List<int> years = new List<int>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT DISTINCT EXTRACT(YEAR FROM doj) AS year
                        FROM cim_emp_master_list
                        WHERE status = 'Active'
                        ORDER BY year";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                years.Add(reader.GetInt32(reader.GetOrdinal("year")));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching year range: {ex.Message}");
            }

            // Return the earliest and latest year from the data
            return Json(new { earliestYear = years.FirstOrDefault(), latestYear = years.LastOrDefault() }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEmployeesByDepartmentTotal()
        {
            var departmentData = EmployeeDBConnector.GetEmployeesByDepartmentTotal();
            if (departmentData == null || departmentData.Count == 0)
            {
                return Json(new { message = "No data available." }, JsonRequestBehavior.AllowGet);
            }
            return Json(departmentData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaleFemaleEmployeesTotal()
        {
            var maleFemaleData = EmployeeDBConnector.GetMaleFemaleEmployeesTotal();

            // Log the data to see what is returned from the database
            Console.WriteLine("Male/Female Employee Data: " + JsonConvert.SerializeObject(maleFemaleData));

            if (maleFemaleData == null || maleFemaleData.Count == 0)
            {
                return Json(new { message = "No data available." }, JsonRequestBehavior.AllowGet);
            }

            return Json(maleFemaleData, JsonRequestBehavior.AllowGet);
        }



    }
}