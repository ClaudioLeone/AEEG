namespace Object
{

    public class Contract
    {
        public string? RegionDonor { get; set; }
        public string? RegionUser { get; set; }
        public string? DateStart { get; set; }
        public string? DateEnd { get; set; }
        public string? WhatToDonate { get; set; }
        public int DonationValue { get; set; }

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