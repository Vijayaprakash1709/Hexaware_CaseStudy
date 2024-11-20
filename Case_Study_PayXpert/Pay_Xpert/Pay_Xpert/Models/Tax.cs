using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay_Xpert.Models
{
    internal class Tax
    {
        private int taxID;
        private int employeeID;
        private int taxYear;
        private decimal taxableIncome;
        private decimal taxAmount;

        public int TaxID
        {
            get { return taxID; }
            set { taxID = value; }
        }

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public int TaxYear
        {
            get { return taxYear; }
            set { taxYear = value; }
        }

        public decimal TaxableIncome
        {
            get { return taxableIncome; }
            set { taxableIncome = value; }
        }

        public decimal TaxAmount
        {
            get { return taxAmount; }
            set { taxAmount = value; }
        }
    }
}
