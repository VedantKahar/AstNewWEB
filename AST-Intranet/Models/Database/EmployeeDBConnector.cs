using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;

namespace AST_Intranet.Models.Database
{
    public class EmployeeDBConnector
    {
        // Method to get total number of employees
        public static int GetTotalEmployees()
        {
            int totalEmployees = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master_list WHERE STATUS = 'Active'";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalEmployees = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total employees: {ex.Message}");
            }
            return totalEmployees;
        }

        // Method to get new joiners (past month)
        public static int GetNewJoiners()
        {
            int newJoiners = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master_list WHERE join_date >= :joinDate";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":joinDate", OracleDbType.Date)).Value = DateTime.Now.AddMonths(-6);
                        newJoiners = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching new joiners: {ex.Message}");
            }
            return newJoiners;
        }

        // Method to get total number of departments
        public static int GetTotalDepartments()
        {
            int totalDepartments = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT (DISTINCT DEPARTMENT) FROM  cim_emp_master_list";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalDepartments = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total departments: {ex.Message}");
            }
            return totalDepartments;
        }

        public static List<string> GetDepartments()
        {
            List<string> departments = new List<string>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    // Select distinct department names from cim_emp_master_list
                    string query = "SELECT DISTINCT DEPARTMENT FROM cim_emp_master_list WHERE DEPARTMENT IS NOT NULL";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var departmentName = reader.GetString(reader.GetOrdinal("DEPARTMENT"));
                                Console.WriteLine($"Found Department: {departmentName}");  // Debugging line
                                departments.Add(departmentName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching departments: {ex.Message}");
            }
            return departments;
        }

        public static List<Dictionary<string, object>> GetEmployeesByDepartment(string departmentName, int page, int pageSize)
        {
            List<Dictionary<string, object>> employees = new List<Dictionary<string, object>>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Calculate the starting row for pagination
                    int startRow = (page - 1) * pageSize;

                    // Query for employees in a specific department using department name
                    string query = @"
                            SELECT * 
                            FROM (
                                SELECT emp.*, ROWNUM AS rn 
                                FROM cim_emp_master_list emp 
                                WHERE emp.DEPARTMENT = :departmentName AND emp.STATUS = 'Active'
                            ) 
                            WHERE rn BETWEEN :startRow AND :endRow";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentName", departmentName));
                        command.Parameters.Add(new OracleParameter("startRow", startRow + 1));  // Oracle ROWNUM starts from 1
                        command.Parameters.Add(new OracleParameter("endRow", startRow + pageSize));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader.GetValue(i);
                                    employee.Add(columnName, columnValue);
                                }

                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees: {ex.Message}");
            }
            return employees;
        }

        public static int GetTotalEmployeesInDepartment(string departmentName)
        {
            int totalEmployees = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master_list WHERE DEPARTMENT = :departmentName and STATUS = 'Active' ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentName", departmentName));
                        totalEmployees = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total employees: {ex.Message}");
            }
            return totalEmployees;
        }


        /*-------------------------------------Graph Queries-------------------------------------------------------*/

        //// Method to fetch male and female employee count per year
        //public static List<Dictionary<string, object>> GetMaleFemaleEmployeesByYear()
        //{
        //    List<Dictionary<string, object>> employeesByYear = new List<Dictionary<string, object>>();
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            string query = @"
        //                SELECT EXTRACT(YEAR FROM join_date) AS year, 
        //                SUM(CASE WHEN gender = 'Male' THEN 1 ELSE 0 END) AS male_count,
        //                SUM(CASE WHEN gender = 'Female' THEN 1 ELSE 0 END) AS female_count
        //                FROM cim_emp_master
        //                WHERE STATUS = '1'
        //                GROUP BY EXTRACT(YEAR FROM join_date)
        //                ORDER BY year";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var year = reader.GetInt32(reader.GetOrdinal("year"));
        //                        var maleCount = reader.GetInt32(reader.GetOrdinal("male_count"));
        //                        var femaleCount = reader.GetInt32(reader.GetOrdinal("female_count"));

        //                        employeesByYear.Add(new Dictionary<string, object>
        //                        {
        //                            { "year", year },
        //                            { "male_count", maleCount },
        //                            { "female_count", femaleCount }
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching male/female employee data: {ex.Message}");
        //    }

        //    return employeesByYear;
        //}


        //// Method to fetch male and female employee count per department per year
        //public static List<Dictionary<string, object>> GetMaleFemaleEmployeesByDepartmentPerYear(int startYear, int endYear)
        //{
        //    List<Dictionary<string, object>> maleFemaleEmployeesByDeptYear = new List<Dictionary<string, object>>();
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;
        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            string query = @"
        //                SELECT EXTRACT(YEAR FROM doj) AS year, 
        //                department, 
        //                SUM(CASE WHEN gender = 1 THEN 1 ELSE 0 END) AS male_count,
        //                SUM(CASE WHEN gender = 2 THEN 1 ELSE 0 END) AS female_count
        //                FROM cim_emp_master
        //                WHERE status = '1'
        //                AND EXTRACT(YEAR FROM doj) BETWEEN :startYear AND :endYear
        //                GROUP BY EXTRACT(YEAR FROM doj), department
        //                ORDER BY year, department";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                // Add parameters to prevent SQL injection
        //                command.Parameters.Add(new OracleParameter(":startYear", startYear));
        //                command.Parameters.Add(new OracleParameter(":endYear", endYear));

        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var year = reader.GetInt32(reader.GetOrdinal("year"));
        //                        var department = reader.GetString(reader.GetOrdinal("department"));
        //                        var maleCount = reader.GetInt32(reader.GetOrdinal("male_count"));
        //                        var femaleCount = reader.GetInt32(reader.GetOrdinal("female_count"));

        //                        maleFemaleEmployeesByDeptYear.Add(new Dictionary<string, object>
        //                        {
        //                            { "year", year },
        //                            { "department", department },
        //                            { "male_count", maleCount },
        //                            { "female_count", femaleCount }
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching male/female employee data per department: {ex.Message}");
        //    }
        //    return maleFemaleEmployeesByDeptYear;
        //}

        //// Method to fetch male and female employee count per year
        //public static List<Dictionary<string, object>> GetMaleFemaleEmployeesByYear(int startYear, int endYear)
        //{
        //    List<Dictionary<string, object>> maleFemaleEmployeesByYear = new List<Dictionary<string, object>>();
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;
        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            string query = @"
        //                SELECT EXTRACT(YEAR FROM doj) AS year, 
        //                SUM(CASE WHEN gender = 1 THEN 1 ELSE 0 END) AS male_count,
        //                SUM(CASE WHEN gender = 2 THEN 1 ELSE 0 END) AS female_count
        //                FROM cim_emp_master
        //                WHERE status = '1'
        //                AND EXTRACT(YEAR FROM doj) BETWEEN :startYear AND :endYear
        //                GROUP BY EXTRACT(YEAR FROM doj)
        //                ORDER BY year";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                // Add parameters to prevent SQL injection
        //                command.Parameters.Add(new OracleParameter(":startYear", startYear));
        //                command.Parameters.Add(new OracleParameter(":endYear", endYear));

        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var year = reader.GetInt32(reader.GetOrdinal("year"));
        //                        var maleCount = reader.GetInt32(reader.GetOrdinal("male_count"));
        //                        var femaleCount = reader.GetInt32(reader.GetOrdinal("female_count"));

        //                        maleFemaleEmployeesByYear.Add(new Dictionary<string, object>
        //                {
        //                    { "year", year },
        //                    { "male_count", maleCount },
        //                    { "female_count", femaleCount }
        //                });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching male/female employee data per year: {ex.Message}");
        //    }
        //    return maleFemaleEmployeesByYear;
        //}


        //// Method to fetch employees per department per year with year range
        //public static List<Dictionary<string, object>> GetEmployeesByDepartmentPerYear(int startYear, int endYear)
        //{
        //    List<Dictionary<string, object>> employeesByDeptYear = new List<Dictionary<string, object>>();
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;
        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            string query = @"
        //                SELECT EXTRACT(YEAR FROM doj) AS year, 
        //                department, 
        //                COUNT(*) AS employee_count
        //                FROM cim_emp_master_list
        //                WHERE status = 'Active'
        //                AND EXTRACT(YEAR FROM doj) BETWEEN :startYear AND :endYear
        //                GROUP BY EXTRACT(YEAR FROM doj), department
        //                ORDER BY year, department";

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                // Add parameters to prevent SQL injection
        //                command.Parameters.Add(new OracleParameter(":startYear", startYear));
        //                command.Parameters.Add(new OracleParameter(":endYear", endYear));

        //                using (OracleDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var year = reader.GetInt32(reader.GetOrdinal("year"));
        //                        var department = reader.GetString(reader.GetOrdinal("department"));
        //                        var employeeCount = reader.GetInt32(reader.GetOrdinal("employee_count"));

        //                        employeesByDeptYear.Add(new Dictionary<string, object>
        //                        {
        //                            { "year", year },
        //                            { "department", department },
        //                            { "employee_count", employeeCount }
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching department employee data: {ex.Message}");
        //    }
        //    return employeesByDeptYear;
        //}


        public static List<Dictionary<string, object>> GetEmployeesByDepartmentTotal()
        {
            List<Dictionary<string, object>> employeesByDept = new List<Dictionary<string, object>>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT department, COUNT(*) AS employee_count
                        FROM cim_emp_master_list
                        WHERE status = 'Active'
                        GROUP BY department
                        ORDER BY department
                    "; // Added ordering to ensure consistent results

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var department = reader.GetString(reader.GetOrdinal("department"));
                                var employeeCount = reader.GetInt32(reader.GetOrdinal("employee_count"));

                                employeesByDept.Add(new Dictionary<string, object>
                                {
                                    { "department", department },
                                    { "employee_count", employeeCount }
                                });
                            }
                        }
                    }   
                }

                // Log the full data retrieved
                Console.WriteLine($"Fetched Departments: {employeesByDept.Count}"); // Ensure it's not being truncated
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching department employee data: {ex.Message}");
            }
            return employeesByDept;
        }

        public static List<Dictionary<string, object>> GetMaleFemaleEmployeesTotal()
        {
            List<Dictionary<string, object>> maleFemaleEmployees = new List<Dictionary<string, object>>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    gender, COUNT(*) AS employee_count
                FROM cim_emp_master_list
                WHERE status = 'Active'
                GROUP BY gender
                ORDER BY gender
            "; // Using the 'gender' column to group data

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var gender = reader.GetString(reader.GetOrdinal("gender"));
                                var employeeCount = reader.GetInt32(reader.GetOrdinal("employee_count"));

                                maleFemaleEmployees.Add(new Dictionary<string, object>
                        {
                            { "gender", gender },
                            { "employee_count", employeeCount }
                        });
                            }
                        }
                    }
                }

                // Log the total count of male and female employees
                Console.WriteLine($"Fetched Male/Female Employee Counts: {maleFemaleEmployees.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching male/female employee data: {ex.Message}");
            }

            return maleFemaleEmployees;
        }


    }
}
