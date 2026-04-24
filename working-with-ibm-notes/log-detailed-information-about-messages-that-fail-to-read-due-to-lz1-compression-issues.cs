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
            // Directory containing MSG files
            string folderPath = "Messages";

            // Verify the directory exists
            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Directory not found: {folderPath}");
                return;
            }

            string[] msgFiles;
            try
            {
                // Get all .msg files in the directory
                msgFiles = Directory.GetFiles(folderPath, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string filePath in msgFiles)
            {
                // Guard against missing files
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

                    Console.Error.WriteLine($"File not found: {filePath}");
                    continue;
                }

                try
                {
                    // Attempt to load the MSG file
                    using (MapiMessage message = MapiMessage.Load(filePath))
                    {
                        // Log successful load (optional)
                        Console.WriteLine($"Loaded: {Path.GetFileName(filePath)} Subject: {message.Subject}");
                    }
                }
                catch (AsposeException ex)
                {
                    // Detailed logging for compression-related failures
                    Console.Error.WriteLine($"Failed to load {Path.GetFileName(filePath)} due to compression issue.");
                    Console.Error.WriteLine($"Error Message: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.Error.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
                catch (Exception ex)
                {
                    // Log any other unexpected errors
                    Console.Error.WriteLine($"Unexpected error loading {Path.GetFileName(filePath)}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
