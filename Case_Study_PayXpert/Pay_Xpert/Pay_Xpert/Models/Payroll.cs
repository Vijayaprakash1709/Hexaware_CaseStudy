using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay_Xpert.Models
{
    internal class Payroll
    {
        private int payrollID;
        private int employeeID;
        private DateTime payPeriodStartDate;
        private DateTime payPeriodEndDate;
        private decimal basicSalary;
        private decimal overtimePay;
        private decimal deductions;
        private decimal netSalary;

        public int PayrollID
        {
            get { return payrollID; }
            set { payrollID = value; }
        }

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public DateTime PayPeriodStartDate
        {
            get { return payPeriodStartDate; }
            set { payPeriodStartDate = value; }
        }

        public DateTime PayPeriodEndDate
        {
            get { return payPeriodEndDate; }
            set { payPeriodEndDate = value; }
        }

        public decimal BasicSalary
        {
            get { return basicSalary; }
            set { basicSalary = value; }
        }

        public decimal OvertimePay
        {
            get { return overtimePay; }
            set { overtimePay = value; }
        }

        public decimal Deductions
        {
            get { return deductions; }
            set { deductions = value; }
        }

        public decimal NetSalary
        {
            get { return netSalary; }
            set { netSalary = value; }
        }

    }
}
