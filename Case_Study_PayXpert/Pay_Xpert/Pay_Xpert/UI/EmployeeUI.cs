using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Services;

namespace Pay_Xpert.UI
{
    internal class EmployeeUI
    {
        public static void employee()
        {
            EmployeeManagementService managementService = new EmployeeManagementService();

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n\t====================================================");
                Console.WriteLine("\t|               Employee Management               |");
                Console.WriteLine("\t====================================================\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t  1. Add Employee");
                Console.WriteLine("\t  2. View All Employees");
                Console.WriteLine("\t  3. Get Employee by ID");
                Console.WriteLine("\t  4. Update Employee");
                Console.WriteLine("\t  5. Remove Employee");
                Console.WriteLine("\t  6. Exit\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\t----------------------------------------------------");
                Console.ResetColor();

                Console.Write("\n\tEnter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid choice! Please enter a number between 1 and 6.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (choice == 6)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nThank you for using Employee Management. Goodbye!");
                    Console.ResetColor();
                    break;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        managementService.AddEmployee();
                        break;
                    case 2:
                        managementService.ViewAllEmployees();
                        break;
                    case 3:
                        managementService.GetEmployeeById();
                        break;
                    case 4:
                        managementService.UpdateEmployee();
                        break;
                    case 5:
                        managementService.RemoveEmployee();
                        break;
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }
}
