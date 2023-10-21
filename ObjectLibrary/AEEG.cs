using System.Runtime.Serialization;
using Newtonsoft.Json;
namespace Object
{
    public class AEEG
    {
        private List<Region> ItalianRegion = new List<Region>();
        private Dictionary<string, List<Contract>> contract = new Dictionary<string, List<Contract>>();

        public int[] ProductionTOT() // The total production
        {
            int[] production = new int[2];
            try
            {
                foreach (Region r in ItalianRegion)
                {
                    production[0] += r.Production()[0];
                    production[1] += r.Production()[1];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when calculating total production: " + ex.Message);
            }
            return production;
        }

        public int[] ConsumptionTOT() // The total consumption
        {
            int[] consumption = new int[2];
            try
            {
                foreach (Region r in ItalianRegion)
                {
                    consumption[0] += r.ConsumptionE;
                    consumption[1] += r.ConsumptionG;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when calculating total consumption: " + ex.Message);
            }
            return consumption;
        }

        public List<Region> NotSatisfying() // Regions with 3 Strike
        {
            List<Region> filter = new List<Region>();
            try
            {
                filter = ItalianRegion.FindAll(r => r.Strike == 3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while searching for lacking Regions: " + ex.Message);
            }
            return filter;
        }

        public List<Region> AutoSufficient() // Regions with production > consumption
        {
            List<Region> filter = new List<Region>();
            try
            {
                filter = ItalianRegion.FindAll(r => r.Production()[0] >= r.ConsumptionE && r.Production()[1] >= r.ConsumptionG);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while searching for auto-sufficient Regions: " + ex.Message);
            }
            return filter;
        }

        public Region Parsimonious() // Region with lesser consumption
        {
            Region region = ItalianRegion[0];
            try
            {
                foreach (Region r in ItalianRegion)
                {
                    if (r.ConsumptionE < region.ConsumptionE && r.ConsumptionG < region.ConsumptionG)
                    {
                        region = r;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while searching for the most parsimonious Region: " + ex.Message);
            }
            return region;
        }

        public string newContract(Contract contr) // Add a new contract
        {
            try
            {
                Region regionUser = FindRegion(contr.RegionUser);
                Region regionDonor = FindRegion(contr.RegionDonor);
                if (contr.WhatToDonate == "Electricity")
                {
                    if (regionUser.Production()[0] - contr.DonationValue <= regionUser.Production()[0] / 2)
                    {
                        regionUser.Strike++;
                    }
                }
                else if (contr.WhatToDonate == "Gas")
                {
                    if (regionUser.Production()[1] - contr.DonationValue <= regionUser.Production()[1] / 2)
                    {
                        regionUser.Strike++;
                    }
                }
                List<Contract>? list = new List<Contract>();
                contract.TryGetValue(regionDonor.RegionName, out list);
                bool CanDo = regionDonor.ICanDo(list, contr.WhatToDonate, contr.DonationValue);
                if (CanDo == true)
                {
                    list.Add(contr);
                    contract.TryGetValue(contr.RegionUser, out list);
                    list.Add(contr);
                    return "The contract has been stipulated successfully";
                }
                else
                {
                    return $"{contr.RegionDonor} refused your request";
                }
            }
            catch (Exception ex)
            {
                return "Error while stipulating the contract: " + ex.Message;
            }

        }

        public Region FindRegion(string nameR) // Return a Region object
        {
            try
            {
                Region Region = ItalianRegion.Find(r => r.RegionName == nameR);
                return Region;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while searching for Region: " + ex.Message);
                return null;
            }
        }

        public List<Contract> YourContract(Region region) // Return a list of contracts between specific Regions
        {
            List<Contract> c = new List<Contract>();
            try
            {
                contract.TryGetValue(region.RegionName, out c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while recovering contracts: " + ex.Message);
            }
            return c;
        }

        public List<Region> Regions() // Return a list of Regions
        {
            List<Region> regions = new List<Region>();
            foreach (Region r in ItalianRegion)
            {
                regions.Add(r);
            }
            return regions;
        }

        public void DeserializeFile(string json, string json2) // Read the file and write in the Dictionaries
        {
            try
            {
            ItalianRegion = JsonConvert.DeserializeObject<List<Region>>(json);

            contract = JsonConvert.DeserializeObject<Dictionary<string, List<Contract>>>(json2);
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("WARNING: an error occurred while writing files: " + ex.Message);
            }
        }

        public string[] SerializeFile() // Write the file with the Sictionaries
        {
            try{
            string[] jsons = new string[2];
            jsons[0] = JsonConvert.SerializeObject(ItalianRegion);

            jsons[1] = JsonConvert.SerializeObject(contract);
            
            return(jsons);
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("WARNING: an error occurred while writing files: " + ex.Message);
                return null;
            }
        }
    }
}