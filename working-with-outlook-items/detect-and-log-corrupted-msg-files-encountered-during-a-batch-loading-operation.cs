using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Directory containing MSG files to process
            string folderPath = "MsgFiles";

            // Verify that the directory exists
            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Error: Directory not found – {folderPath}");
                return;
            }

            // Get all *.msg files in the directory
            string[] msgFiles = Directory.GetFiles(folderPath, "*.msg");

            foreach (string filePath in msgFiles)
            {
                // Ensure the file exists before attempting to load
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

                    Console.Error.WriteLine($"Warning: File not found – {filePath}");
                    continue;
                }

                try
                {
                    // Attempt to load the MSG file
                    using (MapiMessage message = MapiMessage.Load(filePath))
                    {
                        // Successfully loaded – log basic information
                        Console.WriteLine($"Loaded: {Path.GetFileName(filePath)} – Subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    // Log corrupted or unreadable MSG files
                    Console.Error.WriteLine($"Corrupted MSG file: {Path.GetFileName(filePath)} – {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
