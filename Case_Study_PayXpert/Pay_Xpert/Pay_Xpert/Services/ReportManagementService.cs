using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using System;

namespace Pay_Xpert.Services
{
    internal class ReportManagementService
    {
        public static void EmployeeReport()
        {
            ReportGenerator reportGenerator = new ReportGenerator();

            Console.Clear();
            Console.WriteLine("****************************************");
            Console.WriteLine("***** Employee Report Generator *****");
            Console.WriteLine("****************************************");

            Console.Write("Enter Employee ID: ");
            int employeeId;
            if (!int.TryParse(Console.ReadLine(), out employeeId))
            {
                ShowErrorMessage("Invalid Employee ID.");
                return;
            }

            try
            {
                var employeeReport = reportGenerator.GenerateEmployeeReport(employeeId);

                if (employeeReport == null)
                {
                    ShowErrorMessage("No report found for this employee.");
                    return;
                }

                ShowSuccessMessage("Report generated successfully!");
                DisplayEmployeeReport(employeeReport);
            }
            catch (EmployeeNotFoundException ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static void DisplayEmployeeReport(EmployeeReport report)
        {
            Console.Clear();
            Console.WriteLine("****************************************");
            Console.WriteLine("***** Employee Report *****");
            Console.WriteLine("****************************************");
            Console.WriteLine($"Employee ID: {report.EmployeeID}");
            Console.WriteLine($"Employee Name: {report.EmployeeName}");
            Console.WriteLine("****************************************");

            Console.WriteLine("***** Payroll Information *****");
            Console.WriteLine("****************************************");
            if (report.Payrolls.Count == 0)
            {
                Console.WriteLine("No payroll data available.");
            }
            else
            {
                foreach (var payroll in report.Payrolls)
                {
                    Console.WriteLine($"Pay Period: {payroll.PayPeriodStartDate:yyyy-MM-dd} - {payroll.PayPeriodEndDate:yyyy-MM-dd}");
                    Console.WriteLine($"Net Salary: {payroll.NetSalary:C2}");
                    Console.WriteLine("----------------------------------------");
                }
            }
            Console.WriteLine("****************************************");

            Console.WriteLine("***** Tax Information *****");
            Console.WriteLine("****************************************");
            if (report.Taxes.Count == 0)
            {
                Console.WriteLine("No tax data available.");
            }
            else
            {
                foreach (var tax in report.Taxes)
                {
                    Console.WriteLine($"Tax Year: {tax.TaxYear}");
                    Console.WriteLine($"Taxable Income: {tax.TaxableIncome:C2}");
                    Console.WriteLine($"Tax Amount: {tax.TaxAmount:C2}");
                    Console.WriteLine("----------------------------------------");
                }
            }

            Console.WriteLine("****************************************");
        }

        private static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
