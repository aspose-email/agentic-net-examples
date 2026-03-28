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
            Console.WriteLine("Enter the full path to the MSG file:");
            string msgPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(msgPath))
            {
                Console.Error.WriteLine("Error: No path provided.");
                return;
            }

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file safely
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderName} <{msg.SenderEmailAddress}>");
                Console.WriteLine("Body:");
                Console.WriteLine(msg.Body);
                Console.WriteLine();

                // Process attachments
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentFileName = attachment.FileName;
                    // Save each attachment to the current directory
                    attachment.Save(attachmentFileName);
                    Console.WriteLine($"Saved attachment: {attachmentFileName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
