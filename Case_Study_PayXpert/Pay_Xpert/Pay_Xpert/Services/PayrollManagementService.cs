using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Repository.Interfaces;
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
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                Console.Write("Enter Pay Period Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.Write("Enter Pay Period End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var payrolls = _payrollService.GeneratePayroll(employeeId, startDate, endDate);
                if (payrolls.Count > 0)
                {
                    Console.WriteLine("Payroll records retrieved successfully:");
                    foreach (var payroll in payrolls)
                    {
                        DisplayPayrollDetails(payroll);
                        Console.WriteLine("========================================");
                    }
                }
                else
                {
                    Console.WriteLine("No payroll records found for the specified employee and date range.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //Console.ReadLine();

        }

        public void GetPayrollByIdMenu()
        {
            try
            {
                Console.Write("Enter Payroll ID: ");
                int payrollId = int.Parse(Console.ReadLine());

                var payroll = _payrollService.GetPayrollById(payrollId);
                if (payroll != null)
                {
                    DisplayPayrollDetails(payroll);
                }
                else
                {
                    Console.WriteLine("No payroll found for the given Payroll ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //Console.ReadLine();
        }

        public void GetPayrollsForEmployeeMenu()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
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
                    Console.WriteLine("No payroll records found for the given Employee ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //Console.ReadLine();
        }

        public void GetPayrollsForPeriodMenu()
        {
            try
            {
                Console.Write("Enter Pay Period Start Date (yyyy-MM-dd): ");
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
                    Console.WriteLine("No payroll records found for the given period.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //Console.ReadLine();
        }

        private void DisplayPayrollDetails(Payroll payroll)
        {
            Console.WriteLine($"Payroll ID: {payroll.PayrollID}");
            Console.WriteLine($"Employee ID: {payroll.EmployeeID}");
            Console.WriteLine($"Pay Period Start Date: {payroll.PayPeriodStartDate:yyyy-MM-dd}");
            Console.WriteLine($"Pay Period End Date: {payroll.PayPeriodEndDate:yyyy-MM-dd}");
            Console.WriteLine($"Basic Salary: {payroll.BasicSalary}");
            Console.WriteLine($"Overtime Pay: {payroll.OvertimePay}");
            Console.WriteLine($"Deductions: {payroll.Deductions}");
            Console.WriteLine($"Net Salary: {payroll.NetSalary}");
        }
    }
}
