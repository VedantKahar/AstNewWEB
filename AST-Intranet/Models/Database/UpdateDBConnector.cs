using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AST_Intranet.Models.Database
{
    public class UpdateDBConnector
    {
        //public static void GetEmployeeCelebrations(out List<CelebrationViewModel> celebrations, string baseUrl)
        //{
        //    celebrations = new List<CelebrationViewModel>();

        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Use current date
        //            DateTime currentDate = DateTime.Now;

        //            string query = @"
        //                SELECT EMP_CODE, EMP_NAME, DOB, DOJ, DEPARTMENT, DESIGNATION, LOCATION
        //                FROM cim_emp_master_list
        //                WHERE (TO_CHAR(DOB, 'MM-DD') = TO_CHAR(:CurrentDate, 'MM-DD') OR TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(:CurrentDate, 'MM-DD'))
        //                AND STATUS = 'Active'
        //            ";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                // Add current date as parameter
        //                command.Parameters.Add(new OracleParameter(":CurrentDate", OracleDbType.Date)).Value = currentDate;

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
        //                        if (dob.HasValue && dob.Value.Month == currentDate.Month && dob.Value.Day == currentDate.Day)
        //                        {
        //                            var age = currentDate.Year - dob.Value.Year;
        //                            if (currentDate < dob.Value.AddYears(age)) age--; // Adjust if birthday hasn't occurred yet

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
        //                        if (doj.HasValue && doj.Value.Month == currentDate.Month && doj.Value.Day == currentDate.Day)
        //                        {
        //                            var yearsOfService = currentDate.Year - doj.Value.Year;
        //                            if (currentDate < doj.Value.AddYears(yearsOfService)) yearsOfService--; // Adjust if anniversary hasn't occurred yet

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
        //        // Log error message
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

                    // Use a hardcoded date for testing (e.g., March 10, 2025)
                    DateTime currentDate = new DateTime(2025, 1, 1);

                    string query = @"
                SELECT EMP_CODE, EMP_NAME, DOB, DOJ, DEPARTMENT, DESIGNATION, LOCATION
                FROM cim_emp_master_list
                WHERE (TO_CHAR(DOB, 'MM-DD') = TO_CHAR(:CurrentDate, 'MM-DD') OR TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(:CurrentDate, 'MM-DD'))
                AND STATUS = 'Active'
            ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        // Add current date as parameter
                        command.Parameters.Add(new OracleParameter(":CurrentDate", OracleDbType.Date)).Value = currentDate;

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

                                // Construct the image URL for the employee
                                string imageUrl = $"{baseUrl}/Dashboard/GetEmployeeImage?empCode={empCode}";

                                // Add Birthday celebration
                                if (dob.HasValue && dob.Value.Month == currentDate.Month && dob.Value.Day == currentDate.Day)
                                {
                                    var age = currentDate.Year - dob.Value.Year;
                                    if (currentDate < dob.Value.AddYears(age)) age--; // Adjust if birthday hasn't occurred yet

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

                                // Add Anniversary celebration
                                if (doj.HasValue && doj.Value.Month == currentDate.Month && doj.Value.Day == currentDate.Day)
                                {
                                    var yearsOfService = currentDate.Year - doj.Value.Year;
                                    if (currentDate < doj.Value.AddYears(yearsOfService)) yearsOfService--; // Adjust if anniversary hasn't occurred yet

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
                // Log error message
                Console.WriteLine($"Error fetching celebrations: {ex.Message}");
            }
        }

    }
}
