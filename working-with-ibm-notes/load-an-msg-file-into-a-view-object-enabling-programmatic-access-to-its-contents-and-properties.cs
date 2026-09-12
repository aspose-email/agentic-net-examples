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
            // Author note: This sample demonstrates loading an Outlook MSG file and accessing its properties.
            string msgFilePath = "sample.msg";

            // Guard file existence
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

                Console.Error.WriteLine($"Error: MSG file not found at '{msgFilePath}'.");
                return;
            }

            // Load the MSG file into a MapiMessage object
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Access basic properties
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Process attachments, if any
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine("Attachment Name: " + attachment.FileName);

                    // Save attachment to the same directory as the MSG file
                    string attachmentPath = Path.Combine(Path.GetDirectoryName(msgFilePath) ?? string.Empty, attachment.FileName);
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
