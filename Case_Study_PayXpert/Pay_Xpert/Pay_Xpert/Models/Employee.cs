
namespace Pay_Xpert.Models
{
    internal class Employee {
        private int employeeID;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string gender;
        private string email;
        private string phoneNumber;
        private string address;
        private string position;
        private DateTime joiningDate;
        private DateTime? terminationDate;

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                 dateOfBirth = value; 
            }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        public DateTime JoiningDate
        {
            get { return joiningDate; }
            set { joiningDate = value; }
        }

        public DateTime? TerminationDate
        {
            get { return terminationDate; }
            set { terminationDate = value; }
        }

        public int CalculateAge()
        {
            return DateTime.Now.Year - dateOfBirth.Year;
        }
    }
}
