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
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it.
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


            // Load the MSG file into a MapiMessage instance.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Display basic properties.
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.SenderName} <{message.SenderEmailAddress}>");
                Console.WriteLine($"Body: {message.Body}");

                // Process attachments, if any.
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Save each attachment to the current directory.
                    string attachmentPath = Path.Combine(Directory.GetCurrentDirectory(), attachment.FileName);

                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment: {attachment.FileName}");
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
