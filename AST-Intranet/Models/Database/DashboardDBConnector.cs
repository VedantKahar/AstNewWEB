using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AST_Intranet.Models.Database
{
    public class DashboardDBConnector
    {
        public static void GetEmployeeBirthday(out List<BirthdayViewModel> birthdays)
        {
            birthdays = new List<BirthdayViewModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    var today = DateTime.Today;

                    string query = @"
                SELECT EMP_CODE, EMP_NAME, DOB, DEPARTMENT, DESIGNATION, LOCATION 
                FROM cim_emp_master_list
                WHERE TO_CHAR(DOB, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD')
                AND STATUS = '1'
            ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empName = reader["EMP_NAME"].ToString();
                                var dob = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : (DateTime?)null;
                                var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
                                var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
                                var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";

                                // Handle birthdays
                                if (dob.HasValue && dob.Value.Month == today.Month && dob.Value.Day == today.Day)
                                {
                                    var age = today.Year - dob.Value.Year;
                                    if (today < dob.Value.AddYears(age)) age--; // Adjust age if birthday hasn't occurred yet

                                    // Add employee birthday details to the list
                                    birthdays.Add(new BirthdayViewModel
                                    {
                                        EmployeeName = empName,
                                        Age = age,
                                        Department = department,
                                        Designation = designation,
                                        Location = location
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
                Console.WriteLine($"Error fetching birthdays: {ex.Message}");
            }
        }

        public static void GetEmployeeWorkAnniversary(out List<WorkAnniversaryViewModel> employeeAnniversary)
        {
            employeeAnniversary = new List<WorkAnniversaryViewModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    var today = DateTime.Today;

                    string query = @"
                        SELECT EMP_CODE,EMP_NAME, DOJ, DEPARTMENT, DESIGNATION, LOCATION 
                        FROM cim_emp_master_list
                        WHERE  TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD')
                        AND STATUS = 'Active'
            ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empName = reader["EMP_NAME"].ToString();
                                var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;
                                var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
                                var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
                                var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";


                                // Handle work anniversaries
                                if (doj.HasValue && doj.Value.Month == today.Month && doj.Value.Day == today.Day)
                                {
                                    var yearsOfService = today.Year - doj.Value.Year;
                                    if (today < doj.Value.AddYears(yearsOfService)) yearsOfService--; // Adjust age if birthday hasn't occurred yet

                                    // Add employee birthday details to the list
                                    employeeAnniversary.Add(new WorkAnniversaryViewModel
                                    {
                                        EmployeeName = empName,
                                        Year = yearsOfService,
                                        Department = department,
                                        Designation = designation,
                                        Location = location
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
                Console.WriteLine($"Error fetching anniversaries: {ex.Message}");
            }
        }

        /*New Joinee database*/
        // Method to get new joiners (past 3 months)
        public static void GetNewJoiners(out List<NewJoinerViewModel> newJoinersList)
        {
            newJoinersList = new List<NewJoinerViewModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EMP_CODE, EMP_NAME, DEPARTMENT, DESIGNATION, DOJ FROM cim_emp_master_list WHERE DOJ >= :joinDate";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":joinDate", OracleDbType.Date)).Value = DateTime.Now.AddMonths(-12);

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empCode = reader["EMP_CODE"].ToString();
                                var empName = reader["EMP_NAME"].ToString();
                                var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
                                var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
                                var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;

                                // Add new joiner details to the list
                                if (doj.HasValue)
                                {
                                    newJoinersList.Add(new NewJoinerViewModel
                                    {
                                        EmployeeCode = empCode,
                                        EmployeeName = empName,
                                        Department = department,
                                        Designation = designation,
                                        DateOfJoining = doj.Value
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

        public static void GetEmployeeCelebrations(out List<CelebrationViewModel> celebrations)
        {
            celebrations = new List<CelebrationViewModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    var today = DateTime.Today;

                    string query = @"
                SELECT EMP_CODE, EMP_NAME, DOB, DOJ, DEPARTMENT, DESIGNATION, LOCATION, IMAGE_URL
                FROM cim_emp_master_list
                WHERE (TO_CHAR(DOB, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD') OR TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD'))
                AND STATUS = '1'
            ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empName = reader["EMP_NAME"].ToString();
                                var dob = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : (DateTime?)null;
                                var doj = reader["DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["DOJ"]) : (DateTime?)null;
                                var department = reader["DEPARTMENT"] != DBNull.Value ? reader["DEPARTMENT"].ToString() : "N/A";
                                var designation = reader["DESIGNATION"] != DBNull.Value ? reader["DESIGNATION"].ToString() : "N/A";
                                var location = reader["LOCATION"] != DBNull.Value ? reader["LOCATION"].ToString() : "N/A";
                                var imageUrl = reader["IMAGE_URL"] != DBNull.Value ? reader["IMAGE_URL"].ToString() : "~/Images/default-avatar.png";

                                if (dob.HasValue && dob.Value.Month == today.Month && dob.Value.Day == today.Day)
                                {
                                    var age = today.Year - dob.Value.Year;
                                    if (today < dob.Value.AddYears(age)) age--;

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

                                if (doj.HasValue && doj.Value.Month == today.Month && doj.Value.Day == today.Day)
                                {
                                    var yearsOfService = today.Year - doj.Value.Year;
                                    if (today < doj.Value.AddYears(yearsOfService)) yearsOfService--;

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