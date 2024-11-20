using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Repository.Interfaces;
using Pay_Xpert.Services;
using Pay_Xpert.Utility;
using System.Data.SqlClient;

namespace Pay_Xpert.UI
{
    internal class PayrollUI
    {
        public static void payroll()
        {
          
                PayrollManagementService payrollMenu = new PayrollManagementService();

                bool exit = false;

                while (!exit)
                {
                    
                        Console.Clear();
                        Console.WriteLine("------------------Payroll Management System---------------");
                        Console.WriteLine("1. Generate Payroll");
                        Console.WriteLine("2. Get Payroll By ID");
                        Console.WriteLine("3. Get Payrolls For Employee");
                        Console.WriteLine("4. Get Payrolls For Period");
                        Console.WriteLine("5. Exit");
                        Console.WriteLine("===========================================");
                        Console.Write("Choose an option (1-5): ");
                         int choice = int.Parse(Console.ReadLine());

                    if (choice < 1 || choice > 5)
                        {
                            Console.WriteLine("Invalid choice, please enter a number between 1 and 5.");
                            
                        }

                        switch (choice)
                        {
                            case 1:
                                payrollMenu.GeneratePayrollMenu();
                                break;
                            case 2:
                                payrollMenu.GetPayrollByIdMenu();
                                break;
                            case 3:
                                payrollMenu.GetPayrollsForEmployeeMenu();
                                break;
                            case 4:
                                payrollMenu.GetPayrollsForPeriodMenu();
                                break;
                            case 5:
                                exit = true;
                                Console.WriteLine("Thank you for using PayXpert!");
                                return;
                                break;
                            default:
                                Console.WriteLine("Invalid choice, please try again.");
                                break;
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                  

                
            }
        }
}
