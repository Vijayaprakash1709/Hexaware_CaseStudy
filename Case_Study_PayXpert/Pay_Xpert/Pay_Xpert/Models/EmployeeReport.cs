using Pay_Xpert.Models;
namespace Pay_Xpert.Models
{
    public class EmployeeReport
    {
        private int _employeeID;
        private string _employeeName;
        private List<Payroll> _payrolls = new List<Payroll>();
        private List<Tax> _taxes = new List<Tax>();

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        public List<Payroll> Payrolls
        {
            get { return _payrolls; }
            set { _payrolls = value ?? new List<Payroll>(); }
        }

        public List<Tax> Taxes
        {
            get { return _taxes; }
            set { _taxes = value ?? new List<Tax>(); }
        }
    }
}
