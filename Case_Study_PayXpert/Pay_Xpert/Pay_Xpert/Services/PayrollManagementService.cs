using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Repository.Interfaces;
using System;
using System.Globalization;

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

                Console.Write("\nEnter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                Console.Write("Enter Pay Period Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.Write("Enter Pay Period End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

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

                Console.Write("\nEnter Payroll ID: ");
                int payrollId = int.Parse(Console.ReadLine());

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

                Console.Write("\nEnter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

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

                Console.Write("\nEnter Pay Period Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.Write("Enter Pay Period End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

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

                Console.Write("\nEnter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                Console.Write("Enter Pay Period Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.Write("Enter Pay Period End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.Write("Enter Basic Salary: ");
                decimal basicSalary = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Overtime Pay: ");
                decimal overtimePay = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Deductions: ");
                decimal deductions = decimal.Parse(Console.ReadLine());

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
