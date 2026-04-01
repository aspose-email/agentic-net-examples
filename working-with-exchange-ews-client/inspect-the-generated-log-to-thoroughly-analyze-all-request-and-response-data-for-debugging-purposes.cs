using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string logFilePath = "log.txt";

            // Ensure the log file exists; create a minimal placeholder if it does not.
            if (!File.Exists(logFilePath))
            {
                try
                {
                    using (FileStream createStream = File.Create(logFilePath))
                    {
                        byte[] placeholder = Encoding.UTF8.GetBytes("Placeholder log content.");
                        createStream.Write(placeholder, 0, placeholder.Length);
                    }
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder log file: {createEx.Message}");
                    return;
                }
            }

            // Read and display the log file contents.
            try
            {
                using (FileStream readStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(readStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Failed to read log file: {readEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
