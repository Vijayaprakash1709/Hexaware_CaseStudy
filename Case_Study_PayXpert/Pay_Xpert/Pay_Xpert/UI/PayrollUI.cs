using Pay_Xpert.Services;
using System;

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
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\n\t===============================================");
                Console.WriteLine("\t            Payroll Processing Menu            ");
                Console.WriteLine("\t===============================================\n");
                Console.ResetColor();

                Console.WriteLine("\t1. Generate Payroll");
                Console.WriteLine("\t2. Get Payroll By ID");
                Console.WriteLine("\t3. Get Payrolls For Employee");
                Console.WriteLine("\t4. Get Payrolls For Period");
                Console.WriteLine("\t5. Add Payroll");
                Console.WriteLine("\t6. Exit");

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\t-----------------------------------------------");
                Console.ResetColor();

                Console.Write("\n\tChoose an option (1-6): ");
                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 6)
                {
                    ShowErrorMessage("Invalid input! Please enter a number between 1 and 6.");
                    ContinuePrompt();
                    continue;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- Generate Payroll ---\n");
                        Console.ResetColor();
                        payrollMenu.GeneratePayrollMenu();
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- Get Payroll By ID ---\n");
                        Console.ResetColor();
                        payrollMenu.GetPayrollByIdMenu();
                        break;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- Get Payrolls For Employee ---\n");
                        Console.ResetColor();
                        payrollMenu.GetPayrollsForEmployeeMenu();
                        break;

                    case 4:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- Get Payrolls For Period ---\n");
                        Console.ResetColor();
                        payrollMenu.GetPayrollsForPeriodMenu();
                        break;

                    case 5:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- Add Payroll ---\n");
                        Console.ResetColor();
                        payrollMenu.AddPayrollMenu();
                        break;

                    case 6:
                        ShowSuccessMessage("Thank you for using PayXpert! Goodbye!");
                        exit = true;
                        break;

                    default:
                        ShowErrorMessage("Invalid choice, please try again.");
                        break;
                }

                if (!exit)
                {
                    ContinuePrompt();
                }
            }
        }

        private static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        private static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        private static void ContinuePrompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
