using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the folder containing MSG files.
            string inputFolder = "InputMsgs";

            // Verify the input folder exists.
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Get all MSG files in the folder.
            string[] msgFilePaths;
            try
            {
                msgFilePaths = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                return;
            }

            if (msgFilePaths.Length == 0)
            {
                Console.Error.WriteLine("No MSG files found to process.");
                return;
            }

            // Prepare CPU usage measurement.
            Process currentProcess = Process.GetCurrentProcess();
            TimeSpan cpuStartTime = currentProcess.TotalProcessorTime;
            Stopwatch wallClock = new Stopwatch();
            wallClock.Start();

            // List to hold any processing errors.
            List<string> errorLog = new List<string>();

            // Process each MSG file.
            for (int i = 0; i < msgFilePaths.Length; i++)
            {
                string msgPath = msgFilePaths[i];

                // Ensure the file exists before loading.
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    continue;
                }

                try
                {
                    // Load the MSG file inside a using block to ensure disposal.
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        // Example processing: read subject and body length.
                        string subject = message.Subject;
                        int bodyLength = message.Body != null ? message.Body.Length : 0;

                        // Simulate some work (e.g., logging).
                        Console.WriteLine($"Processed [{i + 1}/{msgFilePaths.Length}]: Subject=\"{subject}\", BodyLength={bodyLength}");
                    }
                }
                catch (Exception ex)
                {
                    string error = $"Error processing file '{msgPath}': {ex.Message}";
                    Console.Error.WriteLine(error);
                    errorLog.Add(error);
                }
            }

            wallClock.Stop();
            TimeSpan cpuEndTime = currentProcess.TotalProcessorTime;
            TimeSpan cpuUsed = cpuEndTime - cpuStartTime;

            // Output profiling results.
            Console.WriteLine();
            Console.WriteLine("Batch processing completed.");
            Console.WriteLine($"Total files processed: {msgFilePaths.Length}");
            Console.WriteLine($"Wall‑clock time: {wallClock.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"CPU time used: {cpuUsed.TotalMilliseconds} ms");

            if (errorLog.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Errors encountered during processing:");
                foreach (string err in errorLog)
                {
                    Console.WriteLine(err);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
