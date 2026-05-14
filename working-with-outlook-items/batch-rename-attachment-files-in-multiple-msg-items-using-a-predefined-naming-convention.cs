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
            string inputDirectory = "InputMsgs";
            string outputDirectory = "RenamedAttachments";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists or create it
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Get all MSG files in the input directory
            string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            if (msgFiles.Length == 0)
            {
                Console.WriteLine("No MSG files found in the input directory.");
                return;
            }

            // Process each MSG file
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
                        // Use the message subject (or file name) as part of the new attachment name
                        string subject = message.Subject ?? Path.GetFileNameWithoutExtension(msgFilePath);
                        string safeSubject = SanitizeFileName(subject);

                        int attachmentIndex = 1;
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            string extension = Path.GetExtension(attachment.FileName);
                            string newFileName = $"{safeSubject}_Attachment{attachmentIndex}{extension}";
                            string newFilePath = Path.Combine(outputDirectory, newFileName);

                            try
                            {
                                attachment.Save(newFilePath);
                                Console.WriteLine($"Saved attachment to: {newFilePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                            }

                            attachmentIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to process MSG file '{msgFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to replace invalid filename characters
    private static string SanitizeFileName(string name)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char invalidChar in invalidChars)
        {
            name = name.Replace(invalidChar, '_');
        }
        return name;
    }
}
