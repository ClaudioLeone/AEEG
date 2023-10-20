namespace Object
{

    public class Region
    {
        public string? RegionName { get; set; }
        public int ConsumptionE { get; set; }
        public int ConsumptionG { get; set; }
        public byte Strike {get; set;}
        public List<Structure> Structure { get; set; }

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
            else{
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