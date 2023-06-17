using System;
using System.Diagnostics;
using System.Threading;

namespace ProcessMonitor
{
    class Monitor
    {
        const string ArgsUsageMsg = "Usage: monitor.exe <process name> <maximum lifetime (minutes)> <monitoring frequency (minutes)>";

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine(ArgsUsageMsg);
                return;
            }

            string processName = args[0];
            // Todo: accept decimals
            int maxLifetime = Convert.ToInt32(args[1]);
            int monitoringFrequency = Convert.ToInt32(args[2]);

            Console.WriteLine("Monitoring process: " + processName);
            Console.WriteLine("Max Lifetime: " + maxLifetime + " minutes");
            Console.WriteLine("Monitoring Frequency: " + monitoringFrequency + " minutes");
            Console.WriteLine("Press 'q' to stop monitoring.");

            while (true)
            {
                // Check if the 'q' key is pressed
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    break;

                // Check if the process is running
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes)
                {
                    TimeSpan processLifetime = DateTime.Now - process.StartTime;
                    if (processLifetime.TotalMinutes > maxLifetime)
                    {
                        // Kill the process
                        Console.WriteLine("Killing process: " + process.ProcessName);
                        process.Kill();
                    }
                }

                // Sleep for the monitoring frequency
                Thread.Sleep(monitoringFrequency * 60000);
            }
        }
    }
}