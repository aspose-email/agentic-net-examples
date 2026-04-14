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
            // Specify the directory containing MSG files
            string directoryPath = @"C:\MsgFiles";

            // Verify that the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Error: Directory not found – {directoryPath}");
                return;
            }

            // Get all MSG files in the directory
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(directoryPath, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving files: {ex.Message}");
                return;
            }

            int totalFollowUpFlags = 0;

            // Process each MSG file
            foreach (string msgFilePath in msgFiles)
            {
                // Ensure the file still exists before processing
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
                        // Retrieve follow‑up options; if options are present, count as a flagged message
                        FollowUpOptions options = FollowUpManager.GetOptions(message);
                        if (options != null)
                        {
                            totalFollowUpFlags++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                    // Continue with next file
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
