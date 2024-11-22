using System.Data.SqlClient;
using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;
using Pay_Xpert.Exceptions;

namespace Pay_Xpert.Repository.Implementations
{
    internal class TaxService : ITaxService
    {
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

        private void InsertTaxRecord(int employeeId, int taxYear, decimal taxableIncome, decimal taxAmount, SqlConnection connection)
        {
            try
            {
                using (var cmd = new SqlCommand(@"
                    INSERT INTO Tax (EmployeeID, TaxYear, TaxableIncome, TaxAmount)
                    VALUES (@EmployeeID, @TaxYear, @TaxableIncome, @TaxAmount)", connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                    cmd.Parameters.AddWithValue("@TaxYear", taxYear);
                    cmd.Parameters.AddWithValue("@TaxableIncome", taxableIncome);
                    cmd.Parameters.AddWithValue("@TaxAmount", taxAmount);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while inserting tax record: {ex.Message}");
            }
        }

        public Tax GetTaxById(int taxId)
        {
            try
            {
                if (taxId <= 0)
                {
                    throw new InvalidInputException("Invalid Tax ID provided.");
                }

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new SqlCommand(@"
                        SELECT TaxID, EmployeeID, TaxYear, TaxableIncome, TaxAmount
                        FROM Tax
                        WHERE TaxID = @TaxID", connection))
                    {
                        cmd.Parameters.AddWithValue("@TaxID", taxId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Tax
                                {
                                    TaxID = reader.GetInt32(reader.GetOrdinal("TaxID")),
                                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                    TaxYear = reader.GetInt32(reader.GetOrdinal("TaxYear")),
                                    TaxableIncome = reader.GetDecimal(reader.GetOrdinal("TaxableIncome")),
                                    TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount"))
                                };
                            }
                            else
                            {
                                throw new TaxCalculationException($"No tax record found for Tax ID {taxId}.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving tax by ID: {ex.Message}");
            }
        }

        public List<Tax> GetTaxesForEmployee(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    throw new InvalidInputException("Invalid Employee ID provided.");
                }

                var taxes = new List<Tax>();

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new SqlCommand(@"
                        SELECT TaxID, EmployeeID, TaxYear, TaxableIncome, TaxAmount
                        FROM Tax
                        WHERE EmployeeID = @EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new TaxCalculationException($"No tax records found for Employee ID {employeeId}.");
                            }

                            while (reader.Read())
                            {
                                taxes.Add(new Tax
                                {
                                    TaxID = reader.GetInt32(reader.GetOrdinal("TaxID")),
                                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                    TaxYear = reader.GetInt32(reader.GetOrdinal("TaxYear")),
                                    TaxableIncome = reader.GetDecimal(reader.GetOrdinal("TaxableIncome")),
                                    TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount"))
                                });
                            }
                        }
                    }
                }

                return taxes;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving taxes for employee: {ex.Message}");
            }
        }

        public List<Tax> GetTaxesForYear(int taxYear)
        {
            try
            {
                if (taxYear <= 0)
                {
                    throw new InvalidInputException("Invalid tax year provided.");
                }

                var taxes = new List<Tax>();

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new SqlCommand(@"
                        SELECT TaxID, EmployeeID, TaxYear, TaxableIncome, TaxAmount
                        FROM Tax
                        WHERE TaxYear = @TaxYear", connection))
                    {
                        cmd.Parameters.AddWithValue("@TaxYear", taxYear);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new TaxCalculationException($"No tax records found for the tax year {taxYear}.");
                            }

                            while (reader.Read())
                            {
                                taxes.Add(new Tax
                                {
                                    TaxID = reader.GetInt32(reader.GetOrdinal("TaxID")),
                                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                    TaxYear = reader.GetInt32(reader.GetOrdinal("TaxYear")),
                                    TaxableIncome = reader.GetDecimal(reader.GetOrdinal("TaxableIncome")),
                                    TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount"))
                                });
                            }
                        }
                    }
                }

                return taxes;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving taxes for the year: {ex.Message}");
            }
        }
    }
}
