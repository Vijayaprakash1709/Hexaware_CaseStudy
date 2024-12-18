using Pay_Xpert.Models;
using Pay_Xpert.Services;

namespace Pay_Xpert.UI
{
    internal class FinancialRecordsUI
    {
        public static void financialRecords()
        {
            FinancialRecordManagementService financialRecord = new FinancialRecordManagementService();

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n\t===============================================");
                Console.WriteLine("\t            Financial Reporting Menu           ");
                Console.WriteLine("\t===============================================\n");
                Console.ResetColor();

                Console.WriteLine("\t1. Add Financial Record");
                Console.WriteLine("\t2. View Financial Record by ID");
                Console.WriteLine("\t3. View All Financial Records for an Employee");
                Console.WriteLine("\t4. View Financial Records for a Specific Date");
                Console.WriteLine("\t5. Exit");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\t-----------------------------------------------");
                Console.ResetColor();

                Console.Write("\n\tEnter your choice (1-5): ");
                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
                {
                    ShowErrorMessage("Invalid input! Please enter a number between 1 and 5.");
                    continue;
                }

                if (choice == 5)
                {
                    ShowSuccessMessage("Exiting Financial Record Management. Goodbye!");
                    break;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- Add Financial Record ---\n");
                        Console.ResetColor();
                        financialRecord.AddFinancialRecord();
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- View Financial Record by ID ---\n");
                        Console.ResetColor();
                        financialRecord.GetFinancialRecordById();
                        break;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- View All Financial Records for an Employee ---\n");
                        Console.ResetColor();
                        financialRecord.GetFinancialRecordsForEmployee();
                        break;

                    case 4:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\t--- View Financial Records for a Specific Date ---\n");
                        Console.ResetColor();
                        financialRecord.GetFinancialRecordsForDate();
                        break;

                    default:
                        ShowErrorMessage("Invalid choice. Please try again.");
                        break;
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ResetColor();
                Console.ReadKey();
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
    }
}
