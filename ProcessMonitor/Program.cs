using System;

namespace ProcessMonitor
{
    class Program
    {
        const string ArgsUsageMsg = "Usage: monitor.exe <process name> <maximum lifetime (minutes)> <monitoring frequency (minutes)>";
        private static readonly ConsoleWrapper _consoleWrapper = new ConsoleWrapper();

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    Console.WriteLine(ArgsUsageMsg);
                    return;
                }

                var processName = args[0];
                var maxLifetime = Convert.ToDouble(args[1]);
                var monitoringFrequency = Convert.ToDouble(args[2]);

                var processMonitor = new Monitor(_consoleWrapper);

                Console.WriteLine("Monitoring process: " + processName);
                Console.WriteLine("Max Lifetime: " + maxLifetime + " minutes");
                Console.WriteLine("Monitoring Frequency: " + monitoringFrequency + " minutes");
                Console.WriteLine("Press 'q' to stop monitoring.");

                processMonitor.Start(processName, maxLifetime, monitoringFrequency);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}