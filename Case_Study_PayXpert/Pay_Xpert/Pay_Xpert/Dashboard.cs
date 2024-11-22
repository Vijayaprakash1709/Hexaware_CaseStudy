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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("\n\t           ================ Pay Xpert ===============            ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("\n\n    1. Employee Management");
                Console.WriteLine("    2. Payroll Processing");
                Console.WriteLine("    3. Tax Calculation");
                Console.WriteLine("    4. Financiaal Reporting");
                Console.WriteLine("    5. Employee Report");
                Console.WriteLine("    6. Exit");
                Console.Write("\nEnter your choice: ");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 6)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(".......Thank You for using PayXpert !.......");
                    break;
                }

                switch (choice)
                {
                    case 1:
                        EmployeeUI.employee();
                        break;
                    case 2:
                        PayrollUI.payroll();
                        break;
                    case 3:
                        TaxUI.tax();
                        break;
                    case 4:
                        FinancialRecordsUI.financialRecords();
                        break;
                    case 5:
                        ReportManagementService.EmployeeReport();
                        break;
                    case 6:
                        Console.WriteLine(".............Exiting....................");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }


    }
}
