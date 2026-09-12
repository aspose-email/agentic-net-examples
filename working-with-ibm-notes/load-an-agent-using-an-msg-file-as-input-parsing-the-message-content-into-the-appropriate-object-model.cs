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
            // Path to the MSG file
            string msgPath = @"c:\outlookmessage.msg";

            // Verify the MSG file exists
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

            // Load the Outlook message
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Display basic properties
            Console.WriteLine("Subject: " + msg.Subject);
            Console.WriteLine("From: " + msg.SenderName);
            Console.WriteLine("Body: " + msg.Body);

            // Process attachments if any
            foreach (MapiAttachment att in msg.Attachments)
            {
                Console.WriteLine("Attachment Name: " + att.FileName);

                // Ensure the directory for the attachment exists
                string attachmentPath = Path.Combine(Path.GetDirectoryName(msgPath) ?? "", att.FileName);
                try
                {
                    att.Save(attachmentPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{att.FileName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
