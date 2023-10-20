using Object;
using Json;
class Program
{
    private static Client client = new Client();
    static void Main()
    {
        AEEG aeeg = new AEEG();
        RunJson run = new RunJson();
        run.ReadFile(aeeg);
        client.Run(aeeg, run);
        run.WriteFile(aeeg);
    }
}