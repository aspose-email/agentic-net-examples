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
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <program> <msgFilePath> <outputFolder>");
                return;
            }

            string msgFilePath = args[0];
            string outputFolderPath = args[1];

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            if (!Directory.Exists(outputFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            using (MapiMessageReader reader = new MapiMessageReader(msgFilePath))
            {
                MapiAttachmentCollection attachments = reader.ReadAttachments();
                int attachmentIndex = 0;
                foreach (MapiAttachment attachment in attachments)
                {
                    string fileExtension = Path.GetExtension(attachment.FileName ?? ".dat");
                    string outputFilePath = Path.Combine(outputFolderPath, $"attachment_{attachmentIndex}{fileExtension}");

                    try
                    {
                        // Save the attachment content to a file.
                        attachment.SaveToTnef(outputFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {attachmentIndex}: {ex.Message}");
                    }

                    attachmentIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
