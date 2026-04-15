using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string directoryPath = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();

            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Error: Directory not found – {directoryPath}");
                return;
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(directoryPath, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing files in directory: {ex.Message}");
                return;
            }

            foreach (string filePath in msgFiles)
            {
                if (!File.Exists(filePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    // Skip missing files gracefully
                    continue;
                }

                try
                {
                    using (MapiMessage message = MapiMessage.Load(filePath))
                    {
                        // Check if the MSG file represents a task (IPM.Task)
                        if (!string.IsNullOrEmpty(message.MessageClass) &&
                            message.MessageClass.StartsWith("IPM.Task", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Task Subject: {message.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
