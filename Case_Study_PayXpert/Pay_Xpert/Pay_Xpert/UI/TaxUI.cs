using Pay_Xpert.Services;


namespace Pay_Xpert.UI
{
    internal class TaxUI
    {
        
        public static void tax()
            {
            TaxManagementService taxManagementService = new TaxManagementService();
            try
            {
                    while (true)
                    {
                        Console.WriteLine("\n\t--------------- Tax Calculation ---------------");
                        Console.WriteLine("\n\n    1. Calculate Tax for Employee");
                        Console.WriteLine("    2. Get Tax Details by Tax ID");
                        Console.WriteLine("    3. Get Taxes for Employee");
                        Console.WriteLine("    4. Get Taxes for Year");
                        Console.WriteLine("    5. Exit");
                        Console.WriteLine("\n-----------------------------------------------------------");
                        Console.Write("\nEnter your choice: ");

                        int choice = int.Parse(Console.ReadLine());

                        if (choice == 5)
                        {
                            Console.WriteLine(".......Thank You!.......");
                            break;
                        }

                    switch (choice)
                        {
                            case 1:
                                taxManagementService.CalculateTaxForEmployeeMenu();
                                break;
                            case 2:
                                taxManagementService.GetTaxDetailsByIdMenu();
                                break;
                            case 3:
                                taxManagementService.GetTaxesForEmployeeMenu();
                                break;
                            case 4:
                                taxManagementService.GetTaxesForYearMenu();
                                break;
                            case 5:
                                Console.WriteLine("Exiting... Thank you!");
                                return;
                            default:
                                Console.WriteLine("Invalid choice, please try again.");
                                break;
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
