using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - console UI for processing MSG files
class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Enter the full path to the MSG file:");
            string inputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                Console.Error.WriteLine("No file path was entered.");
                return;
            }

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            // Load the Outlook MSG file
            MapiMessage msg = MapiMessage.Load(inputPath);

            Console.WriteLine($"Subject: {msg.Subject}");
            Console.WriteLine($"From: {msg.SenderName}");
            Console.WriteLine($"Body: {msg.Body}");

            // Process each attachment
            foreach (MapiAttachment attachment in msg.Attachments)
            {
                Console.WriteLine($"Attachment: {attachment.FileName}");

                // Save attachment to the same folder as the MSG file
                string folder = Path.GetDirectoryName(inputPath) ?? string.Empty;
                string attachmentPath = Path.Combine(folder, attachment.FileName);

                try
                {
                    attachment.Save(attachmentPath);
                    Console.WriteLine($"Saved attachment to: {attachmentPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
