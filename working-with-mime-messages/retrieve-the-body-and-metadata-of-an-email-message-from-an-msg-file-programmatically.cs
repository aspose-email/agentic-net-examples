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
            string msgPath = "sample.msg";

            // Ensure the directory for the MSG file exists
            string msgDir = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!string.IsNullOrEmpty(msgDir) && !Directory.Exists(msgDir))
            {
                Directory.CreateDirectory(msgDir);
            }

            // Guard against missing file
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

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the Outlook MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Print basic metadata
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                // If needed, SenderEmailAddress can be accessed similarly:
                // Console.WriteLine("From Email: " + msg.SenderEmailAddress);

                // Print the message body
                Console.WriteLine("Body:");
                Console.WriteLine(msg.Body);

                // List and save attachments, if any
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine("Attachment: " + attachment.FileName);
                    string attachmentPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FileName);
                    string attachmentDir = Path.GetDirectoryName(attachmentPath);
                    if (!Directory.Exists(attachmentDir))
                    {
                        Directory.CreateDirectory(attachmentDir);
                    }
                    attachment.Save(attachmentPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
