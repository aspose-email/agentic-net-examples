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
            // Define input and output directories
            string inputFolder = "Messages";
            string outputFolder = "ProcessedMessages";

            // Ensure the input directory exists; if not, create a placeholder directory and exit gracefully
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder \"{inputFolder}\" does not exist.");
                return;
            }

            // Ensure the output directory exists; create it if necessary
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder \"{outputFolder}\": {ex.Message}");
                    return;
                }
            }

            // Get all MSG files in the input folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                return;
            }

            // Process each MSG file
            foreach (string msgFilePath in msgFiles)
            {
                // Guard against missing files (should not happen after GetFiles, but defensive)
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

                    Console.Error.WriteLine($"File not found: {msgFilePath}");
                    continue;
                }

                try
                {
                    // Load the MSG message
                    using (MapiMessage message = MapiMessage.Load(msgFilePath))
                    {
                        // Determine the message date (use ClientSubmitTime if available)
                        DateTime messageDate = message.ClientSubmitTime;
                        if (messageDate == DateTime.MinValue)
                        {
                            // Fallback to DeliveryTime if ClientSubmitTime is not set
                            messageDate = message.DeliveryTime;
                        }

                        // Check if the message is older than 30 days
                        if (messageDate < DateTime.Now.AddDays(-30))
                        {
                            // Property tag for PR_VOTING_OPTIONS (Unicode string)
                            // Tag format: (propertyId << 16) | propertyType
                            // propertyId = 0x0E03, propertyType = 0x001F (PT_UNICODE)
                            long votingOptionsTag = 0x0E03001F;

                            // Remove the voting options property if it exists
                            message.RemoveProperty(votingOptionsTag);

                            // Save the modified message to the output folder
                            string outputPath = Path.Combine(outputFolder, Path.GetFileName(msgFilePath));
                            message.Save(outputPath);
                            Console.WriteLine($"Processed and saved: {outputPath}");
                        }
                        else
                        {
                            // Message is recent; copy without modification
                            string outputPath = Path.Combine(outputFolder, Path.GetFileName(msgFilePath));
                            message.Save(outputPath);
                            Console.WriteLine($"Copied without changes: {outputPath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file \"{msgFilePath}\": {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
