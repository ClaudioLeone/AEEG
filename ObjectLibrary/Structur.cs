namespace Object
{
    public class Structure
    {
        public bool Work { get; set; }
        public string Type { get; set; }
        public int Production { get; set; }
        public Structure(bool work, string type, int production)
        {
            Work = work;
            Type = type;
            Production = production;
        }

        public void changeWork() // change the structure status
        {
            this.Work = !this.Work;
        }
    }
}