using System;

namespace ProcessMonitor.Interfaces
{
    public interface IConsoleWrapper
    {
        bool KeyAvailable();
        ConsoleKey ReadKey();
        void WriteLine(string value);
    }
}
