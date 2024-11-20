using Pay_Xpert.Models;

namespace Pay_Xpert.Services.Interfaces
{
    internal interface IEmployeeService
    {
        Employee GetEmployeeById(int employeeId);
        List<Employee> GetAllEmployees();
        void AddEmployee(Employee employeeData);
        void UpdateEmployee(Employee employeeData);
        void RemoveEmployee(int employeeId);
    }
}
