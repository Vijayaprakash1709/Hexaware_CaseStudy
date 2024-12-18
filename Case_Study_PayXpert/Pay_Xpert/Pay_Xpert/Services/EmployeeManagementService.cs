using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Services.Interfaces;

//namespace Pay_Xpert.Services
//{
//    internal class EmployeeManagementService
//    {
//        private readonly IEmployeeService _employeeService;

//        public EmployeeManagementService()
//        {
//            _employeeService = new EmployeeService();
//        }

//        public void AddEmployee()
//        {
//            try
//            {
//                var newEmployee = new Employee();

//                Console.ForegroundColor = ConsoleColor.DarkCyan;
//                Console.WriteLine("========================================");
//                Console.WriteLine("             ADD NEW EMPLOYEE           ");
//                Console.WriteLine("========================================");
//                Console.ResetColor();

//                Console.Write("\nEnter First Name: ");
//                newEmployee.FirstName = Console.ReadLine();

//                Console.Write("Enter Last Name: ");
//                newEmployee.LastName = Console.ReadLine();

//                Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
//                newEmployee.DateOfBirth = DateTime.Parse(Console.ReadLine());

//                Console.Write("Enter Gender: ");
//                newEmployee.Gender = Console.ReadLine();

//                Console.Write("Enter Email: ");
//                newEmployee.Email = Console.ReadLine();

//                Console.Write("Enter Phone Number: ");
//                newEmployee.PhoneNumber = Console.ReadLine();

//                Console.Write("Enter Address: ");
//                newEmployee.Address = Console.ReadLine();

//                Console.Write("Enter Position: ");
//                newEmployee.Position = Console.ReadLine();

//                newEmployee.JoiningDate = DateTime.Now;
//                newEmployee.TerminationDate = null;

//                _employeeService.AddEmployee(newEmployee);

//                ShowSuccessMessage("\nEmployee added successfully.");
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Error: {ex.Message}");
//            }
//        }

using System.Text.RegularExpressions;

namespace Pay_Xpert.Services
{
    internal class EmployeeManagementService
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeManagementService()
        {
            _employeeService = new EmployeeService();
        }

