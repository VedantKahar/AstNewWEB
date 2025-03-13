using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using AST_Intranet.Controllers;

namespace AST_Intranet.Models.Database
{
    public class DashboardDBConnector
    {
        //public static void GetEmployeeBirthday(out List<BirthdayViewModel> birthdays)
        //{
        //    birthdays = new List<BirthdayViewModel>();

        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            var today = DateTime.Today;

        //            string query = @"
        //        SELECT EMP_CODE, EMP_NAME, DOB, DEPARTMENT, DESIGNATION, LOCATION 
        //        FROM cim_emp_master_list
        //        WHERE TO_CHAR(DOB, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD')
        //        AND STATUS = '1'
        //    ";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var empName = reader["EMP_NAME"].ToString();
        //                        var dob = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : (DateTime?)null;
        //                        var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
        //                        var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
        //                        var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";

        //                        // Handle birthdays
        //                        if (dob.HasValue && dob.Value.Month == today.Month && dob.Value.Day == today.Day)
        //                        {
        //                            var age = today.Year - dob.Value.Year;
        //                            if (today < dob.Value.AddYears(age)) age--; // Adjust age if birthday hasn't occurred yet

        //                            // Add employee birthday details to the list
        //                            birthdays.Add(new BirthdayViewModel
        //                            {
        //                                EmployeeName = empName,
        //                                Age = age,
        //                                Department = department,
        //                                Designation = designation,
        //                                Location = location
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error message (could be logged to a file or a logging system)
        //        Console.WriteLine($"Error fetching birthdays: {ex.Message}");
        //    }
        //}

        //public static void GetEmployeeWorkAnniversary(out List<WorkAnniversaryViewModel> employeeAnniversary)
        //{
        //    employeeAnniversary = new List<WorkAnniversaryViewModel>();

        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            var today = DateTime.Today;

        //            string query = @"
        //                SELECT EMP_CODE,EMP_NAME, DOJ, DEPARTMENT, DESIGNATION, LOCATION 
        //                FROM cim_emp_master_list
        //                WHERE  TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD')
        //                AND STATUS = 'Active'
        //    ";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var empName = reader["EMP_NAME"].ToString();
        //                        var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;
        //                        var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
        //                        var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
        //                        var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";


        //                        // Handle work anniversaries
        //                        if (doj.HasValue && doj.Value.Month == today.Month && doj.Value.Day == today.Day)
        //                        {
        //                            var yearsOfService = today.Year - doj.Value.Year;
        //                            if (today < doj.Value.AddYears(yearsOfService)) yearsOfService--; // Adjust age if birthday hasn't occurred yet

        //                            // Add employee birthday details to the list
        //                            employeeAnniversary.Add(new WorkAnniversaryViewModel
        //                            {
        //                                EmployeeName = empName,
        //                                Year = yearsOfService,
        //                                Department = department,
        //                                Designation = designation,
        //                                Location = location
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error message (could be logged to a file or a logging system)
        //        Console.WriteLine($"Error fetching anniversaries: {ex.Message}");
        //    }
        //}

        public static void GetNewJoiners(out List<NewJoinerViewModel> newJoinersList, string baseUrl)
        {
            newJoinersList = new List<NewJoinerViewModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EMP_CODE, EMP_NAME, DEPARTMENT, DESIGNATION, DOJ FROM cim_emp_master_list WHERE DOJ >= :joinDate AND status = 'Active' ORDER BY DOJ ASC";


                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":joinDate", OracleDbType.Date)).Value = DateTime.Now.AddMonths(-3);

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empCode = reader["EMP_CODE"].ToString();
                                var empName = reader["EMP_NAME"].ToString();
                                var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
                                var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
                                var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;

                                // Construct the image URL
                                string imageUrl = $"{baseUrl}/Dashboard/GetEmployeeImage?empCode={empCode}";

