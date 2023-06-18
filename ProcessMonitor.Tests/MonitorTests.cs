using Moq;
using NUnit.Framework;
using ProcessMonitor.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;

namespace ProcessMonitor.Tests
{
    [TestFixture]
    public class MonitorTests
    {
        private readonly Mock<IConsoleWrapper> _consoleWrapperMock;

        private const string ProcessName = "notepad";
        private const double MaxLifetime = 0.02;
        private const double MonitoringFrequency = 0.01;

        public MonitorTests()
        {
            _consoleWrapperMock = new Mock<IConsoleWrapper>();
            _consoleWrapperMock.Setup(x => x.KeyAvailable()).Returns(false);
        }

        [SetUp]
        public void Setup()
        {
            Process.Start(ProcessName);
        }

        [TearDown]
        public void Dispose()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            foreach (Process process in processes)
            {
                // Kill the process
                process.Kill();
            }
        }

        [Test]
        public void Start_WhenQKeyPressed_StopsMonitoring()
        {
            // Arrange
            _consoleWrapperMock.Setup(x => x.KeyAvailable()).Returns(true);
            _consoleWrapperMock.Setup(x => x.ReadKey()).Returns(ConsoleKey.Q);

            var processMonitor = ConstructMonitor();

            // Act
            processMonitor.Start(ProcessName, MaxLifetime, MonitoringFrequency);

            // Assert
            Process[] processes = Process.GetProcessesByName(ProcessName);
            Assert.AreEqual(1, processes.Length, "Process should not be killed");
        }

        [Test]
        public void Start_BeforeExceedingMaxLifetime_ContinuesMonitoring()
        {
            // Arrange
            var processMonitor = ConstructMonitor();

            // Wait for the process to be below the max lifetime
            Thread.Sleep(Convert.ToInt32(MaxLifetime * 0.1 * 60000));

            // Act
            var MonitorThread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                processMonitor.Start(ProcessName, MaxLifetime, MonitoringFrequency);
            });
            MonitorThread.Start();

            // Assert
            Process[] processes = Process.GetProcessesByName(ProcessName);
            Assert.AreEqual(1, processes.Length, "Process should not be killed");
        }

        [Test]
        public void Start_AfterExceedingMaxLifetime_KillsProcess()
        {
            // Arrange
            var processMonitor = ConstructMonitor();
            
            // Wait for the process to exceed the max lifetime
            Thread.Sleep(Convert.ToInt32((MaxLifetime * 1.1) * 60000));

            // Act
            var MonitorThread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                processMonitor.Start(ProcessName, MaxLifetime, MonitoringFrequency);
            });
            MonitorThread.Start();

            // Assert
            // Wait for the monitor to run at least once
            Thread.Sleep(Convert.ToInt32((MonitoringFrequency * 2) * 60000));
            
            Process[] processes = Process.GetProcessesByName(ProcessName);
            Assert.AreEqual(0, processes.Length, "Process should be killed");
        }

        private Monitor ConstructMonitor()
        {
            return new Monitor(_consoleWrapperMock.Object);
        }
    }
}