        public void AddEmployee()
        {
            try
            {
                var newEmployee = new Employee();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("             ADD NEW EMPLOYEE           ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                newEmployee.FirstName = GetValidatedInput("Enter First Name: ", @"^[A-Za-z]+$", "Invalid name. Only letters are allowed.");
                newEmployee.LastName = GetValidatedInput("Enter Last Name: ", @"^[A-Za-z]+$", "Invalid name. Only letters are allowed.");
                newEmployee.DateOfBirth = DateTime.Parse(GetValidatedInput("Enter Date of Birth (yyyy-mm-dd): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-mm-dd."));
                newEmployee.Gender = GetValidatedInput("Enter Gender (Male/Female/Other): ", @"^(Male|Female|Other)$", "Invalid gender. Please enter Male, Female, or Other.");
                newEmployee.Email = GetValidatedInput("Enter Email: ", @"^[^@\s]+@[^@\s]+\.[^@\s]+$", "Invalid email format.");
                newEmployee.PhoneNumber = GetValidatedInput("Enter Phone Number (10 digits): ", @"^\d{10}$", "Invalid phone number. It must be exactly 10 digits.");
                newEmployee.Address = GetValidatedInput("Enter Address: ", @".+", "Address cannot be empty.");
                newEmployee.Position = GetValidatedInput("Enter Position: ", @".+", "Position cannot be empty.");

                newEmployee.JoiningDate = DateTime.Now;
                newEmployee.TerminationDate = null;

                _employeeService.AddEmployee(newEmployee);

                ShowSuccessMessage("\nEmployee added successfully.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }


        private string GetValidatedInput(string prompt, string pattern, string errorMessage)
        {
            string input;
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern))
                {
                    break;
                }
                ShowErrorMessage(errorMessage);
            }
            return input;
        }
        public void ViewAllEmployees()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("             VIEW ALL EMPLOYEES         ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                List<Employee> employees = _employeeService.GetAllEmployees();
                if (employees.Count == 0)
                {
                    ShowErrorMessage("No employees found.");
                    return;
                }

                foreach (var emp in employees)
                {
                    DisplayEmployee(emp);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        public void GetEmployeeById()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("            GET EMPLOYEE BY ID          ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());
                if (employeeId < 0)
                {
                    ShowErrorMessage("Invalid ID.");
                    return;
                }

                Employee employee = _employeeService.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                }

                DisplayEmployee(employee);
            }
            catch (EmployeeNotFoundException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        public void UpdateEmployee()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("            UPDATE EMPLOYEE DETAILS     ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Employee ID to update: ");
                if (!int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    ShowErrorMessage("Invalid ID.");
                    return;
                }

                Employee employee = _employeeService.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                }

                employee.FirstName = GetValidatedInput($"Enter First Name (Current: {employee.FirstName}): ", @"^[A-Za-z]+$", "Invalid name. Only letters are allowed.");
                employee.LastName = GetValidatedInput($"Enter Last Name (Current: {employee.LastName}): ", @"^[A-Za-z]+$", "Invalid name. Only letters are allowed.");
                employee.DateOfBirth = DateTime.Parse(GetValidatedInput($"Enter Date of Birth (Current: {employee.DateOfBirth:yyyy-MM-dd}): ", @"^\d{4}-\d{2}-\d{2}$", "Invalid date format. Use yyyy-mm-dd."));
                employee.Gender = GetValidatedInput($"Enter Gender (Current: {employee.Gender}): ", @"^(Male|Female|Other)$", "Invalid gender. Please enter Male, Female, or Other.");
                employee.Email = GetValidatedInput($"Enter Email (Current: {employee.Email}): ", @"^[^@\s]+@[^@\s]+\.[^@\s]+$", "Invalid email format.");
                employee.PhoneNumber = GetValidatedInput($"Enter Phone Number (Current: {employee.PhoneNumber}, 10 digits): ", @"^\d{10}$", "Invalid phone number. It must be exactly 10 digits.");
                employee.Address = GetValidatedInput($"Enter Address (Current: {employee.Address}): ", @".+", "Address cannot be empty.");
                employee.Position = GetValidatedInput($"Enter Position (Current: {employee.Position}): ", @".+", "Position cannot be empty.");

                Console.Write("Enter Termination Date (yyyy-mm-dd) or leave blank for none: ");
                string terminationDate = Console.ReadLine();
                employee.TerminationDate = string.IsNullOrEmpty(terminationDate) ? null : DateTime.Parse(terminationDate);

                _employeeService.UpdateEmployee(employee);

                ShowSuccessMessage("\nEmployee updated successfully.");
            }
            catch (EmployeeNotFoundException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }


        public void RemoveEmployee()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("========================================");
                Console.WriteLine("            REMOVE EMPLOYEE             ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Employee ID to remove: ");
                if (!int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    ShowErrorMessage("Invalid ID.");
                    return;
                }

                Employee employee = _employeeService.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                }

                _employeeService.RemoveEmployee(employeeId);

                ShowSuccessMessage("\nEmployee removed successfully.");
            }
            catch (EmployeeNotFoundException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        private void DisplayEmployee(Employee employee)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------------------------------------");
            Console.ResetColor();

            Console.WriteLine($"ID: {employee.EmployeeID}");
            Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
            Console.WriteLine($"DOB: {employee.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Gender: {employee.Gender}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Phone: {employee.PhoneNumber}");
            Console.WriteLine($"Address: {employee.Address}");
            Console.WriteLine($"Position: {employee.Position}");
            Console.WriteLine($"Joining Date: {employee.JoiningDate:yyyy-MM-dd}");
            Console.WriteLine($"Termination Date: {(employee.TerminationDate.HasValue ? employee.TerminationDate.Value.ToString("yyyy-MM-dd") : "N/A")}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------------------------------------");
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
