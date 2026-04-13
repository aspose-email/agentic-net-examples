using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "InputMsgs";
            string outputDirectory = "ArchivedMsgs";

            // Ensure input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputDirectory}");
                return;
            }

            // Ensure output directory exists or create it
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory – {ex.Message}");
                return;
            }

            // Get all MSG files in the input directory
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating files – {ex.Message}");
                return;
            }

            // Process each MSG file
            foreach (string msgFilePath in msgFiles)
            {
                // Guard against missing file (should not happen after GetFiles, but safe)
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Warning: File not found – {msgFilePath}");
                    continue;
                }

                try
                {
                    using (MapiMessage message = MapiMessage.Load(msgFilePath))
                    {
                        // Determine the received date (prefer ClientSubmitTime, fallback to DeliveryTime)
                        DateTime receivedDateTime = message.ClientSubmitTime != DateTime.MinValue
                            ? message.ClientSubmitTime
                            : message.DeliveryTime != DateTime.MinValue
                                ? message.DeliveryTime
                                : DateTime.Now;

                        DateTime receivedDate = receivedDateTime.Date;

                        // Build the target subfolder path based on the received date (yyyy-MM-dd)
                        string dateFolderName = receivedDate.ToString("yyyy-MM-dd");
                        string targetFolderPath = Path.Combine(outputDirectory, dateFolderName);

                        // Ensure the date folder exists
                        try
                        {
                            if (!Directory.Exists(targetFolderPath))
                            {
                                Directory.CreateDirectory(targetFolderPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating date folder '{targetFolderPath}' – {ex.Message}");
                            continue;
                        }

                        // Save the message into the date folder, preserving original file name
                        string targetFilePath = Path.Combine(targetFolderPath, Path.GetFileName(msgFilePath));
                        try
                        {
                            message.Save(targetFilePath);
                            Console.WriteLine($"Archived: {msgFilePath} -> {targetFilePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving message '{msgFilePath}' – {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}' – {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
