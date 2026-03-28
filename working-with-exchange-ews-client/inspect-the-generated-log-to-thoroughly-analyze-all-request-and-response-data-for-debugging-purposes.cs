using System;
using System.IO;
using System.Collections.Generic;

namespace LogInspector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string logFilePath = "log.txt";

                if (!File.Exists(logFilePath))
                {
                    // Create a minimal placeholder log file
                    using (StreamWriter placeholderWriter = new StreamWriter(logFilePath))
                    {
                        placeholderWriter.WriteLine("Request: Placeholder");
                        placeholderWriter.WriteLine("Response: Placeholder");
                    }
                    Console.Error.WriteLine($"Log file not found. Created placeholder at {logFilePath}.");
                    return;
                }

                List<string> requestLines = new List<string>();
                List<string> responseLines = new List<string>();

                using (StreamReader reader = new StreamReader(logFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Request:", StringComparison.OrdinalIgnoreCase))
                        {
                            requestLines.Add(line);
                        }
                        else if (line.StartsWith("Response:", StringComparison.OrdinalIgnoreCase))
                        {
                            responseLines.Add(line);
                        }
                    }
                }

                Console.WriteLine($"Total request entries: {requestLines.Count}");
                Console.WriteLine($"Total response entries: {responseLines.Count}");

                Console.WriteLine("\nRequests:");
                foreach (string req in requestLines)
                {
                    Console.WriteLine(req);
                }

                Console.WriteLine("\nResponses:");
                foreach (string resp in responseLines)
                {
                    Console.WriteLine(resp);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
