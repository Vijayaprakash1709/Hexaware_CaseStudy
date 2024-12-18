using Pay_Xpert.Services;

namespace Pay_Xpert.UI
{
    internal class TaxUI
    {
        public static void tax()
        {
            TaxManagementService taxManagementService = new TaxManagementService();
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\n\t===============================================");
                    Console.WriteLine("\t                 Tax Management Menu            ");
                    Console.WriteLine("\t===============================================\n");
                    Console.ResetColor();

                    Console.WriteLine("\t1. Calculate Tax for Employee");
                    Console.WriteLine("\t2. Get Tax Details by Tax ID");
                    Console.WriteLine("\t3. Get Taxes for Employee");
                    Console.WriteLine("\t4. Get Taxes for Year");
                    Console.WriteLine("\t5. Exit");

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\t-----------------------------------------------");
                    Console.ResetColor();

                    Console.Write("\n\tChoose an option (1-5): ");
                    if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 5)
                    {
                        ShowErrorMessage("Invalid input! Please enter a number between 1 and 5.");
                        ContinuePrompt();
                        continue;
                    }

                    Console.Clear();

                    switch (choice)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Calculate Tax for Employee ---\n");
                            Console.ResetColor();
                            taxManagementService.CalculateTaxForEmployeeMenu();
                            break;

                        case 2:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Get Tax Details by Tax ID ---\n");
                            Console.ResetColor();
                            taxManagementService.GetTaxDetailsByIdMenu();
                            break;

                        case 3:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Get Taxes for Employee ---\n");
                            Console.ResetColor();
                            taxManagementService.GetTaxesForEmployeeMenu();
                            break;

                        case 4:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t--- Get Taxes for Year ---\n");
                            Console.ResetColor();
                            taxManagementService.GetTaxesForYearMenu();
                            break;

                        case 5:
                            ShowSuccessMessage("Exiting Tax Management. Thank you!");
                            exit = true;
                            break;

                        default:
                            ShowErrorMessage("Invalid choice, please try again.");
                            break;
                    }

                    if (!exit)
                    {
                        ContinuePrompt();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Error: {ex.Message}");
                    ContinuePrompt();
                }
            }
        }

        private static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        private static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        private static void ContinuePrompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
