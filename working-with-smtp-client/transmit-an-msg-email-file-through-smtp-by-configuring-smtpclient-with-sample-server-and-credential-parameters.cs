using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify the MSG file exists before attempting to load it
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


            // Load the MSG file into a MailMessage object
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                SmtpClient client = null;
                try
                {
                    // Initialize the SMTP client with sample server details
                    client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create SMTP client: " + ex.Message);
                    return;
                }

                // Ensure the client is disposed properly
                using (client)
                {
                    try
                    {
                        // Send the loaded message via SMTP
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error sending message: " + ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
