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
            string msgPath = "input.msg";
            string outputFolder = "Attachments";

            // Verify input MSG file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                MapiAttachmentCollection attachments = message.Attachments;

                foreach (MapiAttachment attachment in attachments)
                {
                    // Determine a safe file name for the attachment
                    string fileName = attachment.LongFileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = attachment.FileName;
                    }
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "attachment.bin";
                    }

                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        fileName = fileName.Replace(invalidChar, '_');
                    }

                    string outputPath = Path.Combine(outputFolder, fileName);

                    try
                    {
                        // Write attachment bytes to file
                        byte[] data = attachment.BinaryData;
                        if (data != null && data.Length > 0)
                        {
                            File.WriteAllBytes(outputPath, data);
                            Console.WriteLine($"Saved attachment: {outputPath}");
                        }
                        else
                        {
                            Console.Error.WriteLine($"Attachment {fileName} contains no data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {fileName}: {ex.Message}");
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
