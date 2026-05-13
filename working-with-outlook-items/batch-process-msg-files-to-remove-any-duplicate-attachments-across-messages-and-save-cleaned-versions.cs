using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = @"C:\InputMsg";
            string outputFolder = @"C:\CleanedMsg";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists or create it
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                return;
            }

            // Track attachment file names that have already been seen
            HashSet<string> seenAttachmentNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        List<MapiAttachment> attachmentsToRemove = new List<MapiAttachment>();

                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            // Use attachment file name as a simple duplicate identifier
                            if (seenAttachmentNames.Contains(attachment.FileName))
                            {
                                attachmentsToRemove.Add(attachment);
                            }
                            else
                            {
                                seenAttachmentNames.Add(attachment.FileName);
                            }
                        }

                        // Remove duplicate attachments
                        foreach (MapiAttachment dup in attachmentsToRemove)
                        {
                            message.Attachments.Remove(dup);
                        }

                        string outputPath = Path.Combine(outputFolder, Path.GetFileName(msgPath));
                        message.Save(outputPath);
                        Console.WriteLine($"Processed and saved: {outputPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
