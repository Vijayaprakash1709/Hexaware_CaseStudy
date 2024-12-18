
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Interfaces;
using Pay_Xpert.Utility;
using Pay_Xpert.Exceptions;
using System.Data;
using System.Data.SqlClient;

namespace Pay_Xpert.Repository.Implementations
{
    public class PayrollService : IPayrollService
    {
        public List<Payroll> GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (employeeId <= 0 || startDate > endDate)
                {
                    throw new InvalidInputException("Invalid input: Employee ID must be positive, and start date must be earlier than end date.");
                }

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand(@"
                        SELECT *
                        FROM Payroll
                        WHERE EmployeeID = @EmployeeID AND 
                              PayPeriodStartDate >= @StartDate AND 
                              PayPeriodEndDate <= @EndDate
                        ORDER BY PayPeriodStartDate", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            var payrolls = new List<Payroll>();

                            if (!reader.HasRows)
                            {
                                throw new PayrollGenerationException($"No payroll records found for Employee ID {employeeId} within the specified period.");
                            }

                            while (reader.Read())
                            {
                                payrolls.Add(MapPayroll(reader));
                            }

                            return payrolls;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while generating payroll: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new PayrollGenerationException($"Unexpected error generating payroll: {ex.Message}");
            }
        }

        public Payroll GetPayrollById(int payrollId)
        {
            try
            {
                if (payrollId <= 0)
                {
                    throw new InvalidInputException("Invalid payroll ID provided.");
                }

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Payroll WHERE PayrollID = @PayrollID", connection))
                    {
                        cmd.Parameters.AddWithValue("@PayrollID", payrollId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapPayroll(reader);
                            }
                            else
                            {
                                throw new PayrollGenerationException($"No payroll found with ID {payrollId}.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving payroll by ID: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new PayrollGenerationException($"Unexpected error retrieving payroll by ID: {ex.Message}");
            }
        }

        public List<Payroll> GetPayrollsForEmployee(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    throw new InvalidInputException("Invalid Employee ID provided.");
                }

                var payrolls = new List<Payroll>();
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Payroll WHERE EmployeeID = @EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new PayrollGenerationException($"No payroll records found for Employee ID {employeeId}.");
                            }

                            while (reader.Read())
                            {
                                payrolls.Add(MapPayroll(reader));
                            }
                        }
                    }
                }
                return payrolls;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving payrolls for employee: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new PayrollGenerationException($"Unexpected error retrieving payrolls for employee: {ex.Message}");
            }
        }

        public List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    throw new InvalidInputException("Start date must be earlier than or equal to end date.");
                }

                var payrolls = new List<Payroll>();
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Payroll WHERE PayPeriodStartDate >= @StartDate AND PayPeriodEndDate <= @EndDate", connection))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new PayrollGenerationException($"No payroll records found for the period from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}.");
                            }

                            while (reader.Read())
                            {
                                payrolls.Add(MapPayroll(reader));
                            }
                        }
                    }
                }
                return payrolls;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving payrolls for the period: {ex.Message}");
            }
            catch (InvalidInputException ex)
            {
                throw new InvalidInputException($"Invalid input");
            }
            catch (Exception ex)
            {
                throw new PayrollGenerationException($"Unexpected error retrieving payrolls for the period: {ex.Message}");
            }
        }

        public Payroll AddPayroll(Payroll payroll)
        {
            try
            {
                if (payroll.EmployeeID <= 0 || payroll.PayPeriodStartDate > payroll.PayPeriodEndDate || payroll.BasicSalary < 0 || payroll.OvertimePay < 0 || payroll.Deductions < 0)
                {
                    throw new InvalidInputException("Invalid input: Ensure all payroll fields are valid.");
                }

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand(@"
                INSERT INTO Payroll (EmployeeID, PayPeriodStartDate, PayPeriodEndDate, BasicSalary, OvertimePay, Deductions)
                OUTPUT INSERTED.PayrollID
                VALUES (@EmployeeID, @PayPeriodStartDate, @PayPeriodEndDate, @BasicSalary, @OvertimePay, @Deductions)", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", payroll.EmployeeID);
                        cmd.Parameters.AddWithValue("@PayPeriodStartDate", payroll.PayPeriodStartDate);
                        cmd.Parameters.AddWithValue("@PayPeriodEndDate", payroll.PayPeriodEndDate);
                        cmd.Parameters.AddWithValue("@BasicSalary", payroll.BasicSalary);
                        cmd.Parameters.AddWithValue("@OvertimePay", payroll.OvertimePay);
                        cmd.Parameters.AddWithValue("@Deductions", payroll.Deductions);

                        var payrollId = cmd.ExecuteScalar();

                        if (payrollId != null)
                        {
                            payroll.PayrollID = (int)payrollId;
                            return payroll;
                        }
                        else
                        {
                            throw new PayrollGenerationException("Error inserting new payroll record.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while adding payroll: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new PayrollGenerationException($"Unexpected error adding payroll: {ex.Message}");
            }
        }

        private Payroll MapPayroll(SqlDataReader reader)
        {
            try
            {
                return new Payroll
                {
                    PayrollID = (int)reader["PayrollID"],
                    EmployeeID = (int)reader["EmployeeID"],
                    PayPeriodStartDate = (DateTime)reader["PayPeriodStartDate"],
                    PayPeriodEndDate = (DateTime)reader["PayPeriodEndDate"],
                    BasicSalary = (decimal)reader["BasicSalary"],
                    OvertimePay = (decimal)reader["OvertimePay"],
                    Deductions = (decimal)reader["Deductions"],
                    NetSalary = (decimal)reader["NetSalary"]
                };
            }
            catch (Exception ex)
            {
                throw new PayrollGenerationException($"Error mapping payroll data: {ex.Message}");
            }
        }
    }
}
