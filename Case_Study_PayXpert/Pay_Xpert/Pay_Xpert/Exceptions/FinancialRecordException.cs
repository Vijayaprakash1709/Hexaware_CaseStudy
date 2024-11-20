using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay_Xpert.Exceptions
{
    internal class FinancialRecordException:Exception
    {
        public FinancialRecordException(string message) : base(message) { }
    }
}
