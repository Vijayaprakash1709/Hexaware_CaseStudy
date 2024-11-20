using Pay_Xpert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay_Xpert.Services.Interfaces
{
    internal interface IFinancialRecordService
    {
        void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType);
        FinancialRecord GetFinancialRecordById(int recordId);
        List<FinancialRecord> GetFinancialRecordsForEmployee(int employeeId);
        List<FinancialRecord> GetFinancialRecordsForDate(DateTime recordDate);
    }
}
