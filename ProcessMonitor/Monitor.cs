using ProcessMonitor.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;

namespace ProcessMonitor
{
    public class Monitor
    {
        private readonly IConsoleWrapper _consoleWrapper;

        public Monitor(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public void Start(string processName, double maxLifetime, double monitoringFrequency)
        {
            // Check if the 'q' key is pressed
            while (!_consoleWrapper.KeyAvailable() && _consoleWrapper.ReadKey() != ConsoleKey.Q)
            {
                // Check if the process is running
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes)
                {
                    TimeSpan processLifetime = DateTime.Now - process.StartTime;
                    if (processLifetime.TotalMinutes > maxLifetime)
                    {
                        // Kill the process
                        _consoleWrapper.WriteLine("Killing process: " + process.ProcessName);
                        process.Kill();
                    }
                }

                // Sleep for the monitoring frequency
                Thread.Sleep(Convert.ToInt32(monitoringFrequency * 60000));
            }
        }
    }
}
