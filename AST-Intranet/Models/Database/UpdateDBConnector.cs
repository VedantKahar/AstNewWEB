using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AST_Intranet.Models.Database
{
    public class UpdateDBConnector
    {
        // Method to get employee birthdays and anniversaries
        public static void GetEmployeeBirthdaysAndAnniversaries(out List<string> birthdays, out List<string> anniversaries)
        {
            birthdays = new List<string>();
            anniversaries = new List<string>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    var today = DateTime.Today;

                    string query = @"
                        SELECT EMP_NAME, DOB, DOJ, DEPARTMENT, DESIGNATION, LOCATION 
                        FROM cim_emp_master_list
                        WHERE (TO_CHAR(DOB, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD') 
                        OR TO_CHAR(DOJ, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD'))
                        AND STATUS = 'Active'
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

                                // Handle birthdays
                                if (dob.HasValue && dob.Value.Month == today.Month && dob.Value.Day == today.Day)
                                {
                                    var age = today.Year - dob.Value.Year;
                                    if (today < dob.Value.AddYears(age)) age--; // Adjust age if birthday hasn't occurred yet
                                    birthdays.Add($"{empName} - {age} years old, {department}");
                                }

                                // Handle work anniversaries
                                if (doj.HasValue && doj.Value.Month == today.Month && doj.Value.Day == today.Day)
                                {
                                    var yearsOfService = today.Year - doj.Value.Year;
                                    if (today < doj.Value.AddYears(yearsOfService)) yearsOfService--; // Adjust if anniversary hasn't occurred yet
                                    anniversaries.Add($"{empName} - {yearsOfService} years, {department}, {designation}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error message (could be logged to a file or a logging system)
                Console.WriteLine($"Error fetching birthdays and anniversaries: {ex.Message}");
            }
        }
    }
}
