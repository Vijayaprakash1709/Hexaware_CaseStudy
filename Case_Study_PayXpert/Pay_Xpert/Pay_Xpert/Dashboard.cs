using Pay_Xpert.UI;
using Pay_Xpert.Models;
using Pay_Xpert.Services;

namespace Pay_Xpert
{
    internal class Dashboard
    {
        public void dashboard()
        {
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                //Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("\n\t'''''''''''''''''''''''''''''''''''''''''''''");
                Console.WriteLine("\t|              Welcome to Pay-Xpert         |");
                Console.WriteLine("\t'''''''''''''''''''''''''''''''''''''''''''''\n");
                Console.ResetColor();
                //Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t\t[1] Employee Management");
                Console.WriteLine("\t\t[2] Payroll Processing");
                Console.WriteLine("\t\t[3] Tax Calculation");
                Console.WriteLine("\t\t[4] Financial Reporting");
                Console.WriteLine("\t\t[5] Employee Report");
                Console.WriteLine("\t\t[6] Exit");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\t--------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("\n\t\tEnter your choice (1-6): ");

                int choice;
                bool isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\t   Invalid choice! Please enter a number between 1 and 6.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Employee Management ---\n");
                            Console.ResetColor();
                            EmployeeUI.employee();
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Payroll Processing ---\n");
                            Console.ResetColor();
                            PayrollUI.payroll();
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Tax Calculation ---\n");
                            Console.ResetColor();
                            TaxUI.tax();
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Financial Reporting ---\n");
                            Console.ResetColor();
                            FinancialRecordsUI.financialRecords();
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Employee Report ---\n");
                            Console.ResetColor();
                            ReportManagementService.EmployeeReport();
                            break;
                        case 6:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\t    ........... Thank You for using PayXpert! ...........");
                            break;
                    }
                }

                if (choice == 6)
                {
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\t---------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\t\t\t   Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
