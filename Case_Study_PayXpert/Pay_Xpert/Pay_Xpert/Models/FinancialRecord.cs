
namespace Pay_Xpert.Models
{
    internal class FinancialRecord
    {
        private int recordID;
        private int employeeID;
        private DateTime recordDate;
        private string description;
        private decimal amount;
        private string recordType;

        public int RecordID
        {
            get { return recordID; }
            set { recordID = value; }
        }

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public DateTime RecordDate
        {
            get { return recordDate; }
            set { recordDate = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string RecordType
        {
            get { return recordType; }
            set { recordType = value; }
        }
    }
}
