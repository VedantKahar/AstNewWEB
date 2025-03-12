using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AST_Intranet.Models.Database
{
    public class DatabaseConnector
    {
        // Method to test the database connection
        public static string TestConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    return "Error: Connection string is not configured properly.";
                }

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open(); // Attempt to open the connection
                    return "Database connected successfully!"; // Connection successful
                }
            }
            catch (OracleException ex)
            {
                // Handle Oracle-specific exceptions (use logging framework here)
                return $"Oracle Database Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Handle other exceptions (use logging framework here)
                return $"Error: {ex.Message}";
            }
        }

        // Method to get data from department_master table
        public static List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open(); // Open connection
                    string query = "SELECT DEPT_ID, DEPT_NAME FROM department_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departments.Add(new Department
                                {
                                    DEPT_ID = reader.GetInt32(reader.GetOrdinal("DEPT_ID")),
                                    DEPT_NAME = reader.GetString(reader.GetOrdinal("DEPT_NAME"))
                                });
                            }
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return departments;
        }

        // Method to get data from cim_emp_master table (fetch only EMP_CODE and EMP_NAME)


        public static List<Dictionary<string, object>> GetEmployees()
        {
            List<Dictionary<string, object>> employees = new List<Dictionary<string, object>>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open(); // Open connection
                    string query = "SELECT * FROM cim_emp_master"; // Select all columns

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++) // Loop through all columns dynamically
                                {
                                    employee.Add(reader.GetName(i), reader.GetValue(i)); // Add column name and value
                                }
                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return employees;
        }



        // Model for Department (consider moving to a separate file)
        public class Department
        {
            public int DEPT_ID { get; set; }
            public string DEPT_NAME { get; set; }
        }
    }
}
