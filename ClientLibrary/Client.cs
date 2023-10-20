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

            Console.WriteLine("MENU:\n1. Login\n2. National Production\n3. Print National Status\n4. Print Auto-Sufficient Regions\n5. Print Not Satisfying Regions\n6. Print Parsimonious Regions\n7. Exit");
            int input;
            try
            {
                input = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Input non valido. Inserire un numero.");
                continue;
            }

            switch (input)
            {
                case 1:
                    foreach (Region region in aeeg.ItalianRegion)
                    {
                        Console.WriteLine($"{region.RegionName}");
                    }
                    Console.WriteLine("select your region");
                    string? nameRegion = Console.ReadLine();

                    if (!Regex.IsMatch(nameRegion, @"^[a-zA-Z\s]+$"))
                    {
                        Console.WriteLine("Nome della regione non valido. Inserire solo lettere e spazi.");
                        break;
                    }

                    Region nameR = aeeg.FindRegion(nameRegion);
                    if(nameR == null){
                        Console.WriteLine("valore inserito non valido");
                        break;
                    }

                    Console.WriteLine("MENU:\n1. Request Electricity/Gas Supply\n2. Boost Production\n3. Reduce Production\n4. View Your Contracts\n5. View Your Status\n6. Exit");
                    int input2;
                    try
                    {
                        input2 = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input non valido. Inserire un numero.");
                        break;
                    }

                    switch (input2)
                    {
                        case 1:
                            Console.WriteLine("Select Region You Want Request");
                            string? RequestRegion = Console.ReadLine();

                            if (!Regex.IsMatch(RequestRegion, @"^[a-zA-Z\s]+$"))
                            {
                                Console.WriteLine("Nome della regione richiesta non valido. Inserire solo lettere e spazi.");
                                break;
                            }
                            Region RequestR = aeeg.FindRegion(RequestRegion);
                            if(RequestR == null){
                                Console.WriteLine("la regione inserita non esiste");
                                break;
                            }

                            Console.WriteLine("What Do You Want Request? 'Electricity' 'Gas'");
                            string? RequestType = Console.ReadLine();

                            if (!Regex.IsMatch(RequestType, @"^(Electricity|Gas)$", RegexOptions.IgnoreCase))
                            {
                                Console.WriteLine("Tipo di richiesta non valido. Inserire 'Electricity' o 'Gas'.");
                                break;
                            }

                            int HowMany;
                            try
                            {
                                Console.WriteLine("How Many Energy You Want Request (GW in Day)");
                                HowMany = int.Parse(Console.ReadLine());
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Valore non valido per l'energia richiesta.");
                                break;
                            }

                            Console.WriteLine("When You Want End Contract?");
                            string? DateEnd = Console.ReadLine();

                            if (!Regex.IsMatch(DateEnd, @"^\d{2}/\d{2}/\d{4}$"))
                            {
                                Console.WriteLine("Data di fine contratto non valida. Formato valido: dd/mm/yyyy");
                                break;
                            }

                            if (nameR.Strike == 3)
                            {
                                Console.WriteLine("You Can't Stipulate A Contract With This Region, You Have Three Strikes");
                                break;
                            }

                            try
                            {
                                string? result = aeeg.newContract(new Contract(RequestRegion, nameRegion, Date, DateEnd, RequestType, HowMany));
                                Console.WriteLine(result);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Errore durante la stipulazione del contratto: " + ex.Message);
                            }
                            break;
                        case 2:
                            Console.WriteLine("What You Want To Add?\n1. Wind\n2.Dam\n3.Electric\n4.Gas Distribution Plant");
                            int input3 = int.Parse(Console.ReadLine());
                            switch (input3)
                            {
                                case 1:
                                    nameR.Structure.Add(new Structure(true, "Wind", 1000));
                                    break;
                                case 2:
                                    nameR.Structure.Add(new Structure(true, "Dam", 400));
                                    break;
                                case 3:
                                    nameR.Structure.Add(new Structure(true, "Electric", 1200));
                                    break;
                                case 4:
                                    nameR.Structure.Add(new Structure(true, "Gas Distribution Plant", 100000));
                                    break;
                                default:
                                    Console.WriteLine("incorrect value");
                                    break;
                            }
                            Console.WriteLine("Structure Added Successfully");
                            break;
                        case 3:
                            Console.WriteLine("What You Want To Remove?\n1. Wind\n2.Dam\n3.Electric\n4.Gas Distribution Plant");
                            int input4 = int.Parse(Console.ReadLine());
                            switch (input4)
                            {
                                case 1:
                                    Structure? wind = nameR.Structure.Find(s => s.Work == true && s.Type == "Wind");
                                    wind.changeWork();
                                    break;
                                case 2:
                                    Structure? dam = nameR.Structure.Find(s => s.Work == true && s.Type == "Dam");
                                    dam.changeWork();
                                    break;
                                case 3:
                                    Structure? electric = nameR.Structure.Find(s => s.Work == true && s.Type == "Electric");
                                    electric.changeWork();
                                    break;
                                case 4:
                                    Structure? Gas = nameR.Structure.Find(s => s.Work == true && s.Type == "Gas Distribuction Plant");
                                    Gas.changeWork();
                                    break;
                                default:
                                    Console.WriteLine("incorrect value");
                                    break;
                            }
                            Console.WriteLine("A Structure Has Been Shut Down Successfully");
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
                            Console.WriteLine("incorrect value");
                            break;
                    }
                    break;
                case 2:
                    int[] tot = aeeg.ProductionTOT();
                    Console.WriteLine($"Energy Production: {tot[0]} GW\nGas Production: {tot[1]} Cm");
                    break;
                case 3:
                    foreach (Region region in aeeg.ItalianRegion)
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
                    Console.WriteLine($"The Most Parsimonious Region Is {Parsimonious}");
                    break;
                case 7:
                    return;
                default:
                    Console.WriteLine("incorrect value");
                    break;
            }
            json.WriteFile(aeeg);
        }
    }
}