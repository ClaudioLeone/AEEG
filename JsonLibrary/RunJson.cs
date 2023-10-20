using Object;

namespace Json
{
    public class RunJson
    {

        public void ReadFile(AEEG aeeg) // Read the file and write in the Dictionaries
        {
            try
            {
            string json = File.ReadAllText("../JsonLibrary/Regions.json");

            string json2 = File.ReadAllText("../JsonLibrary/Contracts.json");

            aeeg.DeserializeFile(json, json2);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la lettura dei file: " + ex.Message);
            }
        }

        public void WriteFile(AEEG aeeg) // Write the file with the Sictionaries
        {
            try
            {
            string[] jsons = aeeg.SerializeFile();
            File.WriteAllText("../JsonLibrary/Regions.json", jsons[0]);

            File.WriteAllText("../JsonLibrary/Contracts.json", jsons[1]);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore durante la scrittura dei file: " + ex.Message);
            }
        }
    }
}