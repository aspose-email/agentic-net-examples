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
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Retrieve the subject
                string subject = msg.Subject ?? string.Empty;
                Console.WriteLine("Subject: " + subject);

                // Retrieve the sender information
                string senderName = msg.SenderName ?? string.Empty;
                string senderEmail = msg.SenderEmailAddress ?? string.Empty;
                string sender = string.IsNullOrEmpty(senderEmail) ? senderName : $"{senderName} <{senderEmail}>";
                Console.WriteLine("From: " + sender);

                // Retrieve recipient information from the Recipients collection
                if (msg.Recipients != null)
                {
                    foreach (MapiRecipient recipient in msg.Recipients)
                    {
                        string recipientName = recipient.DisplayName ?? string.Empty;
                        string recipientEmail = recipient.EmailAddress ?? string.Empty;
                        string formattedRecipient = string.IsNullOrEmpty(recipientEmail) ? recipientName : $"{recipientName} <{recipientEmail}>";
                        Console.WriteLine("To: " + formattedRecipient);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors
            Console.Error.WriteLine(ex.Message);
        }
    }
}
