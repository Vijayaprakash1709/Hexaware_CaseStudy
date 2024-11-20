using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;
using System;
using System.Collections.Generic;
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving employee by ID: {ex.Message}");
            }
            return null;
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all employees: {ex.Message}");
            }
            return employees;
        }

        public void AddEmployee(Employee employeeData)
        {
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee: {ex.Message}");
            }
        }

        public void UpdateEmployee(Employee employeeData)
        {
            try
            {
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
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
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
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing employee: {ex.Message}");
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
                Console.WriteLine($"Error mapping employee data: {ex.Message}");
                throw;
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
                Console.WriteLine($"Error adding parameters to SQL command: {ex.Message}");
                throw;
            }
        }
    }
}
