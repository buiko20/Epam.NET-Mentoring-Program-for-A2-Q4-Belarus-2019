using Newtonsoft.Json;

namespace Task1.Console
{
    internal class Program
    {
        private static void Main()
        {
            System.Console.WriteLine($"LastSleepTime: {SystemPowerUtils.GetLastSleepTime()}");
            System.Console.WriteLine($"LastWakeTime: {SystemPowerUtils.GetLastWakeTime()}");
            System.Console.WriteLine($"SystemBatteryState: {JsonConvert.SerializeObject(SystemPowerUtils.GetSystemBatteryState(), Formatting.Indented)}");
            System.Console.WriteLine($"SystemPowerInformation: {JsonConvert.SerializeObject(SystemPowerUtils.GetSystemPowerInformation(), Formatting.Indented)}");

            //SystemPowerUtils.ReserveHibernationFile();
            //System.Console.WriteLine("ReserveHibernationFile");
            //System.Console.ReadKey();
            //System.Console.WriteLine("DeleteHibernationFile");
            //SystemPowerUtils.DeleteHibernationFile();

            // System.Console.WriteLine($"TurnOnSleepMode {SystemPowerUtils.TurnOnSleepMode()}");

            System.Console.WriteLine("Finish");
            System.Console.ReadKey();
        }
    }
}
