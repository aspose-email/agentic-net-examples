using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

// Author: Example code for loading a MSG file (e.g., exported from IBM Notes) into memory.

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file.
            string msgPath = "sample.msg";

            // Verify the file exists before attempting to load.
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

            // Load the MSG file into a MapiMessage instance.
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Example processing: display basic properties.
            Console.WriteLine($"Subject: {msg.Subject}");
            Console.WriteLine($"From: {msg.SenderName}");
            Console.WriteLine($"Body: {msg.Body}");

            // Iterate through attachments, if any.
            foreach (MapiAttachment attachment in msg.Attachments)
            {
                Console.WriteLine($"Attachment: {attachment.FileName}");

                // Save each attachment to the same directory as the MSG file.
                string attachmentPath = Path.Combine(Path.GetDirectoryName(msgPath) ?? string.Empty, attachment.FileName);
                try
                {
                    attachment.Save(attachmentPath);
                    Console.WriteLine($"Saved attachment to {attachmentPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
