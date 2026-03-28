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
            // Path to the MSG file
            string msgPath = "message.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file inside a using block to ensure proper disposal
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Display basic metadata
                Console.WriteLine("Subject: " + message.Subject);
                Console.WriteLine("From: " + message.SenderName);
                Console.WriteLine("Sent: " + message.ClientSubmitTime);
                Console.WriteLine("Body:");
                Console.WriteLine(message.Body);

                // Process each attachment
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    Console.WriteLine("Attachment: " + attachment.FileName);

                    // Save the attachment to the current directory
                    string outputPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FileName);
                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved to: {outputPath}");
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
