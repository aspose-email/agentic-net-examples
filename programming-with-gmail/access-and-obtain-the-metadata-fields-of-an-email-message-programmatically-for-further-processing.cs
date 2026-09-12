using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the Outlook MSG file
            string msgPath = @"c:\outlookmessage.msg";

            // Ensure the directory for the MSG file exists
            string msgDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(msgDir) && !Directory.Exists(msgDir))
            {
                Directory.CreateDirectory(msgDir);
            }

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                // Create a placeholder MSG file if it does not exist
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

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Output basic metadata
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Iterate through attachments, display their names and save them
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine("Attachment Name: " + attachment.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FileName);
                    string saveDir = Path.GetDirectoryName(savePath);
                    if (!string.IsNullOrEmpty(saveDir) && !Directory.Exists(saveDir))
                    {
                        Directory.CreateDirectory(saveDir);
                    }
                    attachment.Save(savePath);
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
