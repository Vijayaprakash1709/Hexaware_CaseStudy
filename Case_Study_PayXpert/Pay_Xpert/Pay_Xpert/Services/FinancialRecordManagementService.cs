﻿using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Services.Interfaces;
using System.Globalization;

namespace Pay_Xpert.Services
{
    internal class FinancialRecordManagementService
    {
        private readonly IFinancialRecordService _financialRecordService;

        public FinancialRecordManagementService()
        {
            _financialRecordService = new FinancialRecordService();
        }

        public void AddFinancialRecord()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Console.Write("Enter Amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Record Type (Income, Expense, Tax Payment, etc.): ");
                string recordType = Console.ReadLine();

                _financialRecordService.AddFinancialRecord(employeeId, description, amount, recordType);
                Console.WriteLine("Financial record added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void GetFinancialRecordById()
        {
            try
            {
                Console.Write("Enter Record ID: ");
                int recordId = int.Parse(Console.ReadLine());

                var record = _financialRecordService.GetFinancialRecordById(recordId);
                if (record != null)
                {
                    DisplayFinancialRecord(record);
                }
                else
                {
                    Console.WriteLine("No financial record found for the given Record ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void GetFinancialRecordsForEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                var records = _financialRecordService.GetFinancialRecordsForEmployee(employeeId);
                if (records.Count > 0)
                {
                    foreach (var record in records)
                    {
                        DisplayFinancialRecord(record);
                        Console.WriteLine("-------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No financial records found for the given Employee ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void GetFinancialRecordsForDate()
        {
            try
            {
                Console.Write("Enter Date (yyyy-MM-dd): ");
                DateTime recordDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var records = _financialRecordService.GetFinancialRecordsForDate(recordDate);
                if (records.Count > 0)
                {
                    foreach (var record in records)
                    {
                        DisplayFinancialRecord(record);
                        Console.WriteLine("-------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No financial records found for the given date.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void DisplayFinancialRecord(FinancialRecord record)
        {
            Console.WriteLine($"Record ID: {record.RecordID}");
            Console.WriteLine($"Employee ID: {record.EmployeeID}");
            Console.WriteLine($"Record Date: {record.RecordDate:yyyy-MM-dd}");
            Console.WriteLine($"Description: {record.Description}");
            Console.WriteLine($"Amount: {record.Amount}");
            Console.WriteLine($"Record Type: {record.RecordType}");
        }
    }
}