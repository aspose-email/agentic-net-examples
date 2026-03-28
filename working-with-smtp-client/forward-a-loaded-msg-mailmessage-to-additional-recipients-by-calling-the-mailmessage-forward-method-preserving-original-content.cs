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
            string msgPath = "original.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder Subject", "Placeholder Body"))
                {
                    placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                }
            }

            // Load the original message.
            using (MailMessage originalMessage = MailMessage.Load(msgPath))
            {
                // Initialize the SMTP client.
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password"))
                {
                    try
                    {
                        // Prepare additional recipients for forwarding.
                        MailAddressCollection forwardRecipients = new MailAddressCollection();
                        forwardRecipients.Add("new1@example.com");
                        forwardRecipients.Add("new2@example.com");

                        // Forward the loaded message preserving its original content.
                        client.Forward(originalMessage.From.ToString(), forwardRecipients, originalMessage);
                        Console.WriteLine("Message forwarded successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during forwarding: {ex.Message}");
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
