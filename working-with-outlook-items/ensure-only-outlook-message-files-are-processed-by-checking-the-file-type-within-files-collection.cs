using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace OutlookMessageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the folder containing message files
                string inputFolder = "InputMessages";

                // Verify the folder exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {inputFolder}");
                    return;
                }

                // Retrieve all files in the folder
                string[] allFiles;
                try
                {
                    allFiles = Directory.GetFiles(inputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving files: {ex.Message}");
                    return;
                }

                // Process only Outlook MSG files
                foreach (string filePath in allFiles)
                {
                    // Guard against missing file (unlikely after GetFiles, but for safety)
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

                    // Check if the file is an Outlook MSG format
                    bool isMsg;
                    try
                    {
                        isMsg = MapiMessage.IsMsgFormat(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error checking format for '{filePath}': {ex.Message}");
                        continue;
                    }

                    if (!isMsg)
                    {
                        // Skip non‑MSG files
                        Console.WriteLine($"Skipping non‑MSG file: {Path.GetFileName(filePath)}");
                        continue;
                    }

                    // Load and process the MSG file
                    try
                    {
                        using (MapiMessage msg = MapiMessage.Load(filePath))
                        {
                            Console.WriteLine($"Processed: {msg.Subject}");
                            // Additional processing can be added here
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error loading MSG file '{filePath}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
