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
            // Path to the MSG file that contains the email to be sent
            string msgPath = "email.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the email message from the MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Create and configure the SMTP client
                // Replace the placeholder values with actual server details and credentials
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                {
                    try
                    {
                        // Send the loaded message
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
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
