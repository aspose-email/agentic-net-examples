using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Directory containing MSG files
            string msgsFolder = "Messages";

            // Verify the directory exists
            if (!Directory.Exists(msgsFolder))
            {
                Console.Error.WriteLine($"Error: Directory not found – {msgsFolder}");
                return;
            }

            // Get all MSG files in the directory
            string[] msgFiles = Directory.GetFiles(msgsFolder, "*.msg");
            if (msgFiles.Length == 0)
            {
                Console.Error.WriteLine($"Error: No MSG files found in – {msgsFolder}");
                return;
            }

            // Set parallel options (optional)
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            // Process each MSG file in parallel
            Parallel.ForEach(msgFiles, parallelOptions, (msgFile) =>
            {
                try
                {
                    // Verify the file exists before loading
                    if (!File.Exists(msgFile))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Error: File not found – {msgFile}");
                        return;
                    }

                    // Load the MSG file; each thread works with its own instance
                    using (MapiMessage message = MapiMessage.Load(msgFile))
                    {
                        // Example processing: output the subject
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Additional thread‑safe processing can be added here
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file {msgFile}: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
