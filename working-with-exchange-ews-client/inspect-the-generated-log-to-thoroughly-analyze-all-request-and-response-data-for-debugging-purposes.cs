using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the log file
            string logFilePath = "log.txt";

            // Verify that the log file exists before attempting to read it
            if (!File.Exists(logFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {logFilePath}");
                return;
            }

            // Open the file stream and read the log contents
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                int lineNumber = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lineNumber++;

                    // Simple analysis: output line number and content
                    Console.WriteLine($"{lineNumber}: {line}");
                }
            }
        }
        catch (Exception exception)
        {
            // Output any unexpected errors
            Console.Error.WriteLine($"Exception: {exception.Message}");
        }
    }
}