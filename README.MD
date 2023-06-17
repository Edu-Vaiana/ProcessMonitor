# Process Monitor

Process Monitor is a command-line utility written in C# that monitors Windows processes and kills the processes that exceed a specified lifetime threshold.

## Usage

The utility expects three command-line arguments:
- `<process name>`: The name of the process you want to monitor and potentially kill.
- `<maximum lifetime (minutes)>`: The maximum allowed lifetime for the process before it gets terminated.
- `<monitoring frequency (minutes)>`: The frequency at which the utility checks for processes that exceed the maximum lifetime.

Example:

In this example, the program will monitor if the "notepad" process lives longer than 5 minutes every 1 minute.
If it exceeds the maximum lifetime, the program will kill the process.

## Stopping the Monitoring

To stop the monitoring, press the 'q' key on your keyboard.