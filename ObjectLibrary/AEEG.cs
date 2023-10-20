using Newtonsoft.Json;
namespace Object
{
    public class AEEG
    {
        public List<Region> ItalianRegion = new List<Region>();
        private Dictionary<string, List<Contract>> contract = new Dictionary<string, List<Contract>>();

        public int[] ProductionTOT() // the total production
        {
            int[] production = new int[2];
            try
            {
                foreach (Region r in ItalianRegion)
                {
                    foreach (Structure s in r.Structure)
                    {
                        if (s.Type != "Gas Distribution Plant" && s.Work != false)
                        {
                            production[0] += s.Production;
                        }
                        else
                        {
                            production[1] += s.Production;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante il calcolo della produzione totale: " + ex.Message);
            }
            return production;
        }

        public int[] ConsumptionTOT() // the total consumption
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
                Console.WriteLine("Errore durante il calcolo del consumo totale: " + ex.Message);
            }
            return consumption;
        }

        public List<Region> NotSatisfying() // regions with 3 Strike
        {
            List<Region> filter = new List<Region>();
            try
            {
                filter = ItalianRegion.FindAll(r => r.Strike == 3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la ricerca delle regioni non soddisfacenti: " + ex.Message);
            }
            return filter;
        }

        public List<Region> AutoSufficient() // regions with production greater then comsuption
        {
            List<Region> filter = new List<Region>();
            try
            {
                filter = ItalianRegion.FindAll(r => r.Production()[0] >= r.ConsumptionE && r.Production()[1] >= r.ConsumptionG);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la ricerca delle regioni auto-sufficienti: " + ex.Message);
            }
            return filter;
        }

        public Region Parsimonious() // region with the less consumption
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
                Console.WriteLine("Errore durante la ricerca della regione parsimoniosa: " + ex.Message);
            }
            return region;
        }

        public string newContract(Contract contr) //add new contract in section donor and user
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
                    return "the contract is stipulated";
                }
                else
                {
                    return $"{contr.RegionDonor} refused your request";
                }
            }
            catch (Exception ex)
            {
                return "Errore durante la stipulazione del contratto: " + ex.Message;
            }

        }

        public Region FindRegion(string nameR) // With a name return a region object
        {
            try
            {
                Region Region = ItalianRegion.Find(r => r.RegionName == nameR);
                return Region;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la ricerca della regione: " + ex.Message);
                return null;
            }
        }

        public List<Contract> YourContract(Region region) // return list of contract of specific region
        {
            List<Contract> c = new List<Contract>();
            try
            {
                contract.TryGetValue(region.RegionName, out c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante il recupero dei contratti: " + ex.Message);
            }
            return c;
        }

        public void DeserializeFile(string json, string json2) // Read the file and write in the Dictionaries
        {
            try
            {
            ItalianRegion = JsonConvert.DeserializeObject<List<Region>>(json);

            contract = JsonConvert.DeserializeObject<Dictionary<string, List<Contract>>>(json2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
                return null;
            }
        }
    }
}