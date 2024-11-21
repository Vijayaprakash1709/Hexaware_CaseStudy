using Pay_Xpert.Models;
using Pay_Xpert.Repository.Implementations;
using Pay_Xpert.Repository.Interfaces;
using Pay_Xpert.Services.Interfaces;
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
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());
                Console.Write("Enter Tax Year: ");
                int taxYear = int.Parse(Console.ReadLine());

                decimal taxAmount = _taxService.CalculateTax(employeeId, taxYear);

                Console.WriteLine($"Calculated Tax for Employee {employeeId} in Year {taxYear}: {taxAmount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating tax: {ex.Message}");
            }
        }

        public void GetTaxDetailsByIdMenu()
        {
            try
            {
                Console.Write("Enter Tax ID: ");
                int taxId = int.Parse(Console.ReadLine());

                var tax = _taxService.GetTaxById(taxId);
                if (tax != null)
                {
                    DisplayTaxDetails(tax);
                }
                else
                {
                    Console.WriteLine("No tax record found for the given Tax ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tax details: {ex.Message}");
            }
        }

        public void GetTaxesForEmployeeMenu()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                var taxes = _taxService.GetTaxesForEmployee(employeeId);
                if (taxes.Count > 0)
                {
                    foreach (var tax in taxes)
                    {
                        DisplayTaxDetails(tax);
                        Console.WriteLine("========================================");
                    }
                }
                else
                {
                    Console.WriteLine("No tax records found for the given Employee ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving taxes for employee: {ex.Message}");
            }
        }

        public void GetTaxesForYearMenu()
        {
            try
            {
                Console.Write("Enter Tax Year: ");
                int taxYear = int.Parse(Console.ReadLine());

                var taxes = _taxService.GetTaxesForYear(taxYear);
                if (taxes.Count > 0)
                {
                    foreach (var tax in taxes)
                    {
                        DisplayTaxDetails(tax);
                        Console.WriteLine("========================================");
                    }
                }
                else
                {
                    Console.WriteLine("No tax records found for the given year.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving taxes for year: {ex.Message}");
            }
        }

        private void DisplayTaxDetails(Tax tax)
        {
            Console.WriteLine($"Tax ID: {tax.TaxID}");
            Console.WriteLine($"Employee ID: {tax.EmployeeID}");
            Console.WriteLine($"Tax Year: {tax.TaxYear}");
            Console.WriteLine($"Taxable Income: {tax.TaxableIncome}");
            Console.WriteLine($"Tax Amount: {tax.TaxAmount}");
        }
    }
}
