using Pay_Xpert.Models;
using Pay_Xpert.Repository.Interfaces;
using Pay_Xpert.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Pay_Xpert.Repository.Implementations
{
    internal class PayrollService : IPayrollService
    {
        public List<Payroll> GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
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

                            while (reader.Read())
                            {
                                payrolls.Add(MapPayroll(reader));
                            }

                            return payrolls;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching payroll records: {ex.Message}");
            }
        }

        public Payroll GetPayrollById(int payrollId)
        {
            try
            {
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payroll by ID: {ex.Message}");
            }
            return null;
        }

        public List<Payroll> GetPayrollsForEmployee(int employeeId)
        {
            var payrolls = new List<Payroll>();
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Payroll WHERE EmployeeID = @EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payrolls.Add(MapPayroll(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payrolls for employee: {ex.Message}");
            }
            return payrolls;
        }

        public List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate)
        {
            var payrolls = new List<Payroll>();
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Payroll WHERE PayPeriodStartDate >= @StartDate AND PayPeriodEndDate <= @EndDate", connection))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payrolls.Add(MapPayroll(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payrolls for period: {ex.Message}");
            }
            return payrolls;
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
                throw new Exception($"Error mapping payroll data: {ex.Message}");
            }
        }
    }
}
