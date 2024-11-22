using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Services.Interfaces;

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

                Console.Write("Enter First Name: ");
                newEmployee.FirstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                newEmployee.LastName = Console.ReadLine();

                Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
                newEmployee.DateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Gender: ");
                newEmployee.Gender = Console.ReadLine();

                Console.Write("Enter Email: ");
                newEmployee.Email = Console.ReadLine();

                Console.Write("Enter Phone Number: ");
                newEmployee.PhoneNumber = Console.ReadLine();

                Console.Write("Enter Address: ");
                newEmployee.Address = Console.ReadLine();

                Console.Write("Enter Position: ");
                newEmployee.Position = Console.ReadLine();

                newEmployee.JoiningDate = DateTime.Now;
                newEmployee.TerminationDate = null;

                _employeeService.AddEmployee(newEmployee);

                ShowSuccessMessage("Employee added successfully.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        public void ViewAllEmployees()
        {
            try
            {
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
                Console.Write("Enter Employee ID: ");
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
                Console.Write("Enter Employee ID to update: ");
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

                Console.Write("Enter First Name (Current: {0}): ", employee.FirstName);
                employee.FirstName = Console.ReadLine();

                Console.Write("Enter Last Name (Current: {0}): ", employee.LastName);
                employee.LastName = Console.ReadLine();

                Console.Write("Enter Date of Birth (Current: {0:yyyy-MM-dd}): ", employee.DateOfBirth);
                employee.DateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Gender (Current: {0}): ", employee.Gender);
                employee.Gender = Console.ReadLine();

                Console.Write("Enter Email (Current: {0}): ", employee.Email);
                employee.Email = Console.ReadLine();

                Console.Write("Enter Phone Number (Current: {0}): ", employee.PhoneNumber);
                employee.PhoneNumber = Console.ReadLine();

                Console.Write("Enter Address (Current: {0}): ", employee.Address);
                employee.Address = Console.ReadLine();

                Console.Write("Enter Position (Current: {0}): ", employee.Position);
                employee.Position = Console.ReadLine();

                Console.Write("Enter Termination Date (yyyy-mm-dd) or leave blank for none: ");
                string terminationDate = Console.ReadLine();
                employee.TerminationDate = string.IsNullOrEmpty(terminationDate) ? null : DateTime.Parse(terminationDate);

                _employeeService.UpdateEmployee(employee);

                ShowSuccessMessage("Employee updated successfully.");
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
                Console.Write("Enter Employee ID to remove: ");
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

                ShowSuccessMessage("Employee removed successfully.");
            }
            catch (EmployeeNotFoundException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Employee Not Found");
            }
        }

        private void DisplayEmployee(Employee employee)
        {
            Console.WriteLine("------------------------------------------------------------");
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
            Console.WriteLine("------------------------------------------------------------");
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
