using NUnit.Framework;
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Exceptions;


namespace Pay_Xpert.Tests
{
    public class PayXpertTests
    {
        TestService service = new TestService();

        [TestCase(1, 2024)]
        public void CalculateGrossSalaryForEmployee(int num1, int num2)
        {

            decimal grossSalary = service.GetGrossSalaryOfAnEmployee(num1, num2);
            Assert.That(800000, Is.EqualTo(grossSalary));
        }

        [TestCase(1, 2024)]
        public void CalculateNetSalaryAfterDeductions(int num1, int num2)
        {
            decimal netSalary = service.GetNetSalaryOfAnEmployee(num1, num2);
            Assert.That(799550, Is.EqualTo(netSalary));
        }

        [TestCase(1, 2024)]
        public void VerifyTaxCalculationForHighIncomeEmployee(int num1, int num2)
        {
            decimal tax = service.CalculateTax(num1, num2);
            Assert.That(72410m, Is.EqualTo(tax));
        }

 

        [TestCase(1, "2024-11-01", "2024-11-30")]
        public void ProcessPayrollForMultipleEmployees(int employeeId, string startDate, string endDate)
        {
            PayrollService payrollService = new PayrollService();
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            List<Payroll> totalPayroll = payrollService.GeneratePayroll(employeeId, start, end);
            decimal expectedTotal = 799550m;
            decimal actualTotal = totalPayroll.Sum(p => p.NetSalary);

            Assert.That(actualTotal, Is.EqualTo(expectedTotal));
        }

        [TestCase("2024-11-30", "2024-11-01")]
       
        public void VerifyErrorHandlingForInvalidEmployeeData(string startDate, string endDate)
        {
            PayrollService payrollService = new PayrollService();

            var ex = Assert.Throws<InvalidInputException>(() =>
            {
                DateTime start = !string.IsNullOrWhiteSpace(startDate) ? DateTime.Parse(startDate) : throw new InvalidInputException("Start date is invalid.");
                DateTime end = !string.IsNullOrWhiteSpace(endDate) ? DateTime.Parse(endDate) : throw new InvalidInputException("End date is invalid.");
                payrollService.GetPayrollsForPeriod(start, end);
            });

            Assert.That(ex.Message, Does.Contain("Invalid input"));
        }



    }
}
