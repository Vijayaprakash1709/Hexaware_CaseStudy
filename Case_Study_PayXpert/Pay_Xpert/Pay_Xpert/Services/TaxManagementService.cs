using Pay_Xpert.Exceptions;
using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Repository.Interfaces;
using Pay_Xpert.Services.Interfaces;
using System;
using System.Globalization;

namespace Pay_Xpert.Services
{
    internal class TaxManagementService
    {
        private readonly ITaxService _taxService;

        public TaxManagementService()
        {
            _taxService = new TaxService();
        }

        public void CalculateTaxForEmployeeMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("========================================");
                Console.WriteLine("        CALCULATE TAX FOR EMPLOYEE       ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());
                Console.Write("Enter Tax Year: ");
                int taxYear = int.Parse(Console.ReadLine());

                decimal taxAmount = _taxService.CalculateTax(employeeId, taxYear);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCalculated Tax for Employee {employeeId} in Year {taxYear}: {taxAmount:C2}");
                Console.ResetColor();
            }
            catch (TaxCalculationException tce)
            {
                ShowErrorMessage($"Tax Calculation Error: {tce.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void GetTaxDetailsByIdMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("========================================");
                Console.WriteLine("           VIEW TAX DETAILS BY ID        ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Tax ID: ");
                int taxId = int.Parse(Console.ReadLine());

                var tax = _taxService.GetTaxById(taxId);
                if (tax != null)
                {
                    DisplayTaxDetails(tax);
                }
                else
                {
                    ShowErrorMessage("No tax record found for the given Tax ID.");
                }
            }
            catch (TaxCalculationException tce)
            {
                ShowErrorMessage($"Tax Retrieval Error: {tce.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void GetTaxesForEmployeeMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("========================================");
                Console.WriteLine("       VIEW TAXES FOR AN EMPLOYEE        ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                var taxes = _taxService.GetTaxesForEmployee(employeeId);
                if (taxes.Count > 0)
                {
                    foreach (var tax in taxes)
                    {
                        DisplayTaxDetails(tax);
                        Console.WriteLine("========================================");
                    }
                    ShowSuccessMessage("Taxes retrieved successfully.");
                }
                else
                {
                    ShowErrorMessage("No tax records found for the given Employee ID.");
                }
            }
            catch (TaxCalculationException tce)
            {
                ShowErrorMessage($"Tax Retrieval Error: {tce.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        public void GetTaxesForYearMenu()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("========================================");
                Console.WriteLine("           VIEW TAXES FOR A YEAR         ");
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.Write("\nEnter Tax Year: ");
                int taxYear = int.Parse(Console.ReadLine());

                var taxes = _taxService.GetTaxesForYear(taxYear);
                if (taxes.Count > 0)
                {
                    foreach (var tax in taxes)
                    {
                        DisplayTaxDetails(tax);
                        Console.WriteLine("========================================");
                    }
                    ShowSuccessMessage("Taxes retrieved successfully.");
                }
                else
                {
                    ShowErrorMessage("No tax records found for the given year.");
                }
            }
            catch (TaxCalculationException tce)
            {
                ShowErrorMessage($"Tax Retrieval Error: {tce.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Unexpected Error: {ex.Message}");
            }
        }

        private void DisplayTaxDetails(Tax tax)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nTax ID: {tax.TaxID}");
            Console.WriteLine($"Employee ID: {tax.EmployeeID}");
            Console.WriteLine($"Tax Year: {tax.TaxYear}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------");
            Console.ResetColor();

            Console.WriteLine($"Taxable Income: {tax.TaxableIncome:C2}");
            Console.WriteLine($"Tax Amount: {tax.TaxAmount:C2}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------");
            Console.ResetColor();
        }

        private void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
