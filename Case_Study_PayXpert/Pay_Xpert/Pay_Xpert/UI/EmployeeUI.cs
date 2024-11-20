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
                Console.WriteLine("-------------------- Employee Management System --------------------");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View All Employees");
                Console.WriteLine("3. Get Employee by ID");
                Console.WriteLine("4. Update Employee");
                Console.WriteLine("5. Remove Employee");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 6)
                {
                    Console.WriteLine(".......Thank You!.......");
                    break;
                }

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
