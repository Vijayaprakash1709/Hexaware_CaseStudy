using Pay_Xpert.Models;


namespace Pay_Xpert.Repository.Interfaces
{
    public interface IPayrollService
    {
        List<Payroll> GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate);
        Payroll GetPayrollById(int payrollId);
        List<Payroll> GetPayrollsForEmployee(int employeeId);
        List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate);
    }
}
