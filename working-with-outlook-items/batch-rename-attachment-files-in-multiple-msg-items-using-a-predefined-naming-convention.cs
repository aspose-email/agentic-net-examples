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
            string inputFolder = @"C:\MsgFiles";
            string outputFolder = @"C:\RenamedAttachments";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputFolder}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Could not create output directory – {ex.Message}");
                    return;
                }
            }

            string[] msgFiles = Directory.GetFiles(inputFolder, "*.msg");
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

                    Console.Error.WriteLine($"Warning: MSG file not found – {msgPath}");
                    continue;
                }

                try
                {
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : MakeFileSystemSafe(message.Subject);
                        int attachmentIndex = 1;

                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            string originalFileName = attachment.FileName;
                            string extension = Path.GetExtension(originalFileName);
                            string newFileName = $"{safeSubject}_Attachment{attachmentIndex}{extension}";
                            string newFilePath = Path.Combine(outputFolder, newFileName);

                            // Save the attachment with the new file name
                            attachment.Save(newFilePath);
                            Console.WriteLine($"Saved attachment as: {newFileName}");

                            attachmentIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgPath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to replace invalid filename characters
    private static string MakeFileSystemSafe(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
