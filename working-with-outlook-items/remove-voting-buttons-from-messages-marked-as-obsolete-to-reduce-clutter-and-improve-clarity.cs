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
            string inputFolder = "Messages";
            string outputFolder = "Processed";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Create output folder if it does not exist
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            // Process each .msg file in the input folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    continue;
                }

                try
                {
                    using (MapiMessage msg = MapiMessage.Load(msgPath))
                    {
                        // Check if the message is marked as "Obsolete"
                        if (!string.IsNullOrEmpty(msg.Subject) && msg.Subject.IndexOf("Obsolete", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Remove voting buttons
                            FollowUpManager.ClearVotingButtons(msg);

                            // Save the modified message to the output folder
                            string fileName = Path.GetFileNameWithoutExtension(msgPath);
                            string outputPath = Path.Combine(outputFolder, $"{fileName}_clean.msg");
                            msg.Save(outputPath);
                            Console.WriteLine($"Processed and saved: {outputPath}");
                        }
                        else
                        {
                            Console.WriteLine($"Skipped (not obsolete): {msgPath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
