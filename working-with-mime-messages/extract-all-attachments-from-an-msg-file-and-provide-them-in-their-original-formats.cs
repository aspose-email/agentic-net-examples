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
            // Define input MSG file path and output directory for attachments
            string msgFilePath = "input.msg";
            string attachmentsFolder = "Attachments";

            // Ensure the output directory exists
            if (!Directory.Exists(attachmentsFolder))
            {
                Directory.CreateDirectory(attachmentsFolder);
            }

            // Guard against missing input file; create a minimal placeholder if absent
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

                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Save(msgFilePath);
                    }
                    Console.WriteLine($"Placeholder MSG created at '{msgFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract attachments
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string fileName = attachment.FileName;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = $"attachment_{Guid.NewGuid()}.dat";
                        }

                        string outputPath = Path.Combine(attachmentsFolder, fileName);
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
