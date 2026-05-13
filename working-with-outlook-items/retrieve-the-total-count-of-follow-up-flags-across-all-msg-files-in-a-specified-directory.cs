using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailFollowUpCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Directory that contains the MSG files.
                string messagesDirectory = "Messages";

                // Ensure the directory exists; if not, create it and add a placeholder MSG file.
                if (!Directory.Exists(messagesDirectory))
                {
                    Directory.CreateDirectory(messagesDirectory);
                    string placeholderPath = Path.Combine(messagesDirectory, "placeholder.msg");
                    try
                    {
                        using (MapiMessage placeholderMessage = new MapiMessage(
                            "from@example.com",
                            "to@example.com",
                            "Placeholder Subject",
                            "Placeholder body"))
                        {
                            placeholderMessage.Save(placeholderPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                        return;
                    }
                }

                // Get all MSG files in the directory.
                string[] msgFiles;
                try
                {
                    msgFiles = Directory.GetFiles(messagesDirectory, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing files in directory '{messagesDirectory}': {ex.Message}");
                    return;
                }

                int totalFollowUpFlags = 0;

                foreach (string filePath in msgFiles)
                {
                    // Guard each file operation with its own try/catch.
                    try
                    {
                        using (MapiMessage message = MapiMessage.Load(filePath))
                        {
                            FollowUpOptions options = FollowUpManager.GetOptions(message);
                            if (options != null && !string.IsNullOrEmpty(options.FlagRequest))
                            {
                                totalFollowUpFlags++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process file '{filePath}': {ex.Message}");
                        // Continue with next file.
                    }
                }

                Console.WriteLine($"Total follow‑up flags across all MSG files: {totalFollowUpFlags}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
