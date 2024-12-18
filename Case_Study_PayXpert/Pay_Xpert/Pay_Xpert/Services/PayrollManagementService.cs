using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Repository.Interfaces;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Pay_Xpert.Services
{
    internal class PayrollManagementService
    {
        private readonly IPayrollService _payrollService;

        public PayrollManagementService()
        {
            _payrollService = new PayrollService();
        }

        public void GeneratePayrollMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("         GENERATE PAYROLL RECORDS       ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                int employeeId = int.Parse(GetValidatedInput("\nEnter Employee ID: ", @"^\d+$", "Invalid Employee ID. It must be a positive integer."));
                DateTime startDate = DateTime.ParseExact(GetValidatedInput("Enter Pay Period Start Date (yyyy-MM-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-MM-dd."), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(GetValidatedInput("Enter Pay Period End Date (yyyy-MM-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-MM-dd."), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var payrolls = _payrollService.GeneratePayroll(employeeId, startDate, endDate);
                if (payrolls.Count > 0)
                {
                    ShowSuccessMessage("Payroll records retrieved successfully:\n");
                    foreach (var payroll in payrolls)
                    {
                        DisplayPayrollDetails(payroll);
                        Console.WriteLine("========================================");
                    }
                }
                else
                {
                    ShowErrorMessage("No payroll records found for the specified employee and date range.");
                }
            }
            catch (PayrollGenerationException pge)
            {
                ShowErrorMessage($"Payroll Generation Error: {pge.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void GetPayrollByIdMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("          VIEW PAYROLL BY ID           ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                int payrollId = int.Parse(GetValidatedInput("\nEnter Payroll ID: ", @"^\d+$", "Invalid Payroll ID. It must be a positive integer."));

                var payroll = _payrollService.GetPayrollById(payrollId);
                if (payroll != null)
                {
                    DisplayPayrollDetails(payroll);
                }
                else
                {
                    ShowErrorMessage("No payroll found for the given Payroll ID.");
                }
            }
            catch (PayrollGenerationException pge)
            {
                ShowErrorMessage($"Payroll Generation Error: {pge.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void GetPayrollsForEmployeeMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("      VIEW PAYROLLS FOR AN EMPLOYEE     ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                int employeeId = int.Parse(GetValidatedInput("\nEnter Employee ID: ", @"^\d+$", "Invalid Employee ID. It must be a positive integer."));

                var payrolls = _payrollService.GetPayrollsForEmployee(employeeId);
                if (payrolls.Count > 0)
                {
                    foreach (var payroll in payrolls)
                    {
                        DisplayPayrollDetails(payroll);
                        Console.WriteLine("========================================");
                    }
                }
                else
                {
                    ShowErrorMessage("No payroll records found for the given Employee ID.");
                }
            }
            catch (PayrollGenerationException pge)
            {
                ShowErrorMessage($"Payroll Generation Error: {pge.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void GetPayrollsForPeriodMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("     VIEW PAYROLLS FOR A DATE RANGE     ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                DateTime startDate = DateTime.ParseExact(GetValidatedInput("\nEnter Pay Period Start Date (yyyy-MM-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-MM-dd."), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(GetValidatedInput("Enter Pay Period End Date (yyyy-MM-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-MM-dd."), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var payrolls = _payrollService.GetPayrollsForPeriod(startDate, endDate);
                if (payrolls.Count > 0)
                {
                    foreach (var payroll in payrolls)
                    {
                        DisplayPayrollDetails(payroll);
                        Console.WriteLine("========================================");
                    }
                }
                else
                {
                    ShowErrorMessage("No payroll records found for the given period.");
                }
            }
            catch (PayrollGenerationException pge)
            {
                ShowErrorMessage($"Payroll Generation Error: {pge.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void AddPayrollMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("          ADD NEW PAYROLL RECORD       ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                int employeeId = int.Parse(GetValidatedInput("\nEnter Employee ID: ", @"^\d+$", "Invalid Employee ID. It must be a positive integer."));
                DateTime startDate = DateTime.ParseExact(GetValidatedInput("Enter Pay Period Start Date (yyyy-MM-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-MM-dd."), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(GetValidatedInput("Enter Pay Period End Date (yyyy-MM-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-MM-dd."), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                decimal basicSalary = decimal.Parse(GetValidatedInput("Enter Basic Salary: ", @"^\d+(\.\d{1,2})?$", "Invalid amount. Use a positive number with up to two decimal places."));
                decimal overtimePay = decimal.Parse(GetValidatedInput("Enter Overtime Pay: ", @"^\d+(\.\d{1,2})?$", "Invalid amount. Use a positive number with up to two decimal places."));
                decimal deductions = decimal.Parse(GetValidatedInput("Enter Deductions: ", @"^\d+(\.\d{1,2})?$", "Invalid amount. Use a positive number with up to two decimal places."));

                var payroll = new Payroll
                {
                    EmployeeID = employeeId,
                    PayPeriodStartDate = startDate,
                    PayPeriodEndDate = endDate,
                    BasicSalary = basicSalary,
                    OvertimePay = overtimePay,
                    Deductions = deductions,
                    NetSalary = 0
                };

                var addedPayroll = _payrollService.AddPayroll(payroll);

                if (addedPayroll != null)
                {
                    ShowSuccessMessage($"Payroll record added successfully! Payroll ID: {addedPayroll.PayrollID}");
                }
                else
                {
                    ShowErrorMessage("Error adding payroll record.");
                }
            }
            catch (PayrollGenerationException pge)
            {
                ShowErrorMessage($"Payroll Generation Error: {pge.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        private string GetValidatedInput(string prompt, string pattern, string errorMessage)
        {
            string input;
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern))
                {
                    break;
                }
                ShowErrorMessage(errorMessage);
            }
            return input;
        }

        private void DisplayPayrollDetails(Payroll payroll)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nPayroll ID: {payroll.PayrollID}");
            Console.WriteLine($"Employee ID: {payroll.EmployeeID}");
            Console.WriteLine($"Pay Period Start Date: {payroll.PayPeriodStartDate:yyyy-MM-dd}");
            Console.WriteLine($"Pay Period End Date: {payroll.PayPeriodEndDate:yyyy-MM-dd}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------");
            Console.ResetColor();

            Console.WriteLine($"Basic Salary: {payroll.BasicSalary:C2}");
            Console.WriteLine($"Overtime Pay: {payroll.OvertimePay:C2}");
            Console.WriteLine($"Deductions: {payroll.Deductions:C2}");
            Console.WriteLine($"Net Salary: {payroll.NetSalary:C2}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------");
            Console.ResetColor();
        }

        private void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
