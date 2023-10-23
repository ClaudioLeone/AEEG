using Newtonsoft.Json;

namespace Object
{

    public class Contract
    {
        public string? RegionDonor { get; private set; }
        public string? RegionUser { get; private set; }

        [JsonProperty]
        private string? DateStart { get; set; }
        [JsonProperty]
        private string? DateEnd { get; set; }
        public string? WhatToDonate { get; private set; }
        public int DonationValue { get; private set; }

        public Contract(string? regiondonor, string? regionuser, string? dateStart, string? dateEnd, string? whatdonate, int donationvalue)
        {
            RegionDonor = regiondonor;
            RegionUser = regionuser;
            DateStart = dateStart;
            DateEnd = dateEnd;
            WhatToDonate = whatdonate;
            DonationValue = donationvalue;
        }
        public override string ToString()
        {
            return $"'{RegionDonor}' must provide {DonationValue} of '{WhatToDonate}' to '{RegionUser}', from {DateStart} to {DateEnd}";
        }
    }
}