using ProcessMonitor.Interfaces;
using System;

namespace ProcessMonitor
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public bool KeyAvailable() => Console.KeyAvailable;

        public ConsoleKey ReadKey() => Console.ReadKey(true).Key;

        public void WriteLine(string value) => Console.WriteLine(value);
    }
}
