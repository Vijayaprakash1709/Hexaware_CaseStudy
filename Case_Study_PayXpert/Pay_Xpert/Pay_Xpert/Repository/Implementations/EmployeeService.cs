using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;
using Pay_Xpert.Exceptions;
using System.Data.SqlClient;

namespace Pay_Xpert.Repository.Implementations
{
    internal class EmployeeService : IEmployeeService
    {
        public Employee GetEmployeeById(int employeeId)
        {
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapEmployee(reader);
                            }
                            else
                            {
                                throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"An error occurred while connecting to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new EmployeeNotFoundException($"An unexpected error occurred while retrieving the employee: {ex.Message}");
            }
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Employee";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapEmployee(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"An error occurred while connecting to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"An unexpected error occurred while retrieving all employees: {ex.Message}");
            }

            return employees;
        }

        public void AddEmployee(Employee employeeData)
        {
            try
            {
                ValidateEmployeeData(employeeData);

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Employee
                        (FirstName, LastName, DateOfBirth, Gender, Email, PhoneNumber, Address, Position, JoiningDate, TerminationDate) 
                        VALUES 
                        (@FirstName, @LastName, @DateOfBirth, @Gender, @Email, @PhoneNumber, @Address, @Position, @JoiningDate, @TerminationDate)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        AddEmployeeParameters(command, employeeData);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (InvalidInputException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"An error occurred while connecting to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"An unexpected error occurred while adding the employee: {ex.Message}");
            }
        }

        public void UpdateEmployee(Employee employeeData)
        {
            try
            {
                ValidateEmployeeData(employeeData);

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        UPDATE Employee
                        SET 
                            FirstName = @FirstName, 
                            LastName = @LastName, 
                            DateOfBirth = @DateOfBirth, 
                            Gender = @Gender, 
                            Email = @Email, 
                            PhoneNumber = @PhoneNumber, 
                            Address = @Address, 
                            Position = @Position, 
                            JoiningDate = @JoiningDate, 
                            TerminationDate = @TerminationDate
                        WHERE 
                            EmployeeId = @EmployeeId";
                    using (var command = new SqlCommand(query, connection))
                    {
                        AddEmployeeParameters(command, employeeData);
                        command.Parameters.AddWithValue("@EmployeeId", employeeData.EmployeeID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new EmployeeNotFoundException($"Employee with ID {employeeData.EmployeeID} not found.");
                        }
                    }
                }
            }
            catch (InvalidInputException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"An error occurred while connecting to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"An unexpected error occurred while updating the employee: {ex.Message}");
            }
        }

        public void RemoveEmployee(int employeeId)
        {
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"An error occurred while connecting to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"An unexpected error occurred while removing the employee: {ex.Message}");
            }
        }

        private Employee MapEmployee(SqlDataReader reader)
        {
            try
            {
                return new Employee
                {
                    EmployeeID = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                    Gender = reader["Gender"].ToString(),
                    Email = reader["Email"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    Address = reader["Address"].ToString(),
                    Position = reader["Position"].ToString(),
                    JoiningDate = (DateTime)reader["JoiningDate"],
                    TerminationDate = reader["TerminationDate"] == DBNull.Value ? null : (DateTime?)reader["TerminationDate"]
                };
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"An error occurred while mapping employee data: {ex.Message}");
            }
        }

        private void AddEmployeeParameters(SqlCommand command, Employee employeeData)
        {
            try
            {
                command.Parameters.AddWithValue("@FirstName", employeeData.FirstName);
                command.Parameters.AddWithValue("@LastName", employeeData.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employeeData.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", employeeData.Gender);
                command.Parameters.AddWithValue("@Email", employeeData.Email);
                command.Parameters.AddWithValue("@PhoneNumber", employeeData.PhoneNumber);
                command.Parameters.AddWithValue("@Address", employeeData.Address);
                command.Parameters.AddWithValue("@Position", employeeData.Position);
                command.Parameters.AddWithValue("@JoiningDate", employeeData.JoiningDate);
                command.Parameters.AddWithValue("@TerminationDate", employeeData.TerminationDate ?? (object)DBNull.Value);
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"An error occurred while adding parameters to the SQL command: {ex.Message}");
            }
        }

        private void ValidateEmployeeData(Employee employeeData)
        {
            if (string.IsNullOrWhiteSpace(employeeData.FirstName) || string.IsNullOrWhiteSpace(employeeData.LastName))
            {
                throw new InvalidInputException("First name and last name are required.");
            }

            if (employeeData.DateOfBirth > DateTime.Now)
            {
                throw new InvalidInputException("Date of birth cannot be in the future.");
            }

            if (string.IsNullOrWhiteSpace(employeeData.Email) || !employeeData.Email.Contains("@"))
            {
                throw new InvalidInputException("A valid email address is required.");
            }
        }
    }
}
