using Object;

namespace Json
{
    public class RunJson
    {

        public void ReadFile(AEEG aeeg) // Read and deserialize the file
        {
            try
            {
            string json = File.ReadAllText("../JsonLibrary/Regions.json");

            string json2 = File.ReadAllText("../JsonLibrary/Contracts.json");

            aeeg.DeserializeFile(json, json2);
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred while reading files: " + ex.Message);
            }
        }

        public void WriteFile(AEEG aeeg) // Write and serialize the file
        {
            try
            {
            string[] jsons = aeeg.SerializeFile();
            File.WriteAllText("../JsonLibrary/Regions.json", jsons[0]);

            File.WriteAllText("../JsonLibrary/Contracts.json", jsons[1]);
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred while writing files: " + ex.Message);
            }
        }
    }
}