using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using Pay_Xpert.Utility;

namespace Pay_Xpert.Services
{
    public class ReportGenerator
    {
        public EmployeeReport GenerateEmployeeReport(int employeeId)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                connection.Open();

                var query = @"
                    SELECT e.EmployeeID, e.FirstName + ' ' + e.LastName AS EmployeeName,
                           p.PayPeriodStartDate, p.PayPeriodEndDate, p.NetSalary, 
                           t.TaxYear, t.TaxableIncome, t.TaxAmount
                    FROM Employee e
                    LEFT JOIN Payroll p ON e.EmployeeID = p.EmployeeID
                    LEFT JOIN Tax t ON e.EmployeeID = t.EmployeeID
                    WHERE e.EmployeeID = @EmployeeID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        var employeeReport = new EmployeeReport();
                        bool dataFound = false; // Flag to track if any record exists

                        while (reader.Read())
                        {
                            dataFound = true;

                            if (employeeReport.EmployeeID == 0)
                            {
                                employeeReport.EmployeeID = reader.GetInt32(0);
                                employeeReport.EmployeeName = reader.GetString(1);
                            }

                            if (!reader.IsDBNull(2))
                            {
                                employeeReport.Payrolls.Add(new Payroll
                                {
                                    EmployeeID = reader.GetInt32(0),
                                    PayPeriodStartDate = reader.GetDateTime(2),
                                    PayPeriodEndDate = reader.GetDateTime(3),
                                    NetSalary = reader.GetDecimal(4)
                                });
                            }

                            if (!reader.IsDBNull(5))
                            {
                                employeeReport.Taxes.Add(new Tax
                                {
                                    EmployeeID = reader.GetInt32(0),
                                    TaxYear = reader.GetInt32(5),
                                    TaxableIncome = reader.GetDecimal(6),
                                    TaxAmount = reader.GetDecimal(7)
                                });
                            }
                        }

                        if (!dataFound)
                        {
                            throw new EmployeeNotFoundException($"No employee found with ID {employeeId}");
                        }

                        return employeeReport;
                    }
                }
            }
        }
    }
}
