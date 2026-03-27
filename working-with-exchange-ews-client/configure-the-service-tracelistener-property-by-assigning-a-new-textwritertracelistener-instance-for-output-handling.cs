using System;
using System.IO;
using System.Diagnostics;

namespace AsposeEmailTraceListenerExample
{
    // Simple service class with a TraceListener property
    public class Service
    {
        public TextWriterTraceListener TraceListener { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Define the log file path
                string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
                string logFilePath = Path.Combine(logDirectory, "service_trace.log");

                // Ensure the directory exists
                if (!Directory.Exists(logDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                        return;
                    }
                }

                // Ensure the file exists or create a minimal placeholder
                if (!File.Exists(logFilePath))
                {
                    try
                    {
                        using (FileStream fs = File.Create(logFilePath))
                        {
                            // Create an empty file as placeholder
                        }
                    }
                    catch (Exception fileEx)
                    {
                        Console.Error.WriteLine($"Failed to create log file: {fileEx.Message}");
                        return;
                    }
                }

                // Initialize the service and assign a TextWriterTraceListener
                Service service = new Service();

                try
                {
                    using (TextWriterTraceListener listener = new TextWriterTraceListener(logFilePath))
                    {
                        service.TraceListener = listener;

                        // Example usage: write a trace message
                        listener.WriteLine("Service trace started at " + DateTime.Now);
                        listener.Flush();

                        // Additional service logic would go here
                    }
                }
                catch (Exception listenerEx)
                {
                    Console.Error.WriteLine($"Failed to configure TraceListener: {listenerEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}