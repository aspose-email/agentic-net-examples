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
            string inputFolder = "InputMsgs";
            string outputFolder = "OutputMsgs";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure output folder exists or create it
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
                    return;
                }
            }

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

            foreach (string msgFilePath in msgFiles)
            {
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
                    using (MapiMessage message = MapiMessage.Load(msgFilePath))
                    {
                        // Define uniform flag request and dates
                        string flagRequest = "Follow up";
                        DateTime startDate = DateTime.Now;
                        DateTime dueDate = startDate.AddDays(7);

                        // Set the follow‑up flag with start and due dates
                        FollowUpManager.SetFlag(message, flagRequest, startDate, dueDate);

                        // Save the updated message to the output folder
                        string outputPath = Path.Combine(outputFolder, Path.GetFileName(msgFilePath));
                        message.Save(outputPath);
                        Console.WriteLine($"Processed: {msgFilePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
