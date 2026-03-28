using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "message.msg";

            // Ensure the MSG file exists
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


            // Load the MSG file into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Prepare SMTP client (replace with real server details)
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password"))
                {
                    try
                    {
                        // Forward the loaded message
                        client.Forward("sender@example.com", "recipient@example.com", mailMessage);
                        Console.WriteLine("Message forwarded successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to forward message: {ex.Message}");
                        return;
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
