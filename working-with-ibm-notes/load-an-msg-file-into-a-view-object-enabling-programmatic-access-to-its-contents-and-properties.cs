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
            // Define paths
            string inputMsgPath = "sample.msg";
            string attachmentsFolder = "Attachments";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                // Create a simple MapiMessage as a placeholder
                MapiMessage placeholderMessage = new MapiMessage(
                    "sender@example.com",
                    "receiver@example.com",
                    "Placeholder Subject",
                    "This is a placeholder message body.");

                // Save the placeholder MSG file
                placeholderMessage.Save(inputMsgPath);
                placeholderMessage.Dispose();
            }

            // Ensure the output directory for attachments exists
            if (!Directory.Exists(attachmentsFolder))
            {
                Directory.CreateDirectory(attachmentsFolder);
            }

            // Load the MSG file into a MapiMessage object
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Access basic properties
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Extract and save attachments, if any
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsFolder, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine("Saved attachment: " + attachmentPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to save attachment '{0}': {1}", attachment.FileName, ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
