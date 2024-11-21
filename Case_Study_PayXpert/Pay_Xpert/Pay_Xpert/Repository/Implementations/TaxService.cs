
using System.Data.SqlClient;
using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;

namespace Pay_Xpert.Repository.Implementations
{
    internal class TaxService : ITaxService
    {
        public decimal CalculateTax(int employeeId, int taxYear)
        {
            try
            {
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

                        if (result != null && decimal.TryParse(result.ToString(), out decimal totalNetSalary))
                        {
                            decimal taxAmount = CalculateTaxUsingNewRegime(totalNetSalary);
                            //InsertTaxRecord(employeeId, taxYear, totalNetSalary, taxAmount, connection);
                            return taxAmount;
                        }
                        else
                        {
                            throw new Exception("No payroll data found for the given employee and year.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating tax: {ex.Message}");
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
        public Tax GetTaxById(int taxId)
        {
            try
            {
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
                                throw new Exception("Tax record not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving tax by ID: {ex.Message}");
            }
        }

        public List<Tax> GetTaxesForEmployee(int employeeId)
        {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving taxes for employee: {ex.Message}");
            }
        }

        public List<Tax> GetTaxesForYear(int taxYear)
        {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving taxes for year: {ex.Message}");
            }
        }


    }
}
