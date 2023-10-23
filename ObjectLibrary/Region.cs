using Newtonsoft.Json;

namespace Object
{
    public class Region
    {
        public string? RegionName { get; private set; }
        public int ConsumptionE { get; private set; }
        public int ConsumptionG { get; private set; }
        public byte Strike { get; set; }
        [JsonProperty]
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

        public string AddStructure(int input) // add in region private list one structure
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
                        return $" input number must be one from the menu! Retry.";
                }
                return $"Success: the structure has been added!";
            }
            catch (Exception ex)
            {
                return $"Warning: {ex.Message}\nInput number must be one from the menu! Retry.";
            }
        }
        public string DontWorkStructure(int input) // remove frome list one structure because don't work
        {
            try
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
                        return $"Invalid input entered. Retry.";
                }
                return $"Success: the structure has been removed!";
            }
            catch (FormatException ex)
            {
                return $"Warning: {ex.Message}\nInvalid input entered. Retry.";
            }
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