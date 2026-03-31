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
            string msgPath = "input.msg";
            string outputDirectory = "Attachments";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
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

                MapiMessage placeholderMessage = new MapiMessage(
                    "sender@example.com",
                    "receiver@example.com",
                    "Placeholder Subject",
                    "This is a placeholder message."
                );
                placeholderMessage.Save(msgPath);
                Console.WriteLine($"Placeholder MSG created at: {msgPath}");
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Retrieve attachment data as a byte array
                    byte[] attachmentData = attachment.BinaryData;
                    if (attachmentData == null || attachmentData.Length == 0)
                    {
                        Console.WriteLine($"Attachment '{attachment.FileName}' contains no data.");
                        continue;
                    }

                    // Determine a safe file name
                    string safeFileName = Path.GetFileName(attachment.FileName);
                    if (string.IsNullOrEmpty(safeFileName))
                    {
                        safeFileName = "attachment.bin";
                    }

                    string outputPath = Path.Combine(outputDirectory, safeFileName);

                    try
                    {
                        // Write the attachment data to a file using streams
                        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            using (MemoryStream memoryStream = new MemoryStream(attachmentData))
                            {
                                memoryStream.CopyTo(fileStream);
                            }
                        }
                        Console.WriteLine($"Saved attachment to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
