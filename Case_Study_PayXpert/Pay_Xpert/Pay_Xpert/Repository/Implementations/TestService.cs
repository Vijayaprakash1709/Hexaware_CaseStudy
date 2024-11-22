using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;
using Pay_Xpert.Exceptions;
using System.Data.SqlClient;

namespace Pay_Xpert.Repository.Implementations
{
    public class TestService
    {
        public decimal GetGrossSalaryOfAnEmployee(int employeeId, int year)
        {
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new SqlCommand(@"
                    SELECT SUM(BasicSalary) AS GrossSalary
                    FROM Payroll
                    WHERE EmployeeID = @EmployeeID AND YEAR(PayPeriodEndDate) = @Year
                    GROUP BY EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@Year", year);

                        object result = cmd.ExecuteScalar();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal grossSalary))
                        {
                            return grossSalary;
                        }
                        else
                        {
                            throw new Exception("No payroll records found for the given employee and year.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching gross salary: {ex.Message}");
                return 0;
            }
        }
        public decimal GetNetSalaryOfAnEmployee(int employeeId, int year)
        {
            try
            {
                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new SqlCommand(@"
                    SELECT SUM(NetSalary) AS NetSalary
                    FROM Payroll
                    WHERE EmployeeID = @EmployeeID AND YEAR(PayPeriodEndDate) = @Year
                    GROUP BY EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@Year", year);

                        object result = cmd.ExecuteScalar();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal grossSalary))
                        {
                            return grossSalary;
                        }
                        else
                        {
                            throw new Exception("No payroll records found for the given employee and year.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching gross salary: {ex.Message}");
                return 0;
            }
        }
        public decimal CalculateTax(int employeeId, int taxYear)
        {
            try
            {
                if (employeeId <= 0 || taxYear <= 0)
                {
                    throw new InvalidInputException("Invalid input: Employee ID and tax year must be positive.");
                }

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new SqlCommand(@"
                        SELECT 
                            SUM(p.NetSalary) AS TotalNetSalary
                        FROM Payroll p
                        WHERE p.EmployeeID = @EmployeeID AND YEAR(p.PayPeriodStartDate) = @TaxYear
                        GROUP BY YEAR(p.PayPeriodStartDate), p.EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@TaxYear", taxYear);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            decimal totalNetSalary = (decimal)result;
                            decimal taxAmount = CalculateTaxUsingNewRegime(totalNetSalary);
                            //InsertTaxRecord(employeeId, taxYear, totalNetSalary, taxAmount, connection);
                            return taxAmount;
                        }

                        else
                        {
                            throw new TaxCalculationException($"No payroll data found for Employee ID {employeeId} and tax year {taxYear}.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while calculating tax: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new TaxCalculationException($"Unexpected error calculating tax: {ex.Message}");
            }
        }
        private decimal CalculateTaxUsingNewRegime(decimal totalNetSalary)
        {
            decimal tax = 0;

            if (totalNetSalary <= 250000)
            {
                tax = 0;
            }
            else if (totalNetSalary <= 500000)
            {
                tax = (totalNetSalary - 250000) * 0.05m;
            }
            else if (totalNetSalary <= 1000000)
            {
                tax = (totalNetSalary - 500000) * 0.2m + 12500;
            }
            else
            {
                tax = (totalNetSalary - 1000000) * 0.3m + 12500 + 100000;
            }

            return tax;
        }
    }
}
