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
                Console.WriteLine("\n\t------------- Financial Reporting --------------");
                Console.WriteLine("\n\n    1. Add Financial Record");
                Console.WriteLine("    2. View Financial Record by ID");
                Console.WriteLine("    3. View All Financial Records for an Employee");
                Console.WriteLine("    4. View Financial Records for a Specific Date");
                Console.WriteLine("    5. Exit");
                Console.WriteLine("\n-----------------------------------------------------------");
                Console.Write("\nEnter your choice: ");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 5)
                {
                    Console.WriteLine(".......Thank You!.......");
                    break;
                }

                switch (choice)
                {
                    case 1:
                        financialRecord.AddFinancialRecord();
                        break;
                    case 2:
                        financialRecord.GetFinancialRecordById();
                        break;
                    case 3:
                        financialRecord.GetFinancialRecordsForEmployee();
                        break;
                    case 4:
                        financialRecord.GetFinancialRecordsForDate();
                        break;
                    case 5:
                        Console.WriteLine("Exiting Financial Record Management. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