                                if (doj.HasValue)
                                {
                                    newJoinersList.Add(new NewJoinerViewModel
                                    {
                                        EmployeeCode = empCode,
                                        EmployeeName = empName,
                                        Department = department,
                                        Designation = designation,
                                        DateOfJoining = doj.Value,
                                        ImageUrl = imageUrl // Add the image URL here
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error message (could be logged to a file or a logging system)
                Console.WriteLine($"Error fetching new joiners: {ex.Message}");
            }
        }


        //public static void GetEmployeeCelebrations(out List<CelebrationViewModel> celebrations, string baseUrl)
        //{
        //    celebrations = new List<CelebrationViewModel>();

        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Replace hardcoded test date with current date
        //            DateTime testDate = DateTime.Now; // Current system date and time

        //            string query = @"
        //                SELECT EMP_CODE, EMP_NAME, DOB, DOJ, DEPARTMENT, DESIGNATION, LOCATION
        //                FROM cim_emp_master_list
        //                WHERE (TO_CHAR(DOB, 'MM-DD') = TO_CHAR(:TestDate, 'MM-DD') OR TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(:TestDate, 'MM-DD'))
        //                AND STATUS = 'Active'
        //            ";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                // Adding the current date parameter to the query
        //                command.Parameters.Add(new OracleParameter(":TestDate", OracleDbType.Date)).Value = testDate;

        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var empCode = reader["EMP_CODE"].ToString();
        //                        var empName = reader["EMP_NAME"].ToString();
        //                        var dob = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : (DateTime?)null;
        //                        var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;
        //                        var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
        //                        var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
        //                        var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";

        //                        // Construct the image URL for the employee
        //                        string imageUrl = $"{baseUrl}/Dashboard/GetEmployeeImage?empCode={empCode}";

        //                        // Add Birthday celebration
        //                        if (dob.HasValue && dob.Value.Month == testDate.Month && dob.Value.Day == testDate.Day)
        //                        {
        //                            var age = testDate.Year - dob.Value.Year;
        //                            if (testDate < dob.Value.AddYears(age)) age--;

        //                            celebrations.Add(new CelebrationViewModel
        //                            {
        //                                EmployeeName = empName,
        //                                CelebrationType = "Birthday",
        //                                Date = dob.Value,
        //                                Age = age,
        //                                Department = department,
        //                                Designation = designation,
        //                                Location = location,
        //                                ImageUrl = imageUrl
        //                            });
        //                        }

        //                        // Add Anniversary celebration
        //                        if (doj.HasValue && doj.Value.Month == testDate.Month && doj.Value.Day == testDate.Day)
        //                        {
        //                            var yearsOfService = testDate.Year - doj.Value.Year;
        //                            if (testDate < doj.Value.AddYears(yearsOfService)) yearsOfService--;

        //                            celebrations.Add(new CelebrationViewModel
        //                            {
        //                                EmployeeName = empName,
        //                                CelebrationType = "Anniversary",
        //                                Date = doj.Value,
        //                                Year = yearsOfService,
        //                                Department = department,
        //                                Designation = designation,
        //                                Location = location,
        //                                ImageUrl = imageUrl
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching celebrations: {ex.Message}");
        //    }
        //}


        public static void GetEmployeeCelebrations(out List<CelebrationViewModel> celebrations, string baseUrl)
        {
            celebrations = new List<CelebrationViewModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    //Hardcoded test date
                    DateTime testDate = new DateTime(2025, 3, 1); // For example, March 11, 2025

                    string query = @"
                        SELECT EMP_CODE, EMP_NAME, DOB, DOJ, DEPARTMENT, DESIGNATION, LOCATION
                        FROM cim_emp_master_list
                        WHERE (TO_CHAR(DOB, 'MM-DD') = TO_CHAR(:TestDate, 'MM-DD') OR TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(:TestDate, 'MM-DD'))
                        AND STATUS = 'Active'
                    ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        //Adding the hardcoded test date parameter to the query
                        command.Parameters.Add(new OracleParameter(":TestDate", OracleDbType.Date)).Value = testDate;

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empCode = reader["EMP_CODE"].ToString();
                                var empName = reader["EMP_NAME"].ToString();
                                var dob = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : (DateTime?)null;
                                var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;
                                var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
                                var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
                                var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";

                                //Construct the image URL for the employee

                                string imageUrl = $"{baseUrl}/Dashboard/GetEmployeeImage?empCode={empCode}";

                                //Add Birthday celebration
                                if (dob.HasValue && dob.Value.Month == testDate.Month && dob.Value.Day == testDate.Day)
                                {
                                    var age = testDate.Year - dob.Value.Year;
                                    if (testDate < dob.Value.AddYears(age)) age--;

                                    celebrations.Add(new CelebrationViewModel
                                    {
                                        EmployeeName = empName,
                                        CelebrationType = "Birthday",
                                        Date = dob.Value,
                                        Age = age,
                                        Department = department,
                                        Designation = designation,
                                        Location = location,
                                        ImageUrl = imageUrl
                                    });
                                }

                                //Add Anniversary celebration
                                if (doj.HasValue && doj.Value.Month == testDate.Month && doj.Value.Day == testDate.Day)
                                {
                                    var yearsOfService = testDate.Year - doj.Value.Year;
                                    if (testDate < doj.Value.AddYears(yearsOfService)) yearsOfService--;

                                    celebrations.Add(new CelebrationViewModel
                                    {
                                        EmployeeName = empName,
                                        CelebrationType = "Anniversary",
                                        Date = doj.Value,
                                        Year = yearsOfService,
                                        Department = department,
                                        Designation = designation,
                                        Location = location,
                                        ImageUrl = imageUrl
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching celebrations: {ex.Message}");
            }
        }


    }
}