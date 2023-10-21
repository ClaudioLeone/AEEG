using System.Security.Principal;

namespace Object
{

    public class Region
    {
        public string? RegionName { get; private set; }
        public int ConsumptionE { get; private set; }
        public int ConsumptionG { get; private set; }
        public byte Strike { get; set; }
        private List<Structure> Structure { get; set; }

        public Region(string regionName, int consumptionE, int consumptionG, byte strike, List<Structure> structure)
        {
            RegionName = regionName;
            ConsumptionE = consumptionE;
            ConsumptionG = consumptionG;
            Strike = strike;
            Structure = structure;
        }

        public int[] Production() // Calculate Region production
        {
            int[] production = new int[2];
            foreach (Structure s in Structure)
                if (s.Type != "Gas Distribution Plant" && s.Work != false)
                {
                    production[0] += s.Production;
                }
                else
                {
                    production[1] += s.Production;
                }
            return production;
        }

        public void AddStructure(int input)
        {
            try
            {
                switch (input)
                {
                    case 1:
                        Structure.Add(new Structure(true, "Wind Farm", 1000));
                        break;
                    case 2:
                        Structure.Add(new Structure(true, "Dam", 400));
                        break;
                    case 3:
                        Structure.Add(new Structure(true, "Power Plant", 1200));
                        break;
                    case 4:
                        Structure.Add(new Structure(true, "Gas Distribution Plant", 100000));
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine(" input number must be one from the menu! Retry.");
                        break;
                }
                Console.Clear();
                Console.WriteLine("Success: the structure has been added!");
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(" input number must be one from the menu! Retry." + ex.Message);
            }
        }
        public void DontWorkStructure(int input)
        {
            switch (input)
            {
                case 1:
                    Structure? wind = Structure.Find(s => s.Work == true && s.Type == "Wind Farm");
                    wind?.changeWork();
                    break;
                case 2:
                    Structure? dam = Structure.Find(s => s.Work == true && s.Type == "Dam");
                    dam?.changeWork();
                    break;
                case 3:
                    Structure? electric = Structure.Find(s => s.Work == true && s.Type == "Power Plant");
                    electric?.changeWork();
                    break;
                case 4:
                    Structure? Gas = Structure.Find(s => s.Work == true && s.Type == "Gas Distribuction Plant");
                    Gas?.changeWork();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine(" invalid input entered. Retry.");
                    break;
            }
            Console.Clear();
            Console.WriteLine("Success: the structure has been removed!");
        }
        public bool ICanDo(List<Contract> contracts, string what, int donation) // Verify if Region can give electricity or gas
        {
            int contractDonation = 0;
            if (what == "Electricity")
            {
                foreach (Contract c in contracts)
                {
                    if (c.WhatToDonate == "Electricity" && c.RegionDonor == this.RegionName)
                    {
                        contractDonation += c.DonationValue;
                    }
                }
                if (Production()[0] - donation - contractDonation >= this.ConsumptionE)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (what == "Gas")
            {
                foreach (Contract c in contracts)
                {
                    if (c.WhatToDonate == "GAS" && c.RegionDonor == this.RegionName)
                    {
                        contractDonation += c.DonationValue;
                    }
                }
                if (Production()[1] - donation - contractDonation >= this.ConsumptionG)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Warning: invalid input entered.");
            }
            return false;
        }

        public override string ToString()
        {
            return $"\nName: {RegionName}\nEnergy Consumption: {ConsumptionE} GW\nEnergy Production: {Production()[0]} GW\nGas Consumption: {ConsumptionG} Cm\nGas Production: {Production()[1]} Cm";
        }
    }
}