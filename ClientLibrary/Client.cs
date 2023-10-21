using System.Text.RegularExpressions;
using Object;
using Json;
public class Client
{
    public void Run(AEEG aeeg, RunJson json)
    {
        while (true)
        {
            DateTime currentDate = DateTime.Now;
            string Date = currentDate.ToString("dd/MM/yyyy");

            Console.WriteLine("MENU:\n1. Login\n2. Total National Production\n3. Print National Status\n4. Print Auto-Sufficient Regions\n5. Print Lacking Regions\n6. Print Parsimonious Regions\n7. Exit");
            int input;
            try
            {
                input = int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.WriteLine("Warning: {ex.Message}\nInput must be a number! Retry.");
                continue;
            }

            switch (input)
            {
                case 1:
                    List<Region> RegionName = aeeg.Regions();
                    foreach (Region region in RegionName)
                    {
                        Console.WriteLine($"{region.RegionName}");
                    }
                    Console.WriteLine("\nSelect your Region:");
                    string? nameRegion = Console.ReadLine();

                    if (!Regex.IsMatch(nameRegion, @"^[a-zA-Z\s]+$"))
                    {
                        Console.WriteLine("Inexistent Region entered: enter a valid name or check for typos.");
                        break;
                    }

                    Region nameR = aeeg.FindRegion(nameRegion);
                    if (nameR == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid input! Retry.");
                        break;
                    }

                    Console.WriteLine("MENU:\n1. Request Electricity/Gas Supply\n2. Boost Production\n3. Reduce Production\n4. View Your Contracts\n5. View Your Status\n6. Exit");
                    int input2;
                    try
                    {
                        input2 = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.Clear();
                        Console.WriteLine($"Warning: {ex.Message}\nInput must be a number! Retry.Retry.");
                        break;
                    }

                    switch (input2)
                    {
                        case 1:
                            Console.WriteLine("Select Region You Want Request");
                            string? RequestRegion = Console.ReadLine();

                            if (!Regex.IsMatch(RequestRegion, @"^[a-zA-Z\s]+$"))
                            {
                                Console.WriteLine("Inexistent Region entered: enter a valid name or check for typos.");
                                break;
                            }
                            Region RequestR = aeeg.FindRegion(RequestRegion);
                            if (RequestR == null)
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid input! Retry.");
                                break;
                            }

                            Console.WriteLine("For what type of energy do you want to request a supply? 'Electricity' 'Gas'");
                            string? RequestType = Console.ReadLine();

                            if (!Regex.IsMatch(RequestType.ToUpper(), @"^(Electricity|Gas)$", RegexOptions.IgnoreCase))
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid input entered! Input must be one of the two choices shown.");
                                break;
                            }

                            int HowMany;
                            try
                            {
                                Console.WriteLine("How much energy (per day) do you want to request the supply for?");
                                HowMany = int.Parse(Console.ReadLine());
                            }
                            catch (FormatException ex)
                            {
                                Console.Clear();
                                Console.WriteLine($"Warning: {ex.Message}\nInvalid input entered! Quantity for supply must be a number.");
                                break;
                            }

                            Console.WriteLine("Enter an expiration date for the supply contract: ");
                            string? DateEnd = Console.ReadLine();

                            if (!Regex.IsMatch(DateEnd, @"^\d{2}/\d{2}/\d{4}$"))
                            {
                                Console.Clear();
                                Console.WriteLine("Incorrect format entered! Valid date format: dd/mm/yyyy");
                                break;
                            }

                            if (nameR.Strike == 3)
                            {
                                Console.WriteLine("You can't stipulate any contract: you have already reached the maximum limit of requests for supplies.");
                                break;
                            }

                            try
                            {
                                string? result = aeeg.newContract(new Contract(RequestRegion, nameRegion, Date, DateEnd, RequestType, HowMany));
                                Console.WriteLine(result);
                            }
                            catch (Exception ex)
                            {
                                Console.Clear();
                                Console.WriteLine("WARNING! The contract was NOT stipulated due to the following error:" + ex.Message);
                            }
                            break;
                        case 2:
                            Console.WriteLine("What structure do you want to ADD?\n1. Wind Farm\n2. Dam\n3. Power Plant\n4. Gas Distribution Plant");
                            int input3 = int.Parse(Console.ReadLine());
                            Console.WriteLine(nameR.AddStructure(input3));
                            break;
                        case 3:
                            Console.WriteLine("What structure do you want to REMOVE?\n1. Wind Farm\n2. Dam\n3. Power Plant\n4. Gas Distribution Plant");
                            int input4 = int.Parse(Console.ReadLine());
                            Console.WriteLine(nameR.DontWorkStructure(input4));
                            break;
                        case 4:
                            List<Contract> yourContrats = aeeg.YourContract(nameR);
                            foreach (Contract c in yourContrats)
                            {
                                Console.WriteLine(c);
                            }
                            break;
                        case 5:
                            Console.WriteLine(nameR);
                            break;
                        case 6:
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid input entered. Retry.");
                            break;
                    }
                    break;
                case 2:
                    int[] tot = aeeg.ProductionTOT();
                    Console.WriteLine($"Electricity production: {tot[0]} GigaWatts\nGas Production: {tot[1]} CubicMeters");
                    break;
                case 3:
                    List<Region> regions = aeeg.Regions();
                    foreach (Region region in regions)
                    {
                        Console.WriteLine(region);
                    }
                    break;
                case 4:
                    List<Region> AutoSufficient = aeeg.AutoSufficient();
                    foreach (Region region in AutoSufficient)
                    {
                        Console.WriteLine(region);
                    }
                    break;
                case 5:
                    List<Region> NotSatisfying = aeeg.NotSatisfying();
                    break;
                case 6:
                    Region Parsimonious = aeeg.Parsimonious();
                    Console.WriteLine($"The most parsimonious region is {Parsimonious}");
                    break;
                case 7:
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Warning: invalid input entered. Retry.");
                    break;
            }
            json.WriteFile(aeeg);
        }
    }
}